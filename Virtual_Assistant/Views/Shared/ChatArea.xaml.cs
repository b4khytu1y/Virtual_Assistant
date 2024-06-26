﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Virtual_Assistant.Controllers;
using Virtual_Assistant.Data;
using Virtual_Assistant.Data.Args;
using Virtual_Assistant.Models;
using Virtual_Assistant.Views.Shared.Popups;
using Virtual_Assistant.ChatHandlers;
using Virtual_Assistant.Utilities;
using Data_Settings = Virtual_Assistant.Data.Settings;
using Settings = Virtual_Assistant.Data.Settings;

namespace Virtual_Assistant.Views.Shared;

public partial class ChatArea : UserControl
{
    private RoleplayCharacter _character;
    private readonly ChatChannel _channel;
    private readonly Messages _messages;
    private readonly Data_Settings _settings;
    private readonly Personas _personas;
    private readonly ChatAreaController _chatAreaController;
    private readonly IChatHandler _chatHandler;
    private readonly Hotkeys _hotkeys;
    private readonly AudioRecorder _audioRecorder;
    private readonly WhisperManager _whisperManager;
    private readonly CharactersMenu _charactersMenu;
    private readonly ILogger<ChatArea> _logger;
    private readonly Characters _characters;
    private readonly ILifetimeScope _lifetimeScope;
    private readonly IVoiceGenerator _voiceGenerator;
    private readonly AudioPlayer _audioPlayer;
    private readonly AudioLevelCalculator _audioLevelCalculator;

    public IChatHandler ChatHandler => _chatHandler;

    public ChatArea(RoleplayCharacter character, ChatChannel channel, Messages messages,
        Data.Settings settings, Personas personas,
        ChatAreaController chatAreaController, IChatHandler chatHandler,
        Hotkeys hotkeys,
        AudioRecorder audioRecorder,
        WhisperManager whisperManager,
        CharactersMenu charactersMenu, ILogger<ChatArea> logger,
        ElevenlabsVoiceGenerator elevenlabsVoiceGenerator,
        Characters characters,
        ILifetimeScope lifetimeScope,
        IVoiceGenerator voiceGenerator,
        AudioPlayer audioPlayer, AudioLevelCalculator audioLevelCalculator)
    {
        ElevenlabsVoiceGenerator = elevenlabsVoiceGenerator;
        _character = character;
        _channel = channel;
        _messages = messages;
        _settings = settings;
        _personas = personas;
        _chatAreaController = chatAreaController;
        _chatHandler = chatHandler;
        _hotkeys = hotkeys;
        _audioRecorder = audioRecorder;
        _whisperManager = whisperManager;
        _charactersMenu = charactersMenu;
        _logger = logger;
        _characters = characters;
        _lifetimeScope = lifetimeScope;
        _voiceGenerator = voiceGenerator;
        _audioPlayer = audioPlayer;
        _audioLevelCalculator = audioLevelCalculator;

        InitializeComponent();

        _whisperManager.DoTranscribe = true;
        _chatHandler.CompleteMessageGenerated += ChatHandlerOnCompleteMessageGenerated;
        _whisperManager.TranscribeFinished += WhisperManagerOnTranscribeFinished;
        _whisperManager.Transcribing += WhisperManagerOnTranscribing;
        audioRecorder.AudioRecordingStarted += AudioRecorderOnAudioRecordingStarted;
        audioRecorder.AudioRecordingStopped += AudioRecorderOnAudioRecordingStopped;
        audioLevelCalculator.AudioLevelCalculated += AudioRecorderOnAudioLevelReceived;
    }

    private void AudioRecorderOnAudioRecordingStopped(object? sender, int e)
    {
        Dispatcher.Invoke(() => { MessageSendBorder.Opacity = 0; });
    }

    private void AudioRecorderOnAudioLevelReceived(object? sender, AudioLevelData e)
    {
        Dispatcher.Invoke(() =>
        {
            var audioLevel = e.AudioLevel;

            MessageSendBorder.Opacity = audioLevel / _maxVoiceLevel;

            // if (audioLevel > _maxVoiceLevel)
            //     _maxVoiceLevel = audioLevel;
        });
    }

    private void ChatHandlerOnCompleteMessageGenerated(object? sender, ChatMessage e)
    {
        if (_settings.CachedSettings is { EnableElevenlabs: true })
            _ = Task.Run(async () =>
            {
                var generateVoice =
                    await _voiceGenerator.GenerateVoiceAsync(e.Message, RoleplayCharacter.ElevenlabsSelectedVoice);

                if (generateVoice is null)
                    return;

                var voiceWaveArray = generateVoice.ToArray();

                if (voiceWaveArray.Any())
                    await _audioPlayer.PlayMp3FromByteArrayAsync(voiceWaveArray);
            });
    }

    private void WhisperManagerOnTranscribing(object? sender, EventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            SendButton.IsEnabled = false;
            SendButtonText.Text = "Transcribing";
        });
    }

    private double _maxVoiceLevel = 100;


    private void AudioRecorderOnAudioRecordingStarted(object? sender, EventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            SendButton.IsEnabled = false;
            SendButtonText.Text = "Listening";
        });
    }

    private void WhisperManagerOnTranscribeFinished(object? sender, string e)
    {
        Dispatcher.Invoke(() =>
        {
            SendButton.IsEnabled = true;
            SendButtonText.Text = "Send";
            VoiceBorder.Opacity = 0;
            MessageInput.Text = e;
            SendMessageFromUi();
        });
    }

    public Hotkey VoiceHotkey { get; set; }
    public ObservableCollection<ChatMessage> ChatMessages { get; set; } = new();

    public RoleplayCharacter RoleplayCharacter
    {
        get => _character;
        set => _character = value;
    }

    public ElevenlabsVoiceGenerator ElevenlabsVoiceGenerator { get; set; }

    public ChatChannel ChatChannel => _channel;

    public event EventHandler<string> MessageSend;

    public void SendMessageFromUi()
    {
        MessageSend?.Invoke(this, MessageInput.Text.Clone() as string);

        // clear text
        MessageInput.Text = string.Empty;
    }

    private void SendMessageButton(object sender, RoutedEventArgs e)
    {
        SendMessageFromUi();
    }

    private void TextBoxTextChanged(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.Enter)
            SendMessageFromUi();
    }

    public long CurrentMessageId { get; set; } = long.MaxValue;


    private void ChatAreaLoaded(object sender, RoutedEventArgs e)
    {
        _ = Task.Run(async () =>
        {
            VoiceHotkey = await _hotkeys.GetOrAddHotkeyAsync("Voice", Key.LeftAlt, Key.LeftCtrl);

            var currentMessages = await _messages.GetMessagesAsync(_channel.Id, CurrentMessageId);

            if (currentMessages is null || !currentMessages.Any())
            {
                // we've probably reached the first message
                return;
            }

            CurrentMessageId = currentMessages.MinBy(x => x.Id).Id;

            Dispatcher.Invoke(() =>
            {
                VoiceHotkeyText.Text = VoiceHotkey.VirtualKeyCodes.ToHotkeyString();

                foreach (var chatMessage in currentMessages.OrderByDescending(x => x.Id))
                    AddChatBasedOnIdLocation(chatMessage);
            });
        });

        _chatAreaController.MessageFromCurrentUser += ChatAreaControllerOnMessageFromCurrentUser;
    }

    private void ChatAreaControllerOnMessageFromCurrentUser(object? sender, ChatMessage e)
    {
        Dispatcher.Invoke(() => { ChatMessages.Add(e); });
    }

    public void AddChatBasedOnIdLocation(ChatMessage chatMessage)
    {
        // only add if the chat channel are the same

        if (chatMessage.ChatChannel is null || _channel.Id != chatMessage.ChatChannel.Id)
        {
            return;
        }

        Dispatcher.Invoke(() =>
        {
            var indexToInsert = ChatMessages
                .Select((msg, index) => new { Message = msg, Index = index })
                .OrderBy(item => item.Message.Id)
                .TakeWhile(item => item.Message.Id < chatMessage.Id)
                .LastOrDefault()?.Index + 1 ?? 0;

            if (indexToInsert < 0)
                indexToInsert = 0;

            ChatMessages.Insert(indexToInsert, chatMessage);

            // goofy ahh ui update
            ChatScroll.ScrollToEnd();

            UpdateLayout();

            ChatScroll.ScrollToEnd();
        });
    }

    private void ChatAreaDisposed(object sender, RoutedEventArgs e)
    {
        _chatHandler.CompleteMessageGenerated -= ChatHandlerOnCompleteMessageGenerated;
        _whisperManager.Transcribing -= WhisperManagerOnTranscribing;
        _whisperManager.TranscribeFinished -= WhisperManagerOnTranscribeFinished;
        _audioRecorder.AudioRecordingStarted -= AudioRecorderOnAudioRecordingStarted;
        _audioLevelCalculator.AudioLevelCalculated -= AudioRecorderOnAudioLevelReceived;
        _chatAreaController.MessageFromCurrentUser -= ChatAreaControllerOnMessageFromCurrentUser;
        _audioRecorder.AudioRecordingStopped -= AudioRecorderOnAudioRecordingStopped;

        _logger.LogInformation($"ChatArea {this.GetHashCode()} is being disposed!");
    }

    private void CharacterSettingsClicked(object sender, MouseButtonEventArgs e)
    {
        var characterEditor = _lifetimeScope.Resolve<CharacterEdit>();

        _ = Task.Run(async () =>
        {
            RoleplayCharacter = await _characters.RefreshRoleplayCharacterAsync(RoleplayCharacter);

            Dispatcher.Invoke(() =>
            {
                characterEditor.RoleplayCharacter = RoleplayCharacter;

                DialogArea.Children.Add(characterEditor);

                characterEditor.RoleplayCharacterChanged += (o, character) =>
                {
                    _ = Task.Run(async () => { await _characters.AddOrUpdateCharacterAsync(character); });
                };

                characterEditor.CloseTriggered += (o, args) => { DialogArea.Children.Clear(); };
            });
        });
    }
}
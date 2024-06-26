﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Virtual_Assistant.Models;

namespace Virtual_Assistant.Views.Shared;
public partial class CharacterItem : UserControl
{
    // for wpf previews to work
    public CharacterItem()
    {
        InitializeComponent();
    }

    public event EventHandler<RoleplayCharacter?> OnActiveRequest;
    public event EventHandler<RoleplayCharacter?> OnDeleteRequest;

    #region Character Name Property

    public String CharacterName
    {
        get { return (String)GetValue(CharacterNameProperty); }
        set { SetValue(CharacterNameProperty, value); }
    }

    public static readonly DependencyProperty CharacterNameProperty =
        DependencyProperty.Register(nameof(CharacterName), typeof(String), typeof(CharacterItem),
            new FrameworkPropertyMetadata(null)
            {
                //  It's read-write, so make it bind both ways by default
                BindsTwoWayByDefault = true
            });

    #endregion

    #region Character Object Property

    public RoleplayCharacter RoleplayCharacter
    {
        get { return (RoleplayCharacter)GetValue(RoleplayCharacterProperty); }
        set { SetValue(RoleplayCharacterProperty, value); }
    }

    public static readonly DependencyProperty RoleplayCharacterProperty =
        DependencyProperty.Register(nameof(RoleplayCharacter), typeof(RoleplayCharacter), typeof(CharacterItem),
            new FrameworkPropertyMetadata(null)
            {
            });

    #endregion

    #region Character Image Property

    public ImageSource Image
    {
        get { return (ImageSource)GetValue(ImageProperty); }
        set { SetValue(ImageProperty, value); }
    }

    public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(CharacterItem),
            new FrameworkPropertyMetadata(null));

    #endregion

    private void ItemClicked(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton is MouseButton.Left)
            OnActiveRequest?.Invoke(this, RoleplayCharacter);
    }

    private void CharacterItemLoaded(object sender, RoutedEventArgs e)
    {
        ToolTipService.SetToolTip(MainGrid, CharacterName);

        Badge.Visibility = Visibility.Collapsed;

        if (RoleplayCharacter.IsCharacterAi)
            SetBadge("chai");
        else
        {
            SetBadge("llama");
        }
    }

    public void SetBadge(string badge)
    {
        BadgeText.Text = badge;

        Badge.Visibility = Visibility.Visible;
    }

    private void DeleteCharacter(object sender, RoutedEventArgs e)
    {
        OnDeleteRequest?.Invoke(this, RoleplayCharacter);
    }
}
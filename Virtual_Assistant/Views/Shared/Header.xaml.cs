﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Virtual_Assistant.Data;
using Virtual_Assistant.Utilities;
using Virtual_Assistant.Views.Index;
using Application = System.Windows.Application;
using UserControl = System.Windows.Controls.UserControl;

namespace Virtual_Assistant.Views.Shared;

public partial class Header : UserControl
{
    private readonly Views.Index.Settings _settings;
    private readonly ProperShutdownHandler _properShutdownHandler;

    public Header(Views.Index.Settings settings, ProperShutdownHandler properShutdownHandler)
    {
        _settings = settings;
        _properShutdownHandler = properShutdownHandler;
        InitializeComponent();
    }

    private void HeaderMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e is { ChangedButton: MouseButton.Left, ButtonState: MouseButtonState.Pressed })
            this.GetCurrentWindow()?.DragMove();
    }

    private void CloseClicked(object sender, MouseButtonEventArgs e)
    {
        // kill puppeteer first
        _properShutdownHandler.ShutdownProperly();
    }

    private void MinimizeClicked(object sender, MouseButtonEventArgs e)
    {
        if (this.GetCurrentWindow() is MainWindow mainWindow)
            mainWindow.WindowState = mainWindow.WindowState == WindowState.Minimized
                ? WindowState.Normal
                : WindowState.Minimized;
    }


    private void MaximizeClicked(object sender, MouseButtonEventArgs e)
    {
        if (this.GetCurrentWindow() is MainWindow mainWindow)
            mainWindow.WindowState = mainWindow.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
    }

    private void GithubClicked(object sender, MouseButtonEventArgs e) =>
        "https://github.com/AlizerUncaged/waifu-desktop".OpenAsUrl();

    private void SettingsClicked(object sender, MouseButtonEventArgs e)
    {
        if (this.GetCurrentWindow() is MainWindow mainWindow)
        {
            mainWindow.SetTopView(_settings);
        }
    }
}
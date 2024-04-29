using System.Windows;
using System.Windows.Controls;
using Virtual_Assistant.Data;

namespace Virtual_Assistant.Views.Index;

public partial class Welcome : UserControl
{
    private readonly StartupCheck _startupCheck;

    public Welcome(StartupCheck startupCheck)
    {
        _startupCheck = startupCheck;

        InitializeComponent();
    }


    private void ControlLoaded(object sender, RoutedEventArgs e)
    {
        LoadingMessage.Text = _startupCheck.CurrentLog;
        
        _startupCheck.OnLogChanged += (o, s) => { this.Dispatcher.Invoke(() => { LoadingMessage.Text = s; }); };
    }
}
using System.Windows;

namespace Virtual_Assistant;

public interface IPopup
{
    public event EventHandler CloseTriggered;
    public event EventHandler<FrameworkElement> ReplaceTriggered;
}
using System.Windows;

namespace Alphicsh.Tasks.Playground;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new TaskTestViewModel();
    }
}
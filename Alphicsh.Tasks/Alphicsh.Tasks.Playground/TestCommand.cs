using System.Windows.Input;

namespace Alphicsh.Tasks.Playground;

public class TestCommand : ICommand
{
    private Action CommandAction { get; }

    public TestCommand(Action commandAction)
    {
        CommandAction = commandAction;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        CommandAction();
    }
}

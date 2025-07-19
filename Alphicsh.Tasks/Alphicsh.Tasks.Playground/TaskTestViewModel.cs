using System.ComponentModel;
using System.Windows.Input;
using Alphicsh.Tasks.Channels;
using Alphicsh.Tasks.Progress;

namespace Alphicsh.Tasks.Playground;

public class TaskTestViewModel : INotifyPropertyChanged
{
    private ITaskChannel<string> TransformTaskChannel { get; }

    public TaskTestViewModel()
    {
        TransformTaskChannel = new LastTaskChannel<string>();
        TransformTaskChannel.GetProgressSubjectOf<IntegerProgress>().ProgressChanged += (sender, progress) => TaskProgress = progress.Current;
        TransformTaskChannel.TaskCompleted += (sender, result) => OutputText = result;

        TransformCommand = new TestCommand(Transform);
        CancelCommand = new TestCommand(CancelTask);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void RaisePropertyChanged(string property)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }

    private string InputTextField = "Lorem";
    public string InputText
    {
        get => InputTextField;
        set
        {
            if (InputTextField == value)
                return;

            InputTextField = value;
            RaisePropertyChanged(nameof(InputText));
        }
    }

    private string OutputTextField = "Ipsum";
    public string OutputText
    {
        get => OutputTextField;
        set
        {
            if (OutputTextField == value)
                return;

            OutputTextField = value;
            RaisePropertyChanged(nameof(OutputText));
        }
    }

    private int TaskProgressField = 0;
    public int TaskProgress
    {
        get => TaskProgressField;
        set
        {
            if (TaskProgressField == value)
                return;

            TaskProgressField = value;
            RaisePropertyChanged(nameof(TaskProgress));
        }
    }

    public ICommand TransformCommand { get; }
    private void Transform()
    {
        var transformTask = ManagedTask.Create(DoTransform);
        TransformTaskChannel.AcceptTask(transformTask);
    }

    public ICommand CancelCommand { get; }
    private void CancelTask()
    {
        TransformTaskChannel.Cancel();
    }

    private async Task<string> DoTransform(CancellationToken cancellationToken, IProgress<object> progressReporter)
    {
        for (var i = 0; i < 100; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            progressReporter.Report(new IntegerProgress(i, 100));
            await Task.Delay(30);
        }
        progressReporter.Report(new IntegerProgress(100, 100));
        return InputText.ToUpperInvariant();
    }
}

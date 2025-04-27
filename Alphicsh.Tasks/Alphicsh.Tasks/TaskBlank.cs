using System.Threading.Tasks;

namespace Alphicsh.Tasks;

public struct TaskBlank
{
    public static async Task<TaskBlank> FromTask(Task task)
    {
        await task;
        return default;
    }
}

using TaskCore.Common;
using TaskCore.DTOs;
using TaskCore.Models;

namespace TaskCore.Services;

public interface ITaskManager
{   
    Task<Result<TaskItem>> CreateTaskAsync(CreateTaskRequest request, CancellationToken ct = default);

    Task<Result> ChangeStatusAsync(int taskId, ChangeStatusRequest request, CancellationToken ct = default);

   
    Task<Result> CloseTaskAsync(int taskId, CancellationToken ct = default);

    Task<Result<IEnumerable<TaskItem>>> GetUserTasksAsync(int userId, CancellationToken ct = default);
}

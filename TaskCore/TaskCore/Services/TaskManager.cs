using Microsoft.EntityFrameworkCore;
using TaskCore.Common;
using TaskCore.Data;
using TaskCore.DTOs;
using TaskCore.Models;

namespace TaskCore.Services;

public class TaskManager : ITaskManager
{
    private readonly AppDbContext _db;
    private readonly IEnumerable<ITaskTypeHandler> _handlers;

    public TaskManager(AppDbContext db, IEnumerable<ITaskTypeHandler> handlers)
    {
        _db = db;
        _handlers = handlers;
    }

    
    public async Task<Result<TaskItem>> CreateTaskAsync(CreateTaskRequest request, CancellationToken ct = default)
    {
        // Validate the task type has a registered handler
        var handler = _handlers.FirstOrDefault(h => h.TaskTypeId == request.TaskTypeId);
        if (handler is null)
            return Result<TaskItem>.Fail($"Unknown task type '{request.TaskTypeId}'.");

        // Validate the assigned user exists
        var userExists = await _db.Users.AnyAsync(u => u.Id == request.AssignedUserId, ct);
        if (!userExists)
            return Result<TaskItem>.Fail($"User {request.AssignedUserId} not found.");

        var task = new TaskItem
        {
            TaskTypeId       = request.TaskTypeId,
            AssignedUserId   = request.AssignedUserId,
            CurrentStatus    = 1,
            IsOpen           = true,
            CustomData       = null
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync(ct);

        return Result<TaskItem>.Ok(task);
    }

   
    public async Task<Result> ChangeStatusAsync(int taskId, ChangeStatusRequest request, CancellationToken ct = default)
    {
        var task = await _db.Tasks.FindAsync([taskId], ct);
        if (task is null)
            return Result.Fail($"Task {taskId} not found.");

        if (!task.IsOpen)
            return Result.Fail("Closed tasks are immutable.");

        var handler = _handlers.First(h => h.TaskTypeId == task.TaskTypeId);

        // Rule 4: forward moves must be sequential (no skipping)
        if (request.TargetStatus > task.CurrentStatus + 1)
            return Result.Fail($"Cannot skip from status {task.CurrentStatus} to {request.TargetStatus}. Forward moves must be sequential.");

        // Rule 7a: validate type-specific data requirements for every status change
        var validation = handler.ValidateTransitionData(request.TargetStatus, request.CustomData);
        if (!validation.IsSuccess)
            return validation;

        // Rule 7b: validate the next assigned user exists
        var userExists = await _db.Users.AnyAsync(u => u.Id == request.NextAssignedUserId, ct);
        if (!userExists)
            return Result.Fail($"User {request.NextAssignedUserId} not found.");

        task.CurrentStatus   = request.TargetStatus;
        task.AssignedUserId  = request.NextAssignedUserId;
        task.CustomData      = request.CustomData;

        await _db.SaveChangesAsync(ct);
        return Result.Ok();
    }

   
    public async Task<Result> CloseTaskAsync(int taskId, CancellationToken ct = default)
    {
        var task = await _db.Tasks.FindAsync([taskId], ct);
        if (task is null)
            return Result.Fail($"Task {taskId} not found.");

        if (!task.IsOpen)
            return Result.Fail("Task is already closed.");

        var handler = _handlers.First(h => h.TaskTypeId == task.TaskTypeId);

        // Rule 6: may only close at the final status
        if (task.CurrentStatus != handler.FinalStatus)
            return Result.Fail($"Task can only be closed at status {handler.FinalStatus}. Current status is {task.CurrentStatus}.");

        task.IsOpen = false;

        await _db.SaveChangesAsync(ct);
        return Result.Ok();
    }

  
    public async Task<Result<IEnumerable<TaskItem>>> GetUserTasksAsync(int userId, CancellationToken ct = default)
    {
        var userExists = await _db.Users.AnyAsync(u => u.Id == userId, ct);
        if (!userExists)
            return Result<IEnumerable<TaskItem>>.Fail($"User {userId} not found.");

        var tasks = await _db.Tasks
            .Where(t => t.AssignedUserId == userId)
            .ToListAsync(ct);

        return Result<IEnumerable<TaskItem>>.Ok(tasks);
    }

}

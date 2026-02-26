using Microsoft.AspNetCore.Mvc;
using TaskCore.DTOs;
using TaskCore.Services;

namespace TaskCore.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskManager _taskManager;
    private readonly IEnumerable<ITaskTypeHandler> _handlers;

    public TasksController(ITaskManager taskManager, IEnumerable<ITaskTypeHandler> handlers)
    {
        _taskManager = taskManager;
        _handlers = handlers;
    }

    // GET api/tasks/metadata
    [HttpGet("metadata")]
    public IActionResult GetMetadata()
    {
        var metadata = _handlers
            .OrderBy(h => h.TaskTypeId)
            .Select(h => new
            {
                taskTypeId = h.TaskTypeId,
                typeName = h.TypeName,
                statuses = h.Statuses.Select(s => new
                {
                    statusNumber = s.StatusNumber,
                    statusName = s.StatusName,
                }),
            });

        return Ok(metadata);
    }

    // POST api/tasks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request, CancellationToken ct)
    {
        var result = await _taskManager.CreateTaskAsync(request, ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Data);
    }

    // PATCH api/tasks/{id}/status
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusRequest request, CancellationToken ct)
    {
        var result = await _taskManager.ChangeStatusAsync(id, request, ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return NoContent();
    }

    // POST api/tasks/{id}/close
    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(int id, CancellationToken ct)
    {
        var result = await _taskManager.CloseTaskAsync(id, ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return NoContent();
    }

    // GET api/tasks/user/{userId}
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await _taskManager.GetUserTasksAsync(userId, ct);
        if (!result.IsSuccess)
            return NotFound(new { error = result.Error });

        return Ok(result.Data);
    }
}

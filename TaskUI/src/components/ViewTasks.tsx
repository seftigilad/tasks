import { useEffect, useState } from "react";
import { api, TaskItem, TaskTypeMeta } from "../api";

// ---- Component ----

export default function ViewTasks() {
  const [taskTypes, setTaskTypes] = useState<TaskTypeMeta[]>([]);
  const [userId, setUserId] = useState("");
  const [tasks, setTasks] = useState<TaskItem[] | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    api.getMetadata().then(setTaskTypes);
  }, []);

  function getTypeName(taskTypeId: number): string {
    return taskTypes.find((t) => t.taskTypeId === taskTypeId)?.typeName ?? `Type ${taskTypeId}`;
  }

  function getStatusName(taskTypeId: number, statusNumber: number): string {
    const type = taskTypes.find((t) => t.taskTypeId === taskTypeId);
    return type?.statuses.find((s) => s.statusNumber === statusNumber)?.statusName ?? `Status ${statusNumber}`;
  }

  function handleFetch() {
    const id = parseInt(userId, 10);
    if (isNaN(id) || id <= 0) {
      setError("Please enter a valid user ID.");
      return;
    }
    setLoading(true);
    setError(null);
    setTasks(null);
    api
      .getUserTasks(id)
      .then(setTasks)
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }

  return (
    <div className="view-tasks">
      <h2>View Tasks</h2>

      <div className="fetch-bar">
        <input
          type="number"
          min={1}
          placeholder="User ID"
          value={userId}
          onChange={(e) => setUserId(e.target.value)}
          className="user-id-input"
        />
        <button onClick={handleFetch} disabled={loading} className="fetch-btn">
          {loading ? "Loading…" : "Fetch"}
        </button>
      </div>

      {error && <p className="state-msg error">{error}</p>}

      {tasks !== null &&
        (tasks.length === 0 ? (
          <p className="state-msg">No tasks assigned to this user.</p>
        ) : (
          <table className="task-table">
            <thead>
              <tr>
                <th>#</th>
                <th>Type</th>
                <th>Status</th>
                <th>State</th>
              </tr>
            </thead>
            <tbody>
              {tasks.map((task) => (
                <tr key={task.id}>
                  <td>{task.id}</td>
                  <td>{getTypeName(task.taskTypeId)}</td>
                  <td>{getStatusName(task.taskTypeId, task.currentStatus)}</td>
                  <td>
                    <span
                      className={`badge ${task.isOpen ? "badge-open" : "badge-closed"}`}
                    >
                      {task.isOpen ? "Open" : "Closed"}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ))}
    </div>
  );
}

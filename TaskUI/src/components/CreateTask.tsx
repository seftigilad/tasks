import { useEffect, useState } from 'react'
import { api, TaskTypeMeta } from '../api'

export default function CreateTask() {
  const [taskTypes, setTaskTypes] = useState<TaskTypeMeta[]>([])
  const [taskTypeId, setTaskTypeId] = useState<number>(0)
  const [assignedUserId, setAssignedUserId] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [successId, setSuccessId] = useState<number | null>(null)

  useEffect(() => {
    api.getMetadata().then((data) => {
      setTaskTypes(data)
      if (data.length > 0) setTaskTypeId(data[0].taskTypeId)
    })
  }, [])

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    const userId = parseInt(assignedUserId, 10)
    if (isNaN(userId) || userId <= 0) {
      setError('Please enter a valid user ID.')
      return
    }
    setLoading(true)
    setError(null)
    setSuccessId(null)
    api
      .createTask(taskTypeId, userId)
      .then((task) => {
        setSuccessId(task.id)
        setAssignedUserId('')
      })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false))
  }

  return (
    <div className="create-task">
      <h2>Create Task</h2>

      <form onSubmit={handleSubmit} className="create-task-form">
        <div className="form-row">
          <label htmlFor="taskType">Task Type</label>
          <select
            id="taskType"
            value={taskTypeId}
            onChange={(e) => setTaskTypeId(Number(e.target.value))}
            className="user-id-input"
          >
            {taskTypes.map((t) => (
              <option key={t.taskTypeId} value={t.taskTypeId}>
                {t.typeName}
              </option>
            ))}
          </select>
        </div>

        <div className="form-row">
          <label htmlFor="assignedUser">Assigned User ID</label>
          <input
            id="assignedUser"
            type="number"
            min={1}
            placeholder="User ID"
            value={assignedUserId}
            onChange={(e) => setAssignedUserId(e.target.value)}
            className="user-id-input"
          />
        </div>

        <button type="submit" disabled={loading} className="fetch-btn">
          {loading ? 'Creating…' : 'Create Task'}
        </button>
      </form>

      {error && <p className="state-msg error">{error}</p>}
      {successId !== null && (
        <p className="state-msg success">Task #{successId} created successfully.</p>
      )}
    </div>
  )
}

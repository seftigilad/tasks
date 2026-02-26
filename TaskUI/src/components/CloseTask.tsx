import { useState } from 'react'
import { api } from '../api'

export default function CloseTask() {
  const [taskId, setTaskId] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault()

    const id = parseInt(taskId, 10)
    if (isNaN(id) || id <= 0) {
      setError('Please enter a valid task ID.')
      return
    }

    setLoading(true)
    setError(null)
    setSuccess(null)

    api
      .closeTask(id)
      .then(() => {
        setSuccess(`Task #${id} closed successfully.`)
        setTaskId('')
      })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false))
  }

  return (
    <div className="create-task">
      <h2>Close Task</h2>

      <form onSubmit={handleSubmit} className="create-task-form">
        <div className="form-row">
          <label htmlFor="ct-taskId">Task ID</label>
          <input
            id="ct-taskId"
            type="number"
            min={1}
            placeholder="Task ID"
            value={taskId}
            onChange={(e) => setTaskId(e.target.value)}
            className="user-id-input"
          />
        </div>

        <button type="submit" disabled={loading} className="fetch-btn">
          {loading ? 'Closing…' : 'Close Task'}
        </button>
      </form>

      {error && <p className="state-msg error">{error}</p>}
      {success && <p className="state-msg success">{success}</p>}
    </div>
  )
}

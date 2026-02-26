import { useState } from 'react'
import { api } from '../api'

export default function ChangeStatus() {
  const [taskId, setTaskId] = useState('')
  const [targetStatus, setTargetStatus] = useState('')
  const [nextUserId, setNextUserId] = useState('')
  const [customData, setCustomData] = useState('')
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
    const status = parseInt(targetStatus, 10)
    if (isNaN(status) || status <= 0) {
      setError('Please enter a valid target status.')
      return
    }
    const userId = parseInt(nextUserId, 10)
    if (isNaN(userId) || userId <= 0) {
      setError('Please enter a valid next assigned user ID.')
      return
    }

    setLoading(true)
    setError(null)
    setSuccess(null)

    api
      .changeStatus(id, status, userId, customData.trim() || null)
      .then(() => {
        setSuccess(`Task #${id} moved to status ${status}.`)
        setTaskId('')
        setTargetStatus('')
        setNextUserId('')
        setCustomData('')
      })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false))
  }

  return (
    <div className="create-task">
      <h2>Change Status</h2>

      <form onSubmit={handleSubmit} className="create-task-form">
        <div className="form-row">
          <label htmlFor="cs-taskId">Task ID</label>
          <input
            id="cs-taskId"
            type="number"
            min={1}
            placeholder="Task ID"
            value={taskId}
            onChange={(e) => setTaskId(e.target.value)}
            className="user-id-input"
          />
        </div>

        <div className="form-row">
          <label htmlFor="cs-targetStatus">Target Status</label>
          <input
            id="cs-targetStatus"
            type="number"
            min={1}
            placeholder="e.g. 2"
            value={targetStatus}
            onChange={(e) => setTargetStatus(e.target.value)}
            className="user-id-input"
          />
        </div>

        <div className="form-row">
          <label htmlFor="cs-nextUser">Next Assigned User ID</label>
          <input
            id="cs-nextUser"
            type="number"
            min={1}
            placeholder="User ID"
            value={nextUserId}
            onChange={(e) => setNextUserId(e.target.value)}
            className="user-id-input"
          />
        </div>

        <div className="form-row">
          <label htmlFor="cs-customData">Custom Data (optional)</label>
          <input
            id="cs-customData"
            type="text"
            placeholder="Required data for this status"
            value={customData}
            onChange={(e) => setCustomData(e.target.value)}
            className="user-id-input"
          />
        </div>

        <button type="submit" disabled={loading} className="fetch-btn">
          {loading ? 'Saving…' : 'Change Status'}
        </button>
      </form>

      {error && <p className="state-msg error">{error}</p>}
      {success && <p className="state-msg success">{success}</p>}
    </div>
  )
}

const BASE_URL = 'https://localhost:44333'

// ---- Domain types ----

export type TaskStatusMeta = {
  statusNumber: number
  statusName: string
}

export type TaskTypeMeta = {
  taskTypeId: number
  typeName: string
  statuses: TaskStatusMeta[]
}

export type TaskItem = {
  id: number
  taskTypeId: number       // 1 = Procurement, 2 = Development
  currentStatus: number
  isOpen: boolean
  assignedUserId: number
  customData: string | null
}

// ---- Helpers ----

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  })
  const text = await res.text()
  if (!res.ok) throw new Error(`${res.status} ${res.statusText}: ${text}`)
  return (text ? JSON.parse(text) : null) as T
}

// ---- API calls ----

export const api = {
  getUserTasks: (userId: number) =>
    request<TaskItem[]>(`/api/tasks/user/${userId}`),

  createTask: (taskTypeId: number, assignedUserId: number) =>
    request<TaskItem>('/api/tasks', {
      method: 'POST',
      body: JSON.stringify({ taskTypeId, assignedUserId }),
    }),

  changeStatus: (
    id: number,
    targetStatus: number,
    nextAssignedUserId: number,
    customData: string | null,
  ) =>
    request<TaskItem>(`/api/tasks/${id}/status`, {
      method: 'PATCH',
      body: JSON.stringify({ targetStatus, nextAssignedUserId, customData }),
    }),

  closeTask: (id: number) =>
    request<void>(`/api/tasks/${id}/close`, { method: 'POST' }),

  getMetadata: () =>
    request<TaskTypeMeta[]>('/api/tasks/metadata'),
}

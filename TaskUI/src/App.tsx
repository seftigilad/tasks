import { useState } from 'react'
import './App.css'
import ViewTasks from './components/ViewTasks'
import CreateTask from './components/CreateTask'
import ChangeStatus from './components/ChangeStatus'
import CloseTask from './components/CloseTask'

type Page = 'view-tasks' | 'create-task' | 'change-status' | 'close-task'

function App() {
  const [activePage, setActivePage] = useState<Page | null>(null)

  return (
    <div className="app-layout">
      <nav className="sidebar">
        <div className="sidebar-header">
          <h2>Task Manager</h2>
        </div>
        <ul className="nav-menu">
          <li
            className={`nav-item ${activePage === 'create-task' ? 'active' : ''}`}
            onClick={() => setActivePage('create-task')}
          >
            Create Task
          </li>
          <li
            className={`nav-item ${activePage === 'change-status' ? 'active' : ''}`}
            onClick={() => setActivePage('change-status')}
          >
            Change Status
          </li>
          <li
            className={`nav-item ${activePage === 'close-task' ? 'active' : ''}`}
            onClick={() => setActivePage('close-task')}
          >
            Close Task
          </li>
          <li
            className={`nav-item ${activePage === 'view-tasks' ? 'active' : ''}`}
            onClick={() => setActivePage('view-tasks')}
          >
            View Tasks
          </li>
        </ul>
      </nav>

      <main className="main-content">
        {activePage === null && (
          <div className="placeholder">
            <p>Select an option from the menu.</p>
          </div>
        )}
        {activePage === 'create-task' && <CreateTask />}
        {activePage === 'change-status' && <ChangeStatus />}
        {activePage === 'close-task' && <CloseTask />}
        {activePage === 'view-tasks' && <ViewTasks />}
      </main>
    </div>
  )
}

export default App

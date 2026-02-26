import { createContext, useContext, useEffect, useState, ReactNode } from 'react'
import { api, TaskTypeMeta } from './api'

type MetadataContextValue = {
  typeLabels: Record<number, string>
  statusLabels: Record<number, Record<number, string>>
  loaded: boolean
}

const MetadataContext = createContext<MetadataContextValue>({
  typeLabels: {},
  statusLabels: {},
  loaded: false,
})

export function MetadataProvider({ children }: { children: ReactNode }) {
  const [value, setValue] = useState<MetadataContextValue>({
    typeLabels: {},
    statusLabels: {},
    loaded: false,
  })

  useEffect(() => {
    api.getMetadata().then((data: TaskTypeMeta[]) => {
      const typeLabels: Record<number, string> = {}
      const statusLabels: Record<number, Record<number, string>> = {}

      for (const t of data) {
        typeLabels[t.taskTypeId] = t.typeName
        statusLabels[t.taskTypeId] = {}
        for (const s of t.statuses) {
          statusLabels[t.taskTypeId][s.statusNumber] = s.statusName
        }
      }

      setValue({ typeLabels, statusLabels, loaded: true })
    })
  }, [])

  return (
    <MetadataContext.Provider value={value}>
      {children}
    </MetadataContext.Provider>
  )
}

export function useMetadata() {
  return useContext(MetadataContext)
}

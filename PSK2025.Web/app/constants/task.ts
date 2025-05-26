import { TaskPriority, TaskStatus } from '~/types/task'
import type { ComponentConfig } from '#ui/types/utils'
import type { AppConfig } from '@nuxt/schema'
import type theme from '#build/ui/badge'

type Badge = ComponentConfig<typeof theme, AppConfig, 'badge'>;

const taskStatusMap: Record<TaskStatus, { text: string, color: Badge['variants']['color'] }> = {
  [TaskStatus.NOT_STARTED]: { text: 'Not started', color: 'warning' },
  [TaskStatus.IN_PROGRESS]: { text: 'Active', color: 'success' },
  [TaskStatus.COMPLETED]: { text: 'Completed', color: 'primary' }
}

export const taskStatusColor = (v: TaskStatus): Badge['variants']['color'] => {
  return taskStatusMap[v]?.color
}

export const taskStatusText = (v: TaskStatus): string => {
  return taskStatusMap[v]?.text ?? v
}

const taskPriorityMap: Record<TaskPriority, { text: string, color: Badge['variants']['color'] }> = {
  [TaskPriority.LOW]: { text: 'Low', color: 'primary' },
  [TaskPriority.MEDIUM]: { text: 'Medium', color: 'warning' },
  [TaskPriority.HIGH]: { text: 'High', color: 'error' }
}

export const taskPriorityColor = (v: TaskPriority): Badge['variants']['color'] => {
  return taskPriorityMap[v]?.color
}

export const taskPriorityText = (v: TaskPriority): string => {
  return taskPriorityMap[v]?.text ?? v
}

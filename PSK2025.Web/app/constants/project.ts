import { ProjectStatus } from '~/types/project'
import type { ComponentConfig } from '#ui/types/utils'
import type { AppConfig } from '@nuxt/schema'
import type theme from '#build/ui/badge'

type Badge = ComponentConfig<typeof theme, AppConfig, 'badge'>;

const projectStatusMap: Record<ProjectStatus, { text: string, color: Badge['variants']['color'] }> = {
  [ProjectStatus.PLANNED]: { text: 'Planned', color: 'warning' },
  [ProjectStatus.ACTIVE]: { text: 'Active', color: 'success' },
  [ProjectStatus.COMPLETED]: { text: 'Completed', color: 'primary' },
  [ProjectStatus.ARCHIVED]: { text: 'Archived', color: 'neutral' }
}

export const projectStatusColor = (v: ProjectStatus): Badge['variants']['color'] => {
  return projectStatusMap[v]?.color
}

export const projectStatusText = (v: ProjectStatus): string => {
  return projectStatusMap[v]?.text ?? v
}

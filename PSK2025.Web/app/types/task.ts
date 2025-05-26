import type { Project } from '~/types/project'
import type { User } from '~/types/user'

export interface Task {
  id: string;
  assignee: User | null;
  name: string,
  startedAt: string,
  finishedAt: string | null,
  deadline: string | null,
  status: TaskStatus,
  priority: TaskPriority,
  project: Project | null,
}

export enum TaskStatus {
  NOT_STARTED,
  IN_PROGRESS,
  COMPLETED
}

export enum TaskPriority {
  LOW,
  MEDIUM,
  HIGH
}

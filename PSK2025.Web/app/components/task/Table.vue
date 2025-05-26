<template>
  <div class="flex flex-wrap items-center gap-2">
    <USelectMenu
      v-model="taskStatus" placeholder="Select status" value-key="value" :items="taskStatusItems"
      class="w-48" />
    <USelectMenu
      v-model="taskPriority" placeholder="Select priority" value-key="value" :items="taskPriorityItems"
      class="w-48" />
    <USelectMenu
      v-model="taskUser" placeholder="Select assignee" value-key="value" :items="userItems"
      class="w-48" :loading="userStatus !== 'success'" />
  </div>

  <UTable
    ref="table"
    class="shrink-0"
    :data="data?.items"
    :loading="status === 'pending' || status === 'idle'"
    :columns="columns"
    :ui="{
            base: 'table-fixed border-separate border-spacing-0',
            thead: '[&>tr]:bg-elevated/50 [&>tr]:after:content-none',
            tbody: '[&>tr]:last:[&>td]:border-b-0',
            th: 'py-2 first:rounded-l-lg last:rounded-r-lg border-y border-default first:border-l last:border-r',
            td: 'border-b border-default'
          }"
    @select="async (row: TableRow<Task>) => {
      currentTaskId = row.original.id
      viewOpen = true
    }"
  />

  <TaskView :id="currentTaskId" v-model="viewOpen" />

  <div class="flex items-center justify-center gap-3 border-t border-default pt-4 mt-auto w-full">
    <UPagination
      :default-page="1"
      :items-per-page="pageSize"
      :total="data?.totalCount"
      @update:page="(v: number) => page = v"
    />
  </div>
</template>

<script setup lang="ts">
import type { PaginatedList } from '~/types/list'
import type { TableColumn, TableRow } from '@nuxt/ui'
import { type Task, TaskPriority, TaskStatus } from '~/types/task'
import { useProjectStore } from '~/store/project'
import { taskPriorityColor, taskPriorityText, taskStatusColor, taskStatusText } from '~/constants/task'
import type { User } from '~/types/user'

const propertyDate = resolveComponent('PropertyDate')
const columns: TableColumn<Task> [] = [
  {
    id: 'name',
    accessorKey: 'name',
    header: 'Name'
  },
  {
    id: 'status',
    header: 'Status',
    cell: ({ row }) => h(resolveComponent('TaskStatusBadge'), {
      value: row.original.status
    })
  },
  {
    id: 'priority',
    header: 'Priority',
    cell: ({ row }) => h(resolveComponent('TaskPriorityBadge'), {
      value: row.original.priority
    })
  },
  {
    id: 'assignee',
    header: 'Assignee',
    cell: ({ row }) => h('div', undefined, `${row.original.assignee?.firstName ?? ''} ${row.original.assignee?.lastName ?? ''}`)
  },
  {
    id: 'startDate',
    header: 'Start date',
    cell: ({ row }) => h(propertyDate, {
      value: row.original.startedAt
    })
  },
  {
    id: 'endDate',
    header: 'End date',
    cell: ({ row }) => h(propertyDate, {
      value: row.original.finishedAt
    })
  },
  {
    id: 'deadline',
    header: 'Deadline',
    cell: ({ row }) => h(propertyDate, {
      value: row.original.deadline
    })
  }
]

const taskStatusItems = [
  { label: 'All', value: undefined },
  ...Object.keys(TaskStatus)
    .filter((key) => !isNaN(Number(key)))
    .map((key) => ({
      label: taskStatusText(Number(key)),
      value: key,
      chip: {
        color: taskStatusColor(Number(key))
      }
    }))
]
const taskPriorityItems = [
  { label: 'All', value: undefined },
  ...Object.keys(TaskPriority)
    .filter((key) => !isNaN(Number(key)))
    .map((key) => ({
      label: taskPriorityText(Number(key)),
      value: key,
      chip: {
        color: taskPriorityColor(Number(key))
      }
    }))
]

const page = ref(1)
const pageSize = ref(10)
const taskStatus = ref<string | undefined>(undefined)
const taskPriority = ref<string | undefined>(undefined)
const taskUser = ref<string | undefined>(undefined)
const projectSt = useProjectStore()
const { projectId } = storeToRefs(projectSt)
const query = computed(() => {
  const q: Record<string, string | number> = {
    'pageNumber': page.value,
    'pageSize': pageSize.value
  }

  if (projectId.value !== null) {
    q['projectId'] = projectId.value
  }

  if (taskStatus.value !== undefined) {
    q['status'] = taskStatus.value
  }

  if (taskPriority.value !== undefined) {
    q['priority'] = taskPriority.value
  }

  if (taskUser.value !== undefined) {
    q['userId'] = taskUser.value
  }

  return q
})

const { data, status, refresh } = await useApiFetch<PaginatedList<Task>>(
  '/api/tasks',
  {
    query: query
  }
)

const nuxtApp = useNuxtApp()

nuxtApp.hook('task:created', () => {
  refresh()
})

const { data: userData, status: userStatus } = await useApiFetch<User[]>(
  () => `/api/projects/${projectId.value}/users`
)

const userItems = computed(() => {
  return [
    { label: 'All', value: undefined },
    ...userData.value?.map((user) => ({
      label: `${user.firstName} ${user.lastName}`,
      value: user.id
    })) ?? []
  ]
})

const currentTaskId = ref<string | null>(null)
const viewOpen = ref(false)
</script>

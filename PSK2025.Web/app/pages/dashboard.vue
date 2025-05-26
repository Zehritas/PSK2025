<template>
  <UDashboardPanel id="dashboard">
    <template #header>
      <UDashboardNavbar title="Dashboard">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div class="flex flex-col gap-y-8">
        <div v-if="projectsStatus === 'success'" class="">
          <div class="font-semibold text-highlighted truncate mt-2 mb-4">
            Favorite projects
          </div>

          <div class="flex flex-row gap-4">
            <UButton
              v-for="(project) in projectsData.items" :key="project.id"
              class="w-full flex justify-between py-3 px-6"
              variant="subtle"
              :color="projectId === project.id ? 'primary' : 'neutral'"
              trailing-icon="i-lucide-star"
              :disabled="projectId === project.id"
              @click="() => {
                projectId = project.id
              }"
            >
              {{ project.name }}
            </UButton>
          </div>
        </div>

        <div v-if="tasksStatus === 'success'" class="">
          <div class="font-semibold text-highlighted truncate mt-2 mb-4">
            Your tasks
          </div>

          <UPageGrid class="lg:grid-cols-3 gap-4 sm:gap-6 lg:gap-px">
            <UPageCard
              v-for="(stat, index) in tasksStats"
              :key="index"
              :icon="stat.icon"
              :title="stat.title"
              variant="subtle"
              :ui="{
                container: 'gap-y-1.5',
                wrapper: 'items-start',
                leading: 'p-2.5 rounded-full bg-primary/10 ring ring-inset ring-primary/25 flex-col',
                title: 'font-normal text-muted text-xs uppercase'
              }"
              class="lg:rounded-none first:rounded-l-lg last:rounded-r-lg hover:z-1"
            >
              <div class="flex items-center gap-2">
                <span class="text-2xl font-semibold text-highlighted">
                  {{ stat.value }}
                </span>
              </div>
            </UPageCard>
          </UPageGrid>
        </div>

        <div v-if="tasksStatus === 'success'" class="">
          <UTable
            ref="table"
            class="shrink-0"
            :data="yourTasksData?.items"
            :loading="yourTasksStatus === 'pending' || yourTasksStatus === 'idle'"
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
              taskViewOpen = true
            }"
          />

          <div class="flex items-center justify-center gap-3 border-t border-default pt-4 mt-auto w-full">
            <UPagination
              :default-page="1"
              :items-per-page="5"
              :total="yourTasksData?.totalCount"
              @update:page="(v: number) => yourTasksPage = v"
            />
          </div>
        </div>

        <TaskEdit />
        <TaskView :id="currentTaskId" v-model="taskViewOpen" />
      </div>
    </template>
  </UDashboardPanel>
</template>

<script setup lang="ts">
import type { Project } from '~/types/project'
import type { PaginatedList } from '~/types/list'
import { useProjectStore } from '~/store/project'
import { type Task, TaskStatus } from '~/types/task'
import { useUserStore } from '~/store/user'
import type { TableColumn, TableRow } from '@nuxt/ui'

useSeoMeta({
  title: 'Dashboard | CoStudent'
})

const userSt = useUserStore()
const user = await userSt.getCurrentUser()
const projectSt = useProjectStore()
const { projectId } = storeToRefs(projectSt)

const { data: projectsData, status: projectsStatus } = await useApiFetch<PaginatedList<Project>>(
  '/api/projects',
  {
    query: {
      pageSize: 4,
      pageNumber: 1
    }
  }
)

const { data: tasksData, status: tasksStatus } = await useApiFetch<PaginatedList<Task>>(
  '/api/tasks',
  {
    query: {
      pageSize: 1000,
      pageNumber: 1,
      userId: user.id
    }
  }
)

const tasksStats = computed(() => {
  let notStarted = 0
  let inProgress = 0
  let completed = 0

  tasksData.value?.items?.forEach((task: Task) => {
    switch (task.status) {
      case TaskStatus.NOT_STARTED:
        notStarted++
        break
      case TaskStatus.IN_PROGRESS:
        inProgress++
        break
      case TaskStatus.COMPLETED:
        completed++
    }
  })

  return [
    {
      title: 'Not started',
      icon: 'i-lucide-circle-pause',
      value: notStarted
    },
    {
      title: 'In progress',
      icon: 'i-lucide-clock',
      value: inProgress
    },
    {
      title: 'Completed',
      icon: 'i-lucide-circle-check-big',
      value: completed
    }
  ]
})

const propertyDate = resolveComponent('PropertyDate')
const columns: TableColumn<Task> [] = [
  {
    id: 'project',
    accessorKey: 'project.name',
    header: 'Project'
  },
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


const yourTasksPage = ref(1)

const {
  data: yourTasksData,
  status: yourTasksStatus,
  refresh: refreshYourTasks
} = await useApiFetch<PaginatedList<Task>>(
  '/api/tasks',
  {
    query: {
      pageSize: 6,
      pageNumber: yourTasksPage,
      userId: user.id
    }
  }
)

const nuxtApp = useNuxtApp()

nuxtApp.hook('task:updated', () => {
  refreshYourTasks()
})

const currentTaskId = ref<string | null>(null)
const taskViewOpen = ref(false)
</script>

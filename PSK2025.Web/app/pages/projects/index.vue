<template>
  <UDashboardPanel id="projects">
    <template #header>
      <UDashboardNavbar title="Projects">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <div class="flex flex-wrap items-center justify-between gap-1.5">
        <USelectMenu
          v-model="projectStatus" placeholder="Select status" value-key="value" :items="projectStatusItems"
          class="w-48" />
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
            td: 'border-b border-default',
            tr: 'cursor-pointer'
          }"
        @select="async (row: TableRow<Project>) => await navigateTo(`/projects/${row.original.id}`)"
      />

      <div class="flex items-center justify-center gap-3 border-t border-default pt-4 mt-auto w-full">
        <UPagination
          :default-page="1"
          :items-per-page="pageSize"
          :total="data?.totalPages"
          @update:page="(v: number) => page = v"
        />
      </div>
    </template>
  </UDashboardPanel>
</template>

<script setup lang="ts">
import type { PaginatedList } from '~/types/list'
import { type Project, ProjectStatus } from '~/types/project'
import type { TableColumn, TableRow } from '@nuxt/ui'
import { projectStatusColor, projectStatusText } from '~/constants/project'

const propertyDateTime = resolveComponent('PropertyDateTime')

const columns: TableColumn<Project> [] = [
  {
    id: 'name',
    accessorKey: 'name',
    header: 'Name'
  },
  {
    id: 'status',
    header: 'Status',
    cell: ({ row }) => h(resolveComponent('ProjectStatusBadge'), {
      value: row.original.status
    })
  },
  {
    id: 'ownerId',
    accessorKey: 'ownerId',
    header: 'Owner'
  },
  {
    id: 'startDate',
    header: 'Start date',
    cell: ({ row }) => h(propertyDateTime, {
      value: row.original.startDate
    })
  },
  {
    id: 'endDate',
    header: 'End date',
    cell: ({ row }) => h(propertyDateTime, {
      value: row.original.endDate
    })
  }
]

const projectStatusItems = [
  { label: 'All', value: undefined },
  ...Object.keys(ProjectStatus)
    .filter((key) => !isNaN(Number(key)))
    .map((key) => ({
      label: projectStatusText(Number(key)),
      value: key,
      chip: {
        color: projectStatusColor(Number(key))
      }
    }))
]

const page = ref(1)
const pageSize = ref(20)
const projectStatus = ref<string | undefined>(undefined)
const query = computed(() => {
  const q: Record<string, string | number> = {
    'pageNumber': page.value,
    'pageSize': pageSize.value
  }

  if (projectStatus.value !== undefined) {
    q['status'] = projectStatus.value
  }

  return q
})

const { data, status } = await useApiFetch<PaginatedList<Project>>(
  '/api/projects',
  {
    query: query
  }
)
</script>

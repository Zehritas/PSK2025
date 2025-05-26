<template>
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
            td: 'border-b border-default'
          }"
    @select="async (row: TableRow<Project>) => await navigateTo(`/projects/${row.original.id}`)"
  />

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
import { type Project, ProjectStatus } from '~/types/project'
import type { TableColumn, TableRow } from '@nuxt/ui'
import { projectStatusColor, projectStatusText } from '~/constants/project'

const propertyDate = resolveComponent('PropertyDate')

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
    id: 'owner',
    header: 'Owner',
    cell: ({ row }) => h('div', undefined, `${row.original.owner.firstName} ${row.original.owner.lastName}`)
  },
  {
    id: 'startDate',
    header: 'Start date',
    cell: ({ row }) => h(propertyDate, {
      value: row.original.startDate
    })
  },
  {
    id: 'endDate',
    header: 'End date',
    cell: ({ row }) => h(propertyDate, {
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
const pageSize = ref(10)
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

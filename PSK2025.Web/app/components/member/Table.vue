<template>
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
    @select="async (row: TableRow<User>) => {
      currentMember = row.original
      viewOpen = true
    }"
  />

  <MemberView v-model="viewOpen" :member="currentMember" />

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
import type { TableColumn, TableRow } from '@nuxt/ui'
import type { Task } from '~/types/task'
import { useProjectStore } from '~/store/project'
import type { User } from '~/types/user'
import type { PaginatedList } from '~/types/list'

const columns: TableColumn<Task> [] = [
  {
    id: 'firstName',
    accessorKey: 'firstName',
    header: 'First name'
  },
  {
    id: 'lastName',
    accessorKey: 'lastName',
    header: 'Last name'
  },
  {
    id: 'email',
    accessorKey: 'email',
    header: 'Email'
  }
]

const page = ref(1)
const pageSize = ref(11)
const projectSt = useProjectStore()
const { projectId } = storeToRefs(projectSt)

const { data, status, refresh } = await useApiFetch<PaginatedList<User>>(
  () => `/api/projects/${projectId.value}/users`,
  {
    query: {
      pageNumber: page.value,
      pageSize: pageSize.value
    }
  }
)

const nuxtApp = useNuxtApp()

nuxtApp.hook('member:created', () => {
  refresh()
})
nuxtApp.hook('member:updated', () => {
  refresh()
})

const currentMember = ref<User | null>(null)
const viewOpen = ref(false)
</script>

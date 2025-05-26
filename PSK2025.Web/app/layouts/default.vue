<template>
  <UDashboardGroup unit="rem">
    <UDashboardSidebar
      id="default"
      v-model:open="open"
      collapsible
      resizable
      class="bg-elevated/25"
      :ui="{ footer: 'lg:border-t lg:border-default' }"
    >
      <template #default="{ collapsed }">
        <div class="text-xl text-pretty tracking-tight font-bold text-(--ui-text-highlighted) ml-1 mt-3">
          <span v-if="!collapsed">
          <span class="text-primary">Co</span>Student
          </span>
          <span v-else class="text-primary">
            CS
          </span>
        </div>

        <ProjectMenu :collapsed="collapsed" />

        <UDashboardSearchButton :collapsed="collapsed" class="bg-transparent ring-default" />

        <UNavigationMenu
          :collapsed="collapsed"
          :items="links[0]"
          orientation="vertical"
        />

        <UNavigationMenu
          :collapsed="collapsed"
          :items="links[1]"
          orientation="vertical"
          class="mt-auto"
        />
      </template>

      <template #footer="{ collapsed }">
        <UserMenu :collapsed="collapsed" />
      </template>
    </UDashboardSidebar>

    <UDashboardSearch :groups="groups" />

    <slot />
  </UDashboardGroup>
</template>

<script setup lang="ts">
import { useProjectStore } from '~/store/project'

const open = ref(false)

const projectSt = useProjectStore()
await projectSt.refreshProjects()
const { projectId } = storeToRefs(projectSt)

const links = [
  [
    {
      label: 'Dashboard',
      icon: 'i-lucide-house',
      to: '/dashboard',
      onSelect: () => {
        open.value = false
      }
    },
    {
      label: 'Projects',
      icon: 'i-lucide-folder-tree',
      to: '/projects',
      onSelect: () => {
        open.value = false
      }
    },
    {
      label: 'Tasks',
      icon: 'i-lucide-list-todo',
      to: '/tasks',
      onSelect: () => {
        open.value = false
      }
    },
    {
      label: 'Members',
      icon: 'i-lucide-users',
      to: '/members',
      onSelect: () => {
        open.value = false
      }
    }
  ]
]

const groups = computed(() => [
    {
      id: 'links',
      label: 'Go to',
      items: links.flat()
    },
    {
      id: 'projects',
      label: 'Switch project',
      items: Object.keys(projectSt.projects ?? {}).map((key) => ({
        label: projectSt.projects![key]!.name,
        icon: 'i-lucide-arrow-right',
        onSelect: async () => {
          open.value = false
          projectId.value = key
        }
      }))
    }
  ]
)
</script>

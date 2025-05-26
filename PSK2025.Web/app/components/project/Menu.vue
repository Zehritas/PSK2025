<template>
  <UDropdownMenu
    :items="items"
    :content="{ align: 'center', collisionPadding: 12 }"
    :ui="{ content: collapsed ? 'w-40' : 'w-(--reka-dropdown-menu-trigger-width)' }"
  >
    <UButton
      v-bind="{
        label: project?.name ?? '',
        trailingIcon: collapsed ? undefined : 'i-lucide-chevrons-up-down'
      }"
      color="neutral"
      variant="subtle"
      block
      :square="collapsed"
      class="data-[state=open]:bg-elevated"
      :class="[!collapsed && 'py-2']"
      :ui="{
        trailingIcon: 'text-dimmed'
      }"
    />
  </UDropdownMenu>
</template>

<script setup lang="ts">
import { useProjectStore } from '~/store/project'

defineProps<{
  collapsed?: boolean
}>()


const projectSt = useProjectStore()
await projectSt.refreshProjects()
const { projectId, project, projects } = storeToRefs(projectSt)
const nuxtApp = useNuxtApp()

const items = computed(() => {
  return [
    Object.keys(projects.value ?? []).map((key) => {
      const project = projects.value![key]

      return {
        label: project!.name,
        onSelect() {
          projectId.value = project!.id
        }
      }
    }),
    [{
      label: 'Create project',
      icon: 'i-lucide-circle-plus',
      onSelect: async () => {
        await navigateTo('/projects')
        await nuxtApp.callHook('project:create')
      }
    }, {
      label: 'Manage projects',
      icon: 'i-lucide-cog',
      onSelect: async () => {
        await navigateTo('/projects')
      }
    }]]
})
</script>

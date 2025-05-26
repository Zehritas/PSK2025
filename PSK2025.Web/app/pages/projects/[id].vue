<template>
  <UDashboardPanel id="project">
    <template #header>
      <UDashboardNavbar :title="`Project ${project?.name}`">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <UButton label="View tasks" icon="i-lucide-list-todo" variant="outline" @click="viewTasks" />
          <ProjectEditModal v-model="project" />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <UCard v-if="project">
        <PropertyGroup>
          <PropertyItem label="Name">
            {{ project.name }}
          </PropertyItem>

          <PropertyItem label="Status">
            <StatusBadge :value="project.status" />
          </PropertyItem>

          <PropertyItem v-if="project.description" label="Description">
            <span class="whitespace-break-spaces">
            {{ project.description }}
            </span>
          </PropertyItem>

          <PropertyItem label="Owner">
            {{ data.project.owner.firstName }}
            {{ data.project.owner.lastName }}
          </PropertyItem>

          <PropertyItem v-if="project.startDate" label="Start date">
            <PropertyDate :value="project.startDate" />
          </PropertyItem>

          <PropertyItem v-if="project.endDate" label="End date">
            <PropertyDate :value="project.endDate" />
          </PropertyItem>
        </PropertyGroup>
      </UCard>

      <div>
        <div class="font-semibold text-highlighted truncate mt-2 mb-4">
          Members
        </div>

        <div class="flex flex-col gap-4 sm:gap-6 flex-1 overflow-y-auto mt-2">
          <!--<TaskTable />-->
        </div>
      </div>
    </template>
  </UDashboardPanel>
</template>

<script setup lang="ts">
import type { NestedProject, Project } from '~/types/project'
import StatusBadge from '~/components/project/StatusBadge.vue'
import { useProjectStore } from '~/store/project'

useSeoMeta({
  title: 'Projects | CoStudent'
})

const route = useRoute()
const id = route.params.id as string
const project = ref<Project | undefined>(undefined)

const { data } = await useApiFetch<NestedProject>(
  () => `/api/projects/${id}`
)

watch(data, () => {
  project.value = data.value?.project
}, { immediate: true })

const projectSt = useProjectStore()
const { projectId } = storeToRefs(projectSt)


const viewTasks = async () => {
  projectId.value = project.value?.id ?? null
  await navigateTo('/tasks')
}
</script>

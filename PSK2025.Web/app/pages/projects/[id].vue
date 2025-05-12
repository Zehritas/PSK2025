<template>
  <UDashboardPanel id="project">
    <template #header>
      <UDashboardNavbar :title="`Project ${project?.name}`">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
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
    </template>
  </UDashboardPanel>
</template>

<script setup lang="ts">
import type { NestedProject, Project } from '~/types/project'
import StatusBadge from '~/components/project/StatusBadge.vue'

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
</script>

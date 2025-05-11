<template>
  <UDashboardPanel id="project">
    <template #header>
      <UDashboardNavbar :title="`Project ${data?.project.name}`">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <UCard v-if="data">
        <PropertyGroup>
          <PropertyItem label="Name">
            {{ data.project.name }}
          </PropertyItem>

          <PropertyItem label="Status">
            <StatusBadge :value="data.project.status" />
          </PropertyItem>

          <PropertyItem label="Owner">
            {{ data.project.ownerId }}
          </PropertyItem>

          <PropertyItem v-if="data.project.startDate" label="Start date">
            <PropertyDateTime :value="data.project.startDate" />
          </PropertyItem>

          <PropertyItem v-if="data.project.endDate" label="End date">
            <PropertyDateTime :value="data.project.endDate" />
          </PropertyItem>
        </PropertyGroup>
      </UCard>
    </template>
  </UDashboardPanel>
</template>

<script setup lang="ts">
import type { NestedProject } from '~/types/project'
import StatusBadge from '~/components/project/StatusBadge.vue'

useSeoMeta({
  title: 'Projects | CoStudent'
})

const route = useRoute()
const id = route.params.id as string

const { data } = await useApiFetch<NestedProject>(
  () => `/api/projects/${id}`
)
</script>

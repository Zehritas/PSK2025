<template>
  <USlideover
    :open="modelValue" :title="`Project ${data?.project.name ?? ''}`" :ui="{content: 'max-w-3xl'}"
    @update:open="v => emit('update:modelValue', v)">
    <template #body>
      <div v-if="data">
        <PropertyGroup>
          <PropertyItem label="Name">
            {{ data.project.name }}
          </PropertyItem>

          <PropertyItem label="Owner">
            {{ data.project.owner?.firstName }}
            {{ data.project.owner?.lastName }}
          </PropertyItem>

          <PropertyItem label="Status">
            <ProjectStatusBadge :value="data.project.status" />
          </PropertyItem>
        </PropertyGroup>

        <PropertyGroup class="mt-8">
          <PropertyItem label="Start date">
            <PropertyDate :value="data.project.startDate" />
          </PropertyItem>

          <PropertyItem label="End date">
            <PropertyDate :value="data.project.endDate" />
          </PropertyItem>
        </PropertyGroup>
      </div>
    </template>

    <template #footer>
      <div class="flex justify-end w-full gap-3">
        <UButton label="View tasks" icon="i-lucide-list-todo" variant="outline" @click="viewTasks" />
        <UButton label="Edit project" icon="i-lucide-pencil" @click="editProject" />
      </div>
    </template>
  </USlideover>
</template>

<script setup lang="ts">
import type { NestedProject } from '~/types/project'
import { useProjectStore } from '~/store/project'

const props = defineProps({
  id: {
    type: String,
    default: null
  },
  modelValue: {
    type: Boolean,
    default: false
  }
})
const emit = defineEmits<{ 'update:modelValue': [value: boolean] }>()

const { data, refresh } = await useApiFetch<NestedProject>(
  () => `/api/projects/${props.id}`,
  {
    immediate: false,
    watch: false
  }
)

watch(() => props.modelValue, () => {
  if (props.modelValue) {
    refresh()
  }
})

const nuxtApp = useNuxtApp()

const editProject = async () => {
  emit('update:modelValue', false)
  await nuxtApp.callHook('project:update', data.value.project)
}

const projectSt = useProjectStore()
const { projectId } = storeToRefs(projectSt)

const viewTasks = async () => {
  projectId.value = data.value?.project.id ?? null
  await navigateTo('/tasks')
}
</script>

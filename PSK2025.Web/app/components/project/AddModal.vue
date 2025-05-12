<script setup lang="ts">
import * as z from 'zod'
import type { FormSubmitEvent } from '@nuxt/ui'
import { type CreateProjectRequest, type Project, ProjectStatus } from '~/types/project'
import { projectStatusColor, projectStatusText } from '~/constants/project'
import { useUserStore } from '~/store/user'

const projectStatusItems = [
  ...Object.keys(ProjectStatus)
    .filter((key) => Number(key) === ProjectStatus.PLANNED || Number(key) === ProjectStatus.ACTIVE)
    .map((key) => ({
      label: projectStatusText(Number(key)),
      value: key,
      chip: {
        color: projectStatusColor(Number(key))
      }
    }))
]

const schema = z.object({
  name: z.string().min(1, 'This field is required.'),
  status: z.nativeEnum(ProjectStatus),
  startDate: z.string().date().optional().nullable(),
  endDate: z.string().date().optional().nullable().refine((val) => {
    if (val && state.startDate) {
      return new Date(val) > new Date(state.startDate)
    }

    return true
  }, {
    message: 'End date must be after start date.'
  }),
  description: z.string()
})
const open = ref(false)

const state = reactive({
  name: '',
  status: ProjectStatus.PLANNED,
  startDate: undefined,
  endDate: undefined,
  description: ''
})

const toast = useToast()
const loading = ref(false)

async function onSubmit() {
  loading.value = true

  try {
    const project = await useApiDollarFetch<Project>('/api/projects', {
      method: 'POST',
      body: {
        status: state.status,
        name: state.name,
        startDate: state.startDate ? `${state.startDate}T00:00:00.0000Z` : null,
        endDate: state.endDate ? `${state.endDate}T00:00:00.0000Z` : null,
        description: state.description.trim().length > 0 ? state.description : null
      } as CreateProjectRequest
    })

    open.value = false
    resetState()
    toast.add({
      title: 'Project created',
      description: 'A new project has been created successfully.',
      color: 'success'
    })
    await navigateTo(`/projects/${project.id}`)
  } catch (e) {
    console.debug(e)
  } finally {
    loading.value = false
  }
}

const resetState = () => {
  state.name = ''
  state.description = ''
  state.status = ProjectStatus.PLANNED
  state.startDate = undefined
  state.endDate = undefined
}
</script>

<template>
  <UModal v-model:open="open" title="New project" description="Create a new project." :dismissible="!loading">
    <UButton label="New project" icon="i-lucide-plus" />

    <template #body>
      <UForm
        :schema="schema"
        :state="state"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Name" name="name">
          <UInput v-model="state.name" class="w-full" placeholder="Aliquam tincidunt" />
        </UFormField>
        <UFormField label="Status" name="status">
          <USelectMenu
            :model-value="state.status.toString()" placeholder="Select status" value-key="value"
            :items="projectStatusItems"
            class="w-full"
            @update:model-value="(v) => state.status = Number(v)" />
        </UFormField>
        <UFormField label="Description" name="description">
          <UTextarea v-model="state.description" class="w-full" />
        </UFormField>
        <UFormField label="Start date" name="startDate">
          <DatePicker v-model="state.startDate" />
        </UFormField>
        <UFormField label="End date" name="endDate">
          <DatePicker v-model="state.endDate" />
        </UFormField>
        <div class="flex justify-end gap-2">
          <UButton
            label="Cancel"
            color="neutral"
            variant="subtle"
            :disabled="loading"
            @click="open = false"
          />
          <UButton
            label="Create"
            color="primary"
            variant="solid"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>

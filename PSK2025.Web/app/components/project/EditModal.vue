<script setup lang="ts">
import * as z from 'zod'
import { type Project, ProjectStatus, type UpdateProjectRequest } from '~/types/project'
import { projectStatusColor, projectStatusText } from '~/constants/project'

const projectStatusItems = [
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

const props = defineProps({
  modelValue: {
    type: Object as PropType<Project>,
    default: null
  }
})
const emit = defineEmits<{ 'update:modelValue': [value: Project] }>()

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
    const project = await useApiDollarFetch<Project>(`/api/projects/${props.modelValue.id}`, {
      method: 'PUT',
      body: {
        status: state.status,
        name: state.name,
        startDate: state.startDate ? `${state.startDate}T00:00:00.0000Z` : null,
        endDate: state.endDate ? `${state.endDate}T00:00:00.0000Z` : null,
        description: state.description.trim().length > 0 ? state.description : null
      } as UpdateProjectRequest
    })

    emit('update:modelValue', project)
    open.value = false
    toast.add({
      title: 'Project updated',
      description: 'A project has been updated successfully.',
      color: 'success'
    })
  } catch (e) {
    console.debug(e)
  } finally {
    loading.value = false
  }
}

const setState = (project: Project) => {
  state.name = project.name
  state.description = project.description ?? ''
  state.status = project.status
  state.startDate = project.startDate ? project.startDate.split('T')[0] : undefined
  state.endDate = project.endDate ? project.endDate.split('T')[0] : undefined
}

watch(props.modelValue, (project) => {
  if (project) {
    setState(project)
  }
}, {
  immediate: true,
  deep: true
})
watch(open, () => {
  if (props.modelValue && open.value) {
    setState(props.modelValue)
  }
})
</script>

<template>
  <UModal v-model:open="open" title="Edit project" description="Update an existing project." :dismissible="!loading">
    <UButton label="Edit project" icon="i-lucide-pencil" :disabled="!modelValue" />

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
            label="Update"
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

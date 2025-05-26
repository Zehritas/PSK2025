<template>
  <UModal v-model:open="open" title="New task" description="Create a new task." :dismissible="!loading">
    <UButton label="New task" icon="i-lucide-plus" />

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
        <UFormField label="Assignee" name="assignee">
          <USelectMenu
            v-model="state.assignee" placeholder="Select assignee" value-key="value" :items="userItems"
            class="w-full" :loading="userStatus !== 'success'" />
        </UFormField>
        <UFormField label="Status" name="status">
          <USelectMenu
            :model-value="state.status.toString()" placeholder="Select status" value-key="value"
            :items="taskStatusItems"
            class="w-full"
            @update:model-value="(v) => state.status = Number(v)" />
        </UFormField>
        <UFormField label="Priority" name="priority">
          <USelectMenu
            :model-value="state.priority.toString()" placeholder="Select priority" value-key="value"
            :items="taskPriorityItems"
            class="w-full"
            @update:model-value="(v) => state.status = Number(v)" />
        </UFormField>
        <UFormField label="Deadline" name="deadline">
          <DatePicker v-model="state.deadline" />
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

<script setup lang="ts">
import * as z from 'zod'
import { useProjectStore } from '~/store/project'
import { TaskPriority, TaskStatus } from '~/types/task'
import { taskPriorityColor, taskPriorityText, taskStatusColor, taskStatusText } from '~/constants/task'
import type { User } from '~/types/user'

const taskStatusItems = Object.keys(TaskStatus)
  .filter((key) => !isNaN(Number(key)))
  .map((key) => ({
    label: taskStatusText(Number(key)),
    value: key,
    chip: {
      color: taskStatusColor(Number(key))
    }
  }))
const taskPriorityItems = Object.keys(TaskPriority)
  .filter((key) => !isNaN(Number(key)))
  .map((key) => ({
    label: taskPriorityText(Number(key)),
    value: key,
    chip: {
      color: taskPriorityColor(Number(key))
    }
  }))
const nuxtApp = useNuxtApp()

const schema = z.object({
  name: z.string().min(1, 'This field is required.'),
  status: z.nativeEnum(TaskStatus),
  priority: z.nativeEnum(TaskPriority),
  assignee: z.string().optional().nullable(),
  deadline: z.string().date().optional().nullable()
})
const open = ref(false)

const state = reactive({
  name: '',
  status: TaskStatus.NOT_STARTED,
  priority: TaskPriority.MEDIUM,
  assignee: undefined,
  deadline: undefined
})

const toast = useToast()
const projectSt = useProjectStore()
const { projectId } = storeToRefs(projectSt)
const loading = ref(false)

async function onSubmit() {
  loading.value = true

  try {
    let startedAt = undefined
    let finishedAt = undefined

    switch (state.status) {
      case TaskStatus.IN_PROGRESS:
        startedAt = new Date().toISOString()
        break
      case TaskStatus.COMPLETED:
        startedAt = new Date().toISOString()
        finishedAt = new Date().toISOString()
    }

    await useApiDollarFetch('/api/tasks', {
      method: 'POST',
      body: {
        projectId: projectId.value,
        name: state.name,
        status: state.status,
        priority: state.priority,
        userId: state.assignee,
        deadline: state.deadline ? `${state.deadline}T00:00:00.0000Z` : null,
        startedAt: startedAt,
        finishedAt: finishedAt
      }
    })

    open.value = false
    resetState()
    toast.add({
      title: 'Task created',
      description: 'A new task has been created successfully.',
      color: 'success'
    })
    await nuxtApp.callHook('task:created')
  } catch (e) {
    console.debug(e)
  } finally {
    loading.value = false
  }
}

const resetState = () => {
  state.name = ''
  state.status = TaskStatus.NOT_STARTED
  state.priority = TaskPriority.MEDIUM
  state.assignee = undefined
  state.deadline = undefined
}

const { data: userData, status: userStatus } = await useApiFetch<User[]>(
  () => `/api/projects/${projectId.value}/users`
)

const userItems = computed(() => {
  return [
    { label: 'None', value: undefined },
    ...userData.value?.map((user) => ({
      label: `${user.firstName} ${user.lastName}`,
      value: user.id
    })) ?? []
  ]
})
</script>

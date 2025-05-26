<template>
  <UModal v-model:open="open" title="Edit task" description="Update an existing task." :dismissible="!loading">
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

  <UModal
    v-model:open="lockModal"
    title="Optimistic locking conflict"
    :dismissible="false"
  >
    <template #body>
      The data was updated by someone else. You can either cancel to discard your changes or bypass to overwrite the
      latest update.
    </template>
    <template #footer>
      <div class="flex justify-end gap-2 w-full">
        <UButton
          :disabled="lockLoading"
          color="neutral" label="Cancel" @click="() => {
          lockModal = false
          open = false
          nuxtApp.callHook('task:updated')
        }" />
        <UButton label="Bypass" :loading="lockLoading" @click="() => update(true)" />
      </div>
    </template>
  </UModal>
</template>

<script setup lang="ts">
import * as z from 'zod'
import { useProjectStore } from '~/store/project'
import { type Task, TaskPriority, TaskStatus } from '~/types/task'
import { taskPriorityColor, taskPriorityText, taskStatusColor, taskStatusText } from '~/constants/task'
import type { User } from '~/types/user'
import type { PaginatedList } from '~/types/list'

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
  deadline: undefined,
  version: undefined
})

const toast = useToast()
const projectSt = useProjectStore()
const { projectId } = storeToRefs(projectSt)
const loading = ref(false)
const task = ref<Task | null>(null)
const lockModal = ref(false)
const lockLoading = ref(false)

async function onSubmit() {
  await update()
}

async function update(bypassConcurrency: boolean = false) {
  loading.value = true
  lockLoading.value = true

  try {
    let startedAt = undefined
    let finishedAt = undefined

    switch (state.status) {
      case TaskStatus.IN_PROGRESS:
        startedAt = task.value!.startedAt ?? new Date().toISOString()
        break
      case TaskStatus.COMPLETED:
        startedAt = task.value!.startedAt ?? new Date().toISOString()
        finishedAt = task.value!.finishedAt ?? new Date().toISOString()
    }

    await useApiDollarFetch(`/api/tasks/${task.value!.id}`, {
      method: 'PUT',
      body: {
        taskId: task.value!.id,
        name: state.name,
        status: state.status,
        priority: state.priority,
        userId: state.assignee,
        deadline: state.deadline ? `${state.deadline}T00:00:00.0000Z` : null,
        startedAt: startedAt,
        finishedAt: finishedAt,
        version: state.version
      },
      query: {
        bypassConcurrency: bypassConcurrency ? true : undefined
      }
    })

    open.value = false
    toast.add({
      title: 'Task updated',
      description: 'A task has been updated successfully.',
      color: 'success'
    })
    await nuxtApp.callHook('task:updated')
    loading.value = false
    lockLoading.value = false
    lockModal.value = false
  } catch (e) {
    loading.value = false
    lockLoading.value = false
    lockModal.value = true
    open.value = false

    if ((e?.statusCode ?? null) === 409) {
      lockModal.value = true
      lockLoading.value = false
    }
  }
}

const setState = (task: Task) => {
  state.name = task.name
  state.status = task.status ?? TaskStatus.NOT_STARTED
  state.priority = task.priority ?? TaskPriority.MEDIUM
  state.assignee = task.assignee ? task.assignee.userId! : undefined
  state.deadline = task.deadline ? task.deadline.split('T')[0] : undefined
  state.version = task.version
}

const { data: userData, status: userStatus } = await useApiFetch<PaginatedList<User>>(
  () => `/api/projects/${projectId.value}/users`
)

const userItems = computed(() => {
  return [
    { label: 'None', value: undefined },
    ...userData.value?.items.map((user) => ({
      label: `${user.firstName} ${user.lastName}`,
      value: user.id
    })) ?? []
  ]
})

nuxtApp.hook('task:update', (t: Task) => {
  task.value = t
  setState(t)
  open.value = true
})
</script>

<template>
  <USlideover
    :open="modelValue" :title="`Task ${data?.name ?? ''}`" :ui="{content: 'max-w-3xl'}"
    @update:open="v => emit('update:modelValue', v)">
    <template #body>
      <div v-if="data">
        <PropertyGroup>
          <PropertyItem label="Name">
            {{ data.name }}
          </PropertyItem>

          <PropertyItem label="Assignee">
            {{ data.assignee?.firstName }}
            {{ data.assignee?.lastName }}
          </PropertyItem>

          <PropertyItem label="Status">
            <TaskStatusBadge :value="data.status" />
          </PropertyItem>

          <PropertyItem label="Priority">
            <TaskPriorityBadge :value="data.priority" />
          </PropertyItem>
        </PropertyGroup>

        <PropertyGroup class="mt-8">
          <PropertyItem label="Started at">
            <PropertyDate :value="data.startedAt" />
          </PropertyItem>

          <PropertyItem label="Finished at">
            <PropertyDate :value="data.startedAt" />
          </PropertyItem>

          <PropertyItem label="Deadline">
            <PropertyDate :value="data.startedAt" />
          </PropertyItem>
        </PropertyGroup>
      </div>
    </template>

    <template #footer>
      <div class="flex justify-end w-full">
        <UButton
          label="Edit task" icon="i-lucide-pencil" @click="() => {
          emit('update:modelValue', false)
        }" />
      </div>
    </template>
  </USlideover>
</template>

<script setup lang="ts">
import type { Task } from '~/types/task'

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

const { data } = await useApiFetch<Task>(
  () => `/api/tasks/${props.id}`,
  {
    immediate: false
  }
)
</script>

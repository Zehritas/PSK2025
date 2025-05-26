<script setup lang="ts">
import type { PropType } from 'vue'
import { taskPriorityColor, taskPriorityText } from '~/constants/task'
import type { TaskPriority } from '~/types/task'

defineProps({
  value: {
    type: Number as PropType<TaskPriority>,
    default: null
  }
})
</script>

<template>
  <UBadge v-if="value !== null" :color="taskPriorityColor(value)" variant="subtle">
    {{ taskPriorityText(value) }}
  </UBadge>
</template>

<script setup lang="ts">
import type { PropType } from 'vue'
import { taskStatusColor, taskStatusText } from '~/constants/task'
import type { TaskStatus } from '~/types/task'

defineProps({
  value: {
    type: Number as PropType<TaskStatus>,
    default: null
  }
})
</script>

<template>
  <UBadge v-if="value !== null" :color="taskStatusColor(value)" variant="subtle">
    {{ taskStatusText(value) }}
  </UBadge>
</template>

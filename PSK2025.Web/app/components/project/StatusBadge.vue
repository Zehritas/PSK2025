<script setup lang="ts">
import type { PropType } from 'vue'
import { projectStatusColor, projectStatusText } from '~/constants/project'
import type { ProjectStatus } from '~/types/project'

defineProps({
  value: {
    type: Number as PropType<ProjectStatus>,
    default: null
  }
})
</script>

<template>
  <UBadge v-if="value !== null" :color="projectStatusColor(value)" variant="subtle">
    {{ projectStatusText(value) }}
  </UBadge>
</template>

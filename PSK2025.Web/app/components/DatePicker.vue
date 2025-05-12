<template>
  <UPopover>
    <UButton color="neutral" variant="outline" class="w-full text-sm text-highlighted">
      <span v-if="date">{{ date }}</span>
      <span v-else class="text-dimmed">Select a date</span>
    </UButton>

    <template #content>
      <UCalendar v-model="date" class="p-2" />
    </template>
  </UPopover>
</template>

<script setup lang="ts">
import { parseDate } from '@internationalized/date'

const props = defineProps({
  modelValue: {
    type: String,
    default: null
  }
})
const emit = defineEmits<{ 'update:modelValue': [value: string | null] }>()
const date = computed({
  get: () => props.modelValue ? parseDate(props.modelValue) : null,
  set: (value) => emit('update:modelValue', value ? value.toString() : null)
})
</script>

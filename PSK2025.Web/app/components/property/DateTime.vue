<script setup lang="ts">
defineProps({
  value: {
    type: [String, null],
    default: null
  }
})
</script>

<template>
  <span v-if="value">{{ useFormatApiDateTime(value) }}</span>
</template>

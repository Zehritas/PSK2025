<script setup lang="ts">
defineProps({
  label: {
    type: String,
    default: null
  },
  to: {
    type: String,
    default: null
  },
  singleCol: {
    type: Boolean,
    default: false
  }
})
</script>

<template>
  <div class="w-full">
    <slot name="label">
      <div v-if="label && !to" class="mb-3">
        {{ label }}
      </div>
    </slot>
    <NuxtLink
      v-if="label && to"
      :to="to"
      class="mb-3 flex items-center cursor-pointer"
    >
      {{ label }}
      <UIcon name="i-heroicons-link" class="h-3 w-3 ml-2" />
    </NuxtLink>
    <div class="grid gap-4" :class="singleCol ? 'grid-cols-1' : 'grid-cols-1 sm:grid-cols-2'">
      <slot />
    </div>
  </div>
</template>

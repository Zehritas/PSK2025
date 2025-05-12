<script setup lang="ts">
defineProps({
  label: {
    type: String,
    default: null
  },
  to: {
    type: String,
    default: null
  }
})
</script>

<template>
  <div>
    <NuxtLink v-if="to" :to="to">
      <div class="text-sm mb-1 flex items-center text-gray-400">
        {{ label }}
        <UIcon name="i-heroicons-link" class="h-3 w-3 ml-2" />
      </div>
      <div
        class="text-sm min-h-[20px] overflow-x-auto scrollbar-thin scrollbar-track-transparent scrollbar-thumb-gray-600 whitespace-nowrap">
        <slot />
      </div>
    </NuxtLink>

    <div v-else>
      <div class="text-sm mb-1 text-gray-400">
        {{ label }}
      </div>
      <div
        class="text-sm  min-h-[20px] overflow-x-auto scrollbar-thin scrollbar-track-transparent scrollbar-thumb-gray-600 whitespace-nowrap">
        <slot />
      </div>
    </div>
  </div>
</template>

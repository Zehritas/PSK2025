<template>
  <div>
    <ClientOnly v-if="!colorMode?.forced">
      <UButton
        color="neutral" variant="ghost" class="cursor-pointer"
        :icon="isDark ? 'i-heroicons-moon' : 'i-heroicons-sun'" size="lg" @click="isDark = !isDark" />
      <template #fallback>
        <div class="size-9" />
      </template>
    </ClientOnly>
  </div>
</template>
<script setup lang="ts">
const colorMode = useColorMode()

const isDark = computed({
  get() {
    return colorMode.value === 'dark'
  },
  set() {
    colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark'
  }
})
</script>

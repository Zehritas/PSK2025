<template>
  <div>
    <ClientOnly v-if="!colorMode?.forced">
      <UButton
        color="neutral" variant="ghost" class="cursor-pointer"
        :icon="icon" size="lg" @click="set" />
      <template #fallback>
        <div class="size-9" />
      </template>
    </ClientOnly>
  </div>
</template>

<script setup lang="ts">
const colorMode = useColorMode()

const set = () => {
  switch (colorMode.preference) {
    case 'system':
      colorMode.preference = 'light'

      break
    case 'dark':
      colorMode.preference = 'system'

      break
    default:
      colorMode.preference = 'dark'
  }
}

const icon = computed(() => {
  switch (colorMode.preference) {
    case 'system':
      return 'i-lucide-monitor'
    case 'dark':
      return 'i-lucide-moon'
    default:
      return 'i-lucide-sun'
  }
})
</script>

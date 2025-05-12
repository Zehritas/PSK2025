import { createSharedComposable } from '@vueuse/shared'

const _useUiState = () => {
  const commandsOpen = ref(false)
  const timezoneOpen = ref(false)
  const sidebarOpen = ref(false)
  const userInfoOpen = ref(false)

  defineShortcuts({
    'meta_k': () => commandsOpen.value = true
  })

  return {
    commandsOpen,
    timezoneOpen,
    sidebarOpen,
    userInfoOpen
  }
}

export const useUiState = createSharedComposable(_useUiState)

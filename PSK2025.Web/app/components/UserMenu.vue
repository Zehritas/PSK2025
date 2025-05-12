<template>
  <UDropdownMenu
    :items="items"
    :content="{ align: 'center', collisionPadding: 12 }"
    :ui="{ content: collapsed ? 'w-48' : 'w-(--reka-dropdown-menu-trigger-width)' }"
  >
    <UButton
      v-bind="{
        label: collapsed ? undefined : `${user.firstName} ${user.lastName}`,
        trailingIcon: collapsed ? undefined : 'i-lucide-chevrons-up-down'
      }"
      color="neutral"
      variant="ghost"
      block
      :square="collapsed"
      class="data-[state=open]:bg-elevated"
      :ui="{
        trailingIcon: 'text-dimmed'
      }"
    />

    <template #chip-leading="{ item }">
      <span
        :style="{ '--chip': `var(--color-${(item as any).chip}-400)` }"
        class="ms-0.5 size-2 rounded-full bg-(--chip)"
      />
    </template>
  </UDropdownMenu>
</template>

<script setup lang="ts">
import type { DropdownMenuItem } from '@nuxt/ui'
import { useUserStore } from '~/store/user'

defineProps<{
  collapsed?: boolean
}>()

const colorMode = useColorMode()
const userSt = useUserStore()
const user = await userSt.getCurrentUser()
const items = computed<DropdownMenuItem[][]>(() => ([
  [
    {
      type: 'label',
      label: user?.email ?? ''
    }
  ],
  [
    {
      label: 'Appearance',
      icon: 'i-lucide-sun-moon',
      children: [
        {
          label: 'System',
          icon: 'i-lucide-monitor',
          type: 'checkbox',
          checked: colorMode.preference === 'system',
          onSelect(e: Event) {
            e.preventDefault()

            colorMode.preference = 'system'
          }
        },
        {
          label: 'Light',
          icon: 'i-lucide-sun',
          type: 'checkbox',
          checked: colorMode.preference === 'light',
          onSelect(e: Event) {
            e.preventDefault()

            colorMode.preference = 'light'
          }
        },
        {
          label: 'Dark',
          icon: 'i-lucide-moon',
          type: 'checkbox',
          checked: colorMode.preference === 'dark',
          onSelect(e: Event) {
            e.preventDefault()

            colorMode.preference = 'dark'
          }
        }
      ]
    }
  ],
  [
    {
      label: 'Log out',
      icon: 'i-lucide-log-out',
      onSelect(e: Event) {
        e.preventDefault()

        userSt.logout()
        navigateTo('/')
      }
    }
  ]
]))
</script>

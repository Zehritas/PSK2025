<template>
  <div>
    <h1>
      Dashboard
    </h1>

    <UButton @click="logout">
      Logout
    </UButton>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from '~/store/user'

const userSt = useUserStore()

const logout = async () => {
  userSt.logout()
  await navigateTo('/')
}
</script>

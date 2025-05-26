<template>
  <div class="flex flex-col">
    <div class="m-auto max-w-96 w-full py-6">
      <UForm :schema="schema" :state="state" class="space-y-4" @submit="submit">
        <UCard>
          <template #header>
            <h1 class="text-2xl text-pretty tracking-tight font-medium text-(--ui-text-highlighted)">
              Sign In
            </h1>
          </template>

          <div class="space-y-5">
            <UAlert
              v-if="error"
              title="An error has occurred"
              :description="error"
              color="error"
              variant="subtle"
            />

            <UFormField label="E-mail address" name="email">
              <UInput
                v-model="state.email"
                placeholder="john.doe@example.com"
                class="w-full"
                autocomplete="email"
              />
            </UFormField>

            <UFormField label="Password" name="password">
              <UInput
                v-model="state.password"
                placeholder="Password"
                :type="showPassword ? 'text' : 'password'"
                :ui="{ trailing: 'pe-1' }"
                class="w-full"
                autocomplete="current-password"
              >
                <template #trailing>
                  <UButton
                    color="neutral"
                    variant="link"
                    size="sm"
                    :icon="showPassword ? 'i-lucide-eye-off' : 'i-lucide-eye'"
                    :aria-label="showPassword ? 'Hide password' : 'Show password'"
                    :aria-pressed="showPassword"
                    aria-controls="password"
                    @click="showPassword = !showPassword"
                  />
                </template>
              </UInput>
            </UFormField>
          </div>

          <template #footer>
            <div class="flex items-center gap-3 justify-between">
              <div class="text-sm font-light">
                Don’t have an account?
                <ULink to="/signup" :active="true" :disabled="loading">Sign Up</ULink>
              </div>
              <UButton class="cursor-pointer" type="submit" :loading="loading">Sign In</UButton>
            </div>
          </template>
        </UCard>
      </UForm>
    </div>
  </div>
</template>

<script setup lang="ts">
import { type InferType, object, string } from 'yup'
import type { FormSubmitEvent } from '#ui/types'
import { getErrorMessage } from '~/utils/api'
import { useUserStore } from '~/store/user'
import type { LoginRequest, LoginResponse } from '~/types/auth'
import { useProjectStore } from '~/store/project'

definePageMeta({
  layout: 'guest'
})
useSeoMeta({
  title: 'Sign In | CoStudent'
})

const userSt = useUserStore()
const projectSt = useProjectStore()
const showPassword = ref(false)
const loading = ref(false)
const error = ref<string | null>(null)
const state = reactive({
  email: '',
  password: ''
})
const schema = object({
  email: string().required('Must be filled').email('Must be valid e-mail address'),
  password: string().required('Must be filled')
})

type Schema = InferType<typeof schema>

const submit = async (event: FormSubmitEvent<Schema>) => {
  error.value = null
  loading.value = true

  try {
    const resp = await useApiDollarFetch<LoginResponse>('/api/auth/login', {
      method: 'POST',
      body: {
        email: event.data.email,
        password: event.data.password
      } as LoginRequest,
      showError: false,
      guest: true,
      handleUnauthorized: false
    })

    userSt.setAccessToken(resp.token)
    userSt.setRefreshToken(resp.refreshToken)
    await projectSt.refreshProjects(true)

    await navigateTo('/dashboard')
  } catch (e) {
    console.debug(e)
    error.value = getErrorMessage(e)
  } finally {
    loading.value = false
  }
}
</script>

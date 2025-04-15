<template>
  <div class="min-h-screen flex flex-col">
    <div class="m-auto min-w-96">
      <UForm :schema="schema" :state="state" class="space-y-4" @submit="submit">
        <UCard>
          <template #header>
            <h1 class="text-2xl text-pretty tracking-tight font-medium text-(--ui-text-highlighted)">
              Register
            </h1>
          </template>

          <div class="space-y-5">
            <UFormField label="First name" name="firstName">
              <UInput
                  v-model="state.firstName"
                  placeholder="John"
                  class="w-full"
                  autocomplete="given-name"
              />
            </UFormField>

            <UFormField label="Last name" name="lastName">
              <UInput
                  v-model="state.lastName"
                  placeholder="Doe"
                  class="w-full"
                  autocomplete="family-name"
              />
            </UFormField>

            <UFormField label="E-mail address" name="email">
              <UInput
                  v-model="state.email"
                  placeholder="john.doe@example.com"
                  class="w-full"
                  autocomplete="email"
              />
            </UFormField>

            <div class="space-y-2">
              <UFormField label="Password" name="password">
                <UInput
                    v-model="state.password"
                    placeholder="Password"
                    :color="passwordColor"
                    :type="showPassword ? 'text' : 'password'"
                    :ui="{ trailing: 'pe-1' }"
                    :aria-invalid="passwordScore < 4"
                    aria-describedby="password-strength"
                    class="w-full"
                    autocomplete="new-password"
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

              <UProgress
                  :color="passwordColor"
                  :indicator="passwordText"
                  :model-value="passwordScore"
                  :max="4"
                  size="sm"
              />

              <p id="password-strength" class="text-sm font-medium">
                {{ passwordText }}. Must contain:
              </p>

              <ul class="space-y-1" aria-label="Password requirements">
                <li
                    v-for="(req, index) in passwordStrength"
                    :key="index"
                    class="flex items-center gap-0.5"
                    :class="req.met ? 'text-(--ui-success)' : 'text-(--ui-text-muted)'"
                >
                  <UIcon :name="req.met ? 'i-lucide-circle-check' : 'i-lucide-circle-x'" class="size-4 shrink-0 mr-1"/>

                  <span class="text-xs font-light">
                  {{ req.text }}
                  <span class="sr-only">
                    {{ req.met ? ' - Requirement met' : ' - Requirement not met' }}
                  </span>
                </span>
                </li>
              </ul>
            </div>
          </div>

          <template #footer>
            <div class="flex items-center gap-3 justify-between">
              <div class="text-sm font-light">
                Already registered?
                <ULink to="/login" :active="true" :disabled="loading">Login</ULink>
              </div>
              <UButton type="submit" :loading="loading">Register</UButton>
            </div>
          </template>
        </UCard>
      </UForm>
    </div>
  </div>
</template>

<script setup lang="ts">
import {type InferType, object, string} from "yup";
import type {FormSubmitEvent} from "#ui/types";

definePageMeta({
  layout: 'guest',
})

const showPassword = ref(false)
const loading = ref(false)
const state = reactive({
  firstName: '',
  lastName: '',
  email: '',
  password: ''
})
const schema = object({
  firstName: string().required('Must be filled'),
  lastName: string().required('Must be filled'),
  email: string().required('Must be filled').email('Must be valid e-mail address'),
  password: string()
      .required('Must be filled')
      .min(8, 'Must be at least 8 characters')
      .matches(/\d/, 'Must contain at least 1 number')
      .matches(/[a-z]/, 'Must contain at least 1 lowercase letter')
      .matches(/[A-Z]/, 'Must contain at least 1 uppercase letter')
})

type Schema = InferType<typeof schema>

const passwordStrength = computed(() => checkStrength(state.password))
const passwordScore = computed(() => passwordStrength.value.filter(req => req.met).length)
const passwordColor = computed(() => {
  if (passwordScore.value === 0) return 'primary'
  if (passwordScore.value <= 1) return 'error'
  if (passwordScore.value <= 2) return 'warning'
  if (passwordScore.value === 3) return 'warning'
  return 'success'
})
const passwordText = computed(() => {
  if (passwordScore.value === 0) return 'Enter a password'
  if (passwordScore.value <= 2) return 'Weak password'
  if (passwordScore.value === 3) return 'Medium password'
  return 'Strong password'
})

const checkStrength = (str: string) => {
  const requirements = [
    {regex: /.{8,}/, text: 'At least 8 characters'},
    {regex: /\d/, text: 'At least 1 number'},
    {regex: /[a-z]/, text: 'At least 1 lowercase letter'},
    {regex: /[A-Z]/, text: 'At least 1 uppercase letter'}
  ]

  return requirements.map(req => ({met: req.regex.test(str), text: req.text}))
}
const submit = async (event: FormSubmitEvent<Schema>) => {
  loading.value = true

  await new Promise(resolve => setTimeout(resolve, 2000))
  alert('Success! (not really)')

  loading.value = false

  await navigateTo('/')
}
</script>

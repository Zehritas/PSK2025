<template>
  <UModal v-model:open="open" title="Add member" description="Add a new member." :dismissible="!loading">
    <UButton label="New member" icon="i-lucide-plus" />

    <template #body>
      <UForm
        :schema="schema"
        :state="state"
        class="space-y-4"
        @submit="onSubmit"
      >
        <UFormField label="Email" name="email">
          <UInput v-model="state.email" class="w-full" placeholder="user@example.com" />
        </UFormField>
        <div class="flex justify-end gap-2">
          <UButton
            label="Cancel"
            color="neutral"
            variant="subtle"
            :disabled="loading"
            @click="open = false"
          />
          <UButton
            label="Add"
            color="primary"
            variant="solid"
            type="submit"
            :loading="loading"
          />
        </div>
      </UForm>
    </template>
  </UModal>
</template>

<script setup lang="ts">
import * as z from 'zod'
import { useProjectStore } from '~/store/project'

const nuxtApp = useNuxtApp()

const schema = z.object({
  email: z.string().min(1, 'This field is required.').email('Invalid e-mail address format.')
})
const open = ref(false)

const state = reactive({
  email: ''
})

const toast = useToast()
const projectSt = useProjectStore()
const { projectId } = storeToRefs(projectSt)
const loading = ref(false)

async function onSubmit() {
  loading.value = true

  try {
    await useApiDollarFetch('/api/project-members', {
      method: 'POST',
      body: {
        userProject: {
          projectId: projectId.value,
          email: state.email
        }
      }
    })

    open.value = false
    resetState()
    toast.add({
      title: 'Member added',
      description: 'A new member has been created added.',
      color: 'success'
    })
    await nuxtApp.callHook('member:created')
  } catch (e) {
    console.debug(e)
  } finally {
    loading.value = false
  }
}

const resetState = () => {
  state.email = ''
}
</script>

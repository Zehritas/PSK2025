<template>
  <USlideover
    :open="modelValue" :title="`Member ${member?.firstName ?? ''} ${member?.lastName ?? ''}`"
    :ui="{content: 'max-w-3xl'}"
    @update:open="v => emit('update:modelValue', v)">
    <template #body>
      <div v-if="member">
        <PropertyGroup>
          <PropertyItem label="First name">
            {{ member.firstName }}
          </PropertyItem>

          <PropertyItem label="Last name">
            {{ member.lastName }}
          </PropertyItem>

          <PropertyItem label="Email">
            {{ member.email }}
          </PropertyItem>
        </PropertyGroup>
      </div>
    </template>
  </USlideover>
</template>

<script setup lang="ts">
import type { User } from '~/types/user'

defineProps({
  member: {
    type: Object as PropType<User>,
    default: null
  },
  modelValue: {
    type: Boolean,
    default: false
  }
})
const emit = defineEmits<{ 'update:modelValue': [value: boolean] }>()
</script>

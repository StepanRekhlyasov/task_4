<template>
  <auth-form :config="config" />
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import AuthForm, { type FormConfig } from '@/components/widgets/Form.vue'
import { useAuthStore } from '@/stores/auth'
import { toast } from 'vue3-toastify'
import router from '@/router'

const route = useRoute()

const config = computed<FormConfig>(() => ({
  title: 'Reset password',
  description: 'Enter your new password.',
  fields: [
    {
      name: 'email',
      label: 'Email',
      type: 'email',
      disabled: true,
      defaultValue: String(route.query.email ?? ''),
    },
    {
      name: 'newPassword',
      label: 'New password',
      type: 'password',
      required: true,
      autocomplete: 'new-password',
      requiredMessage: 'Password is required.',
    },
    {
      name: 'resetCode',
      hidden: true,
      label: '',
      defaultValue: String(route.query.resetCode ?? ''),
    },
  ],
  submitLabel: 'Reset password',
  links: {
    afterForm: { to: '/login', text: 'Sign in' },
  },
  onSubmit: async (values) => {
    const response = await useAuthStore().resetPassword(
      values.resetCode!,
      values.email!,
      values.newPassword!,
    )
    if (response.status === 200) {
      toast.success('Your password has been reset. You can now sign in.')
      router.push({ name: 'login' })
    }
  },
}))
</script>

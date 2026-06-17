<template>
  <auth-form :config="config" />
</template>

<script setup lang="ts">
import AuthForm, { type FormConfig } from '@/components/widget/Form.vue'
import { useAuthStore } from '@/stores/auth'
import router from '@/router'

const config: FormConfig = {
  title: 'Sign in',
  fields: [
    {
      name: 'email',
      label: 'Login',
      type: 'email',
      required: true,
      trim: true,
      requiredMessage: 'Login is required.',
    },
    {
      name: 'password',
      label: 'Password',
      type: 'password',
      required: true,
      requiredMessage: 'Password is required.',
    },
  ],
  submitLabel: 'Sign in',
  links: {
    beforeSubmit: { to: '/forgot-password', text: 'Forgot password?' },
    afterForm: { to: '/register', text: 'Sign up' },
  },
  onSubmit: async (values) => {
    const response = await useAuthStore().login(values.email!, values.password!)
    if (response.status === 200) {
      router.push('/')
    }
  },
}
</script>
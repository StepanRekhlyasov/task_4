<template>
  <auth-form :config="config" />
</template>

<script setup lang="ts">
import AuthForm, { type FormConfig } from '@/components/widgets/Form.vue'
import { useAuthStore } from '@/stores/auth'
import { toast } from 'vue3-toastify'

const config: FormConfig = {
  title: 'Forgot password?',
  description: 'Your email should be verified to receive a password reset link.',
  fields: [
    {
      name: 'email',
      label: 'Email',
      type: 'email',
      required: true,
      trim: true,
      requiredMessage: 'Email is required.',
      placeholder: 'Enter your email',
    },
  ],
  submitLabel: 'Send reset link',
  links: {
    afterForm: { to: '/login', text: 'Sign in' },
  },
  onSubmit: async (values) => {
    const response = await useAuthStore().forgotPassword(values.email!)
    if (response.status === 200) {
      toast.success('A password reset link has been sent to your email.')
    }
  },
}
</script>

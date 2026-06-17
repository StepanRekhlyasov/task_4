<template>
  <auth-form :config="config" />
</template>

<script setup lang="ts">
import AuthForm, { type FormConfig } from '@/components/widgets/Form.vue'
import { useAuthStore } from '@/stores/auth'
import { toast } from 'vue3-toastify'
import router from '@/router'

const config: FormConfig = {
  title: 'Sign up',
  fields: [
    {
      name: 'name',
      label: 'Name',
      type: 'text',
      required: true,
      trim: true,
      requiredMessage: 'Name is required.',
    },
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
      autocomplete: 'new-password',
      requiredMessage: 'Password is required.',
    },
  ],
  submitLabel: 'Sign up',
  links: {
    afterForm: { to: '/login', text: 'Sign in' },
  },
  onSubmit: async (values) => {
    const response = await useAuthStore().register(values.name!, values.email!, values.password!)
    if (response.status === 200) {
      toast.success('Registration successful. Check your email for verification. You can now login.')
      router.push({ name: 'login' })
    }
  },
}
</script>

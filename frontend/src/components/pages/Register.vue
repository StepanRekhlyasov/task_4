<template>
    <form-layout>
        <div class="container">
            <div class="row justify-content-center">
            <div class="col-12 col-sm-10 col-md-8 col-lg-5 col-xl-4">
                <div class="card auth-card shadow-sm">
                <div class="card-body p-4 p-md-5">
                    <div class="mb-3">
                    <h1 class="h3 mb-2 auth-title">Sign up</h1>
                    </div>
                    <form class="needs-validation" @submit.prevent="onSubmit">
                    <div class="mb-3">
                        <label for="name" class="form-label">Name</label>
                        <input
                        id="name"
                        v-model.trim="form.name"
                        class="form-control"
                        type="text"
                        inputmode="text"
                        autocomplete="name"
                        placeholder="Enter your name"
                        :aria-invalid="!!errors.name"
                        />
                        <div v-if="errors.name" class="invalid-feedback d-block">
                            {{ errors.name }}
                        </div>
                    </div>
                    <div class="mb-3">
                        <label for="email" class="form-label">Login</label>
                        <input
                        id="email"
                        v-model.trim="form.email"
                        class="form-control"
                        type="email"
                        inputmode="text"
                        autocomplete="username"
                        placeholder="Enter your email"
                        :aria-invalid="!!errors.email"
                        />
                        <div v-if="errors.email" class="invalid-feedback d-block">
                            {{ errors.email }}
                        </div>
                    </div>
                    <div class="mb-4">
                        <label for="password" class="form-label">Password</label>
                        <input
                        id="password"
                        v-model="form.password"
                        class="form-control"
                        type="password"
                        autocomplete="new-password"
                        placeholder="Enter your password"
                        :aria-invalid="!!errors.password"
                        />
                        <div v-if="errors.password" class="invalid-feedback d-block">
                            {{ errors.password }}
                        </div>
                    </div>
                    <button class="btn btn-primary w-100" type="submit" :disabled="isSubmitting">
                        <span
                        v-if="isSubmitting"
                        class="spinner-border spinner-border-sm me-2"
                        aria-hidden="true"
                        />
                        Sign up
                    </button>
                    </form>
                    <div class="mt-2 text-center">
                    <router-link class="ms-2 auth-link" to="/login">Sign in</router-link>
                    </div>
                </div>
                </div>
            </div>
            </div>
        </div>
    </form-layout>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { FormLayout } from '@/components'
import { useAuthStore } from '@/stores/auth'
import { toast } from 'vue3-toastify'
import router from '@/router'

const form = reactive({
    name: '',
    email: '',
    password: '',
})

const errors = reactive<{ name?: string; email?: string; password?: string }>({})
const isSubmitting = ref(false)

function validate() {
  errors.name = form.name ? undefined : 'Name is required.'
  errors.email = form.email ? undefined : 'Login is required.'
  errors.password = form.password ? undefined : 'Password is required.'
  return !errors.name && !errors.email && !errors.password
}

async function onSubmit() {
  if (!validate()) return

  isSubmitting.value = true
  try {
    const response = await useAuthStore().register(form.name, form.email, form.password)
    if (response.status === 200) {
      toast.success('Registration successful. Check your email for verification. You can now login.')
      router.push({ name: 'login' })
    }
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style scoped lang="scss">
.auth-page {
  background: $body-bg;
  color: $body-color;
}

.auth-card {
  background: $dark;
  border: 1px solid rgba($secondary, 0.35);
  border-radius: $border-radius;
}

.auth-title {
  color: $body-color;
}

.auth-subtitle {
  opacity: 0.9;
}

.auth-link {
  color: $secondary;
  text-decoration: none;
}

.auth-link:hover,
.auth-link:focus {
  color: $primary;
  text-decoration: underline;
}

</style>

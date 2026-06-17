<template>
    <form-layout>
        <div class="container">
            <div class="row justify-content-center">
            <div class="col-12 col-sm-10 col-md-8 col-lg-5 col-xl-4">
                <div class="card auth-card shadow-sm">
                <div class="card-body p-4 p-md-5">
                    <div class="mb-3">
                    <h1 class="h3 mb-2 auth-title">Sign in</h1>
                    </div>
                    <form class="needs-validation" @submit.prevent="onSubmit">
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
                    <div class="mb-2">
                        <label for="password" class="form-label">Password</label>
                        <input
                        id="password"
                        v-model="form.password"
                        class="form-control"
                        type="password"
                        autocomplete="current-password"
                        placeholder="Enter your password"
                        :aria-invalid="!!errors.password"
                        />
                        <div v-if="errors.password" class="invalid-feedback d-block">
                            {{ errors.password }}
                        </div>
                    </div>
                    <div class="d-flex justify-content-end mb-4">
                        <router-link class="link-secondary auth-link" to="/forgot-password">
                            Forgot password?
                        </router-link>
                    </div>
                    <button class="btn btn-primary w-100" type="submit" :disabled="isSubmitting">
                        <span
                        v-if="isSubmitting"
                        class="spinner-border spinner-border-sm me-2"
                        aria-hidden="true"
                        />
                        Sign in
                    </button>
                    </form>
                    <div class="mt-2 text-center">
                    <router-link class="ms-2 auth-link" to="/register">Sign up</router-link>
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
import router from '@/router'
import { useRoute } from 'vue-router'

const route = useRoute()

const form = reactive({
    email: '',
    password: '',
})

const errors = reactive<{ email?: string; password?: string }>({})
const isSubmitting = ref(false)

function validate() {
  errors.email = form.email ? undefined : 'Login is required.'
  errors.password = form.password ? undefined : 'Password is required.'
  return !errors.email && !errors.password
}

async function onSubmit() {
  if (!validate()) return

  isSubmitting.value = true
  try {
    const response = await useAuthStore().login(form.email, form.password)
    if (response.status === 200) {
      router.push('/')
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
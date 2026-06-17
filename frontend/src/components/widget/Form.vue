<template>
  <form-layout>
    <div class="container">
      <div class="row justify-content-center">
        <div class="col-12 col-sm-10 col-md-8 col-lg-5 col-xl-4">
          <div class="card auth-card shadow-sm">
            <div class="card-body p-4 p-md-5">
              <div class="mb-3">
                <h1 class="h3 mb-2 auth-title">{{ config.title }}</h1>
                <p class="text-muted">{{ config.description }}</p>
              </div>
              <form class="needs-validation" @submit.prevent="handleSubmit">
                <div
                  v-for="(field, index) in config.fields"
                  :key="field.name"
                  :class="fieldMarginClass(index)"
                >
                  <label :for="field.name" class="form-label">{{ field.label }}</label>
                  <input
                    :id="field.name"
                    v-model="form[field.name]"
                    class="form-control"
                    :type="field.type ?? 'text'"
                    :inputmode="field.inputmode ?? 'text'"
                    :autocomplete="fieldAutocomplete(field)"
                    :placeholder="field.placeholder ?? `Enter your ${field.label.toLowerCase()}`"
                    :aria-invalid="!!errors[field.name]"
                  />
                  <div v-if="errors[field.name]" class="invalid-feedback d-block">
                    {{ errors[field.name] }}
                  </div>
                </div>
                <div
                  v-if="config.links?.beforeSubmit"
                  class="d-flex justify-content-end mb-4"
                >
                  <router-link
                    class="link-secondary auth-link"
                    :to="config.links.beforeSubmit.to"
                  >
                    {{ config.links.beforeSubmit.text }}
                  </router-link>
                </div>
                <button class="btn btn-primary w-100" type="submit" :disabled="isSubmitting">
                  <span
                    v-if="isSubmitting"
                    class="spinner-border spinner-border-sm me-2"
                    aria-hidden="true"
                  />
                  {{ config.submitLabel }}
                </button>
              </form>
              <div v-if="config.links?.afterForm" class="mt-2 text-center">
                <router-link class="ms-2 auth-link" :to="config.links.afterForm.to">
                  {{ config.links.afterForm.text }}
                </router-link>
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

export interface FormFieldConfig {
  name: string
  label: string
  type?: 'text' | 'email' | 'password'
  required?: boolean
  trim?: boolean
  placeholder?: string
  autocomplete?: string
  inputmode?: 'text' | 'email' | 'search' | 'tel' | 'url' | 'none' | 'numeric' | 'decimal'
  requiredMessage?: string
}

export interface FormLinkConfig {
  to: string
  text: string
}

export interface FormConfig {
  title: string
  description?: string
  fields: FormFieldConfig[]
  submitLabel: string
  onSubmit: (values: Record<string, string>) => Promise<unknown> | unknown
  links?: {
    beforeSubmit?: FormLinkConfig
    afterForm?: FormLinkConfig
  }
}

const props = defineProps<{
  config: FormConfig
}>()

const form = reactive<Record<string, string>>(
  Object.fromEntries(props.config.fields.map((field) => [field.name, ''])),
)

const errors = reactive<Record<string, string | undefined>>({})
const isSubmitting = ref(false)

function fieldAutocomplete(field: FormFieldConfig): string {
  if (field.autocomplete) return field.autocomplete
  if (field.type === 'email' || field.name === 'email') return 'username'
  if (field.type === 'password') return 'current-password'
  if (field.name === 'name') return 'name'
  return 'off'
}

function fieldMarginClass(index: number): string {
  const isLast = index === props.config.fields.length - 1
  if (!isLast) return 'mb-3'
  return props.config.links?.beforeSubmit ? 'mb-2' : 'mb-4'
}

function getFieldValue(field: FormFieldConfig): string {
  const value = form[field.name] ?? ''
  return field.trim ? value.trim() : value
}

function getFormValues(): Record<string, string> {
  return Object.fromEntries(
    props.config.fields.map((field) => [field.name, getFieldValue(field)]),
  )
}

function validate(): boolean {
  let isValid = true

  for (const field of props.config.fields) {
    if (field.required && !getFieldValue(field)) {
      errors[field.name] = field.requiredMessage ?? `${field.label} is required.`
      isValid = false
    } else {
      errors[field.name] = undefined
    }
  }

  return isValid
}

async function handleSubmit() {
  if (!validate()) return

  isSubmitting.value = true
  try {
    await props.config.onSubmit(getFormValues())
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style scoped lang="scss">
.auth-card {
  background: $dark;
  border: 1px solid rgba($secondary, 0.35);
  border-radius: $border-radius;
}

.auth-title {
  color: $body-color;
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

<template>
  <div class="users-toolbar d-flex gap-2 mb-3">
    <button
      type="button"
      class="btn btn-outline-danger d-inline-flex align-items-center gap-2"
      :disabled="!canBlock || isProcessing"
      @click="blockSelected"
    >
      <svg class="toolbar-icon" viewBox="0 0 16 16" aria-hidden="true">
        <path d="M8 1a2 2 0 0 1 2 2v4H6V3a2 2 0 0 1 2-2zm3 6V3a3 3 0 0 0-6 0v4a2 2 0 0 0-2 2v5a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2z" />
      </svg>
      Block
    </button>
    <button
      type="button"
      class="btn btn-outline-primary d-inline-flex align-items-center gap-2"
      :disabled="!canUnblock || isProcessing"
      @click="unblockSelected"
    >
      <svg class="toolbar-icon" viewBox="0 0 16 16" aria-hidden="true">
        <path d="M11 1a2 2 0 0 0-2 2v4a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V9a2 2 0 0 1 2-2h5V3a3 3 0 1 1 6 0v4a.5.5 0 0 1-1 0V3a2 2 0 0 0-2-2z" />
      </svg>
      Unblock
    </button>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import { storeToRefs } from 'pinia'
import { toast } from 'vue3-toastify'
import { useUsersStore } from '@/stores/users'

const usersStore = useUsersStore()
const { users } = storeToRefs(usersStore)

const selectedIds = defineModel<string[]>('selectedIds', { required: true })
const isProcessing = ref(false)

const selectedUsers = computed(() =>
  users.value.filter((user) => selectedIds.value.includes(user.id)),
)

const canBlock = computed(() =>
  selectedUsers.value.some((user) => user.isActive),
)

const canUnblock = computed(() =>
  selectedUsers.value.some((user) => !user.isActive),
)

async function blockSelected() {
  const usersToBlock = selectedUsers.value.filter((user) => user.isActive)
  if (usersToBlock.length === 0) return

  isProcessing.value = true
  try {
    await Promise.all(usersToBlock.map((user) => usersStore.setUserBlocked(user.id, true)))
    toast.success(usersToBlock.length === 1 ? 'User blocked.' : `${usersToBlock.length} users blocked.`)
    selectedIds.value = []
    await usersStore.fetchUsers()
  } finally {
    isProcessing.value = false
  }
}

async function unblockSelected() {
  const usersToUnblock = selectedUsers.value.filter((user) => !user.isActive)
  if (usersToUnblock.length === 0) return

  isProcessing.value = true
  try {
    await Promise.all(usersToUnblock.map((user) => usersStore.setUserBlocked(user.id, false)))
    toast.success(usersToUnblock.length === 1 ? 'User unblocked.' : `${usersToUnblock.length} users unblocked.`)
    selectedIds.value = []
    await usersStore.fetchUsers()
  } finally {
    isProcessing.value = false
  }
}
</script>

<style scoped lang="scss">
.users-toolbar {
  .btn:disabled {
    opacity: 0.55;
  }
}

.toolbar-icon {
  width: 1rem;
  height: 1rem;
  fill: currentColor;
  flex-shrink: 0;
}
</style>

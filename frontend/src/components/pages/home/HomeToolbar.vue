<template>
  <div class="users-toolbar d-flex align-items-center gap-2 mb-3 justify-content-between">
    <div class="d-flex gap-2 align-items-center">
      <button
        type="button"
        class="btn btn-outline-danger d-inline-flex align-items-center gap-2"
        :disabled="!canBlock || isProcessing"
        @click="blockSelected"
        :title="canBlock ? `Block ${canBlock} active users` : 'No active users found'"
      >
        <font-awesome-icon :icon="faLock" class="toolbar-icon"/>
        Block
      </button>
      <button
        type="button"
        class="btn btn-outline-primary d-inline-flex align-items-center gap-2"
        :disabled="!canUnblock || isProcessing"
        :title="canUnblock ? `Unblock ${canUnblock} blocked users` : 'No blocked users found'"
        @click="unblockSelected"
      >
        <font-awesome-icon :icon="faUnlock" class="toolbar-icon"/>
      </button>
      <button
        type="button"
        class="btn btn-outline-danger d-inline-flex align-items-center gap-2"
        :disabled="!canDelete || isProcessing"
        :title="canDelete ? `Delete ${canDelete} selected users` : 'No users selected'"
        @click="deleteSelected"
      >
        <font-awesome-icon :icon="faTrash" class="toolbar-icon"/>
      </button>
      {{ selectedIds.length }} selected
    </div>

    <div class="d-flex gap-2 align-items-center">
        <button
            type="button"
            class="btn btn-outline-warning d-inline-flex align-items-center gap-2"
            :disabled="!canClean || isProcessing"
            :title="canClean ? `Delete ${canClean} unverified users` : 'No unverified users found'"
            @click="deleteUnconfirmed"
        >
        <font-awesome-icon :icon="faBroom" class="toolbar-icon"/>
        </button>
        <div class="search-field ms-auto">
            <font-awesome-icon :icon="faSearch" class="search-icon"/>
            <input
                v-model.trim="filterString"
                type="search"
                class="form-control search-input"
                placeholder="Search by name or email"
                aria-label="Search users by name or email"
                style="min-width: 240px;"
            />
        </div>
    </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue'
import { storeToRefs } from 'pinia'
import { toast } from 'vue3-toastify'
import { useUsersStore } from '@/stores/users'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faLock, faUnlock, faTrash, faSearch, faBroom } from '@fortawesome/free-solid-svg-icons'

const usersStore = useUsersStore()
const { users } = storeToRefs(usersStore)

const selectedIds = defineModel<string[]>('selectedIds', { required: true })
const filterString = defineModel<string>('filterString', { required: true })
const isProcessing = ref(false)

const selectedUsers = computed(() =>
  users.value.filter((user) => selectedIds.value.includes(user.id)),
)

const canBlock = computed(() =>
  selectedUsers.value.filter((user) => user.isActive)?.length,
)

const canUnblock = computed(() =>
  selectedUsers.value.filter((user) => !user.isActive)?.length,
)

const canClean = computed(() =>
  users.value.filter((user) => !user.emailConfirmed)?.length,
)

const canDelete = computed(() => selectedIds.value.length)

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

async function deleteUnconfirmed() {
  if (!canClean.value) return

  isProcessing.value = true
  try {
    const deletedCount = await usersStore.deleteUnconfirmedUsers()
    toast.success(
      deletedCount === 1
        ? '1 unverified user deleted.'
        : `${deletedCount} unverified users deleted.`,
    )
    selectedIds.value = []
    await usersStore.fetchUsers()
  } finally {
    isProcessing.value = false
  }
}

async function deleteSelected() {
  if (!canDelete.value) return

  const count = selectedUsers.value.length

  isProcessing.value = true
  try {
    await Promise.all(selectedUsers.value.map((user) => usersStore.deleteUser(user.id)))
    toast.success(count === 1 ? 'User deleted.' : `${count} users deleted.`)
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
  .btn, input {
    height: 42px;
  }
}

.toolbar-icon {
  width: 1rem;
  height: 1rem;
  fill: currentColor;
  flex-shrink: 0;
}

.search-field {
  position: relative;
  width: min(100%, 20rem);
}

.search-icon {
  position: absolute;
  left: 0.75rem;
  top: 50%;
  width: 1rem;
  height: 1rem;
  fill: rgba($body-color, 0.65);
  transform: translateY(-50%);
  pointer-events: none;
}

.search-input {
  padding-left: 2.25rem;
  background: $dark;
  border-color: rgba($secondary, 0.35);
  color: $body-color;

  &::placeholder {
    color: rgba($body-color, 0.55);
  }

  &:focus {
    background: $dark;
    border-color: $primary;
    color: $body-color;
    box-shadow: 0 0 0 0.2rem rgba($primary, 0.2);
  }
}
</style>

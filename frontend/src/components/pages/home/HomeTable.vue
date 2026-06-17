<template>
    <div class="table-responsive">
        <table class="table table-dark table-hover users-table align-middle mb-0">
        <thead>
            <tr>
            <th scope="col" class="checkbox-col">
                <input
                ref="selectAllCheckbox"
                class="form-check-input"
                type="checkbox"
                :checked="allSelected"
                aria-label="Select all users"
                @change="toggleSelectAll"
                />
            </th>
            <th scope="col">Name</th>
            <th scope="col">Email</th>
            <th scope="col">Email Status</th>
            <th scope="col">Status</th>
            <th scope="col">Last Activity At</th>
            </tr>
        </thead>
        <tbody>
            <tr v-if="filteredUsers.length === 0">
            <td colspan="6" class="text-center text-muted py-4">
                {{ users.length === 0 ? 'No users found.' : 'No users match your search.' }}
            </td>
            </tr>
            <tr v-for="user in filteredUsers" :key="user.id">
            <td class="checkbox-col">
                <input
                class="form-check-input"
                type="checkbox"
                :checked="selectedIds.includes(user.id)"
                :aria-label="`Select ${user.name}`"
                @change="toggleUser(user.id, ($event.target as HTMLInputElement).checked)"
                />
            </td>
            <td>{{ user.name }}</td>
            <td>{{ user.email }}</td>
            <td>
                <span
                class="badge"
                :class="user.emailConfirmed ? 'text-bg-success' : 'text-bg-secondary'"
                >
                {{ user.emailConfirmed ? 'verified' : 'unverified' }}
                </span>
            </td>
            <td>
                <span
                class="badge"
                :class="user.isActive ? 'text-bg-primary' : 'text-bg-danger'"
                >
                {{ user.isActive ? 'active' : 'blocked' }}
                </span>
            </td>
            <td>{{ formatDate(user.lastActivityAt) }}</td>
            </tr>
        </tbody>
        </table>
    </div>
</template>
<script setup lang="ts">
    import { computed, onMounted, ref, watch } from 'vue'
    import { storeToRefs } from 'pinia'
    import { useUsersStore } from '@/stores/users'
    
    const usersStore = useUsersStore()
    const { users } = storeToRefs(usersStore)
    
    const selectedIds = defineModel<string[]>('selectedIds', { required: true })
    const filterString = defineModel<string>('filterString', { required: true })
    const selectAllCheckbox = ref<HTMLInputElement | null>(null)

    const filteredUsers = computed(() => {
        const query = filterString.value.trim().toLowerCase()
        if (!query) return users.value

        return users.value.filter((user) =>
            user.name.toLowerCase().includes(query) ||
            user.email.toLowerCase().includes(query),
        )
    })
        
    const allSelected = computed(
        () => filteredUsers.value.length > 0 &&
            filteredUsers.value.every((user) => selectedIds.value.includes(user.id)),
    )
    
    const someSelected = computed(() => {
        const selectedCount = filteredUsers.value
            .filter((user) => selectedIds.value.includes(user.id))
            .length
        return selectedCount > 0 && selectedCount < filteredUsers.value.length
    })
    
    watch([someSelected, allSelected], () => {
        if (!selectAllCheckbox.value) return
        selectAllCheckbox.value.indeterminate = someSelected.value
    })
    
    watch(users, () => {
        const validIds = new Set(users.value.map((user) => user.id))
        selectedIds.value = [...selectedIds.value].filter((id) => validIds.has(id))
    })
    
    function toggleSelectAll(event: Event) {
        const checked = (event.target as HTMLInputElement).checked
    
        if (checked) {
        selectedIds.value = [
            ...new Set([
                ...selectedIds.value,
                ...filteredUsers.value.map((user) => user.id),
            ]),
        ]
        return
        }
    
        const filteredIds = new Set(filteredUsers.value.map((user) => user.id))
        selectedIds.value = selectedIds.value.filter((id) => !filteredIds.has(id))
    }
    
    function toggleUser(id: string, checked: boolean) {
        const next = [...selectedIds.value]
        
        if (checked) next.push(id)
        else next.splice(next.indexOf(id), 1)
    
        selectedIds.value = next
    }

    function formatDate(value: string) {
        if (!value) return '—'
    
        return new Date(value).toLocaleString()
    }
    
    onMounted(() => {
        usersStore.fetchUsers()
    })
</script>
  
<style scoped lang="scss">
    .users-table {
        background: $dark;
        border: 1px solid rgba($secondary, 0.35);
        border-radius: $border-radius;
        overflow: hidden;
    
        thead th {
        background: rgba($secondary-bg, 0.6);
        border-bottom: 1px solid rgba($secondary, 0.35);
        color: $body-color;
        font-weight: 600;
        }
    
        tbody td {
        border-color: rgba($secondary, 0.2);
        }
    }
    
    .checkbox-col {
        width: 3rem;
        text-align: center;
    }
    
    .form-check-input {
        cursor: pointer;
    }
</style>
  
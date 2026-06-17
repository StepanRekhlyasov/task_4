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
            <th
                scope="col"
                class="sortable-col"
                :aria-sort="ariaSort('name')"
                @click="setSort('name')"
            >
                <span class="sortable-label">Name<span class="sort-indicator">{{ sortIndicator('name') }}</span></span>
            </th>
            <th
                scope="col"
                class="sortable-col"
                :aria-sort="ariaSort('email')"
                @click="setSort('email')"
            >
                <span class="sortable-label">Email<span class="sort-indicator">{{ sortIndicator('email') }}</span></span>
            </th>
            <th
                scope="col"
                class="sortable-col"
                :aria-sort="ariaSort('emailConfirmed')"
                @click="setSort('emailConfirmed')"
            >
                <span class="sortable-label">Email Status<span class="sort-indicator">{{ sortIndicator('emailConfirmed') }}</span></span>
            </th>
            <th
                scope="col"
                class="sortable-col"
                :aria-sort="ariaSort('isActive')"
                @click="setSort('isActive')"
            >
                <span class="sortable-label">Status<span class="sort-indicator">{{ sortIndicator('isActive') }}</span></span>
            </th>
            <th
                scope="col"
                class="sortable-col"
                :aria-sort="ariaSort('lastActivityAt')"
                @click="setSort('lastActivityAt')"
            >
                <span class="sortable-label">Last Activity At<span class="sort-indicator">{{ sortIndicator('lastActivityAt') }}</span></span>
            </th>
            </tr>
        </thead>
        <tbody>
            <tr v-if="sortedUsers.length === 0">
            <td colspan="6" class="text-center text-muted py-4">
                {{ users.length === 0 ? 'No users found.' : 'No users match your search.' }}
            </td>
            </tr>
            <tr v-for="user in sortedUsers" :key="user.id">
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

    type SortKey = 'name' | 'email' | 'emailConfirmed' | 'isActive' | 'lastActivityAt'
    type SortDirection = 'asc' | 'desc'
    
    const usersStore = useUsersStore()
    const { users } = storeToRefs(usersStore)
    
    const selectedIds = defineModel<string[]>('selectedIds', { required: true })
    const filterString = defineModel<string>('filterString', { required: true })
    const selectAllCheckbox = ref<HTMLInputElement | null>(null)
    const sortKey = ref<SortKey>('lastActivityAt')
    const sortDirection = ref<SortDirection>('desc')

    const filteredUsers = computed(() => {
        const query = filterString.value.trim().toLowerCase()
        if (!query) return users.value

        return users.value.filter((user) =>
            user.name.toLowerCase().includes(query) ||
            user.email.toLowerCase().includes(query),
        )
    })

    const sortedUsers = computed(() => {
        const direction = sortDirection.value === 'asc' ? 1 : -1
        const list = [...filteredUsers.value]

        list.sort((left, right) => {
            let result = 0

            switch (sortKey.value) {
                case 'name':
                    result = left.name.localeCompare(right.name)
                    break
                case 'email':
                    result = left.email.localeCompare(right.email)
                    break
                case 'emailConfirmed':
                    result = Number(left.emailConfirmed) - Number(right.emailConfirmed)
                    break
                case 'isActive':
                    result = Number(left.isActive) - Number(right.isActive)
                    break
                case 'lastActivityAt':
                    result = new Date(left.lastActivityAt).getTime() - new Date(right.lastActivityAt).getTime()
                    break
            }

            return result * direction
        })

        return list
    })
        
    const allSelected = computed(
        () => sortedUsers.value.length > 0 &&
            sortedUsers.value.every((user) => selectedIds.value.includes(user.id)),
    )
    
    const someSelected = computed(() => {
        const selectedCount = sortedUsers.value
            .filter((user) => selectedIds.value.includes(user.id))
            .length
        return selectedCount > 0 && selectedCount < sortedUsers.value.length
    })
    
    watch([someSelected, allSelected], () => {
        if (!selectAllCheckbox.value) return
        selectAllCheckbox.value.indeterminate = someSelected.value
    })
    
    watch(users, () => {
        const validIds = new Set(users.value.map((user) => user.id))
        selectedIds.value = [...selectedIds.value].filter((id) => validIds.has(id))
    })

    function defaultDirectionFor(key: SortKey): SortDirection {
        return key === 'lastActivityAt' ? 'desc' : 'asc'
    }

    function setSort(key: SortKey) {
        if (sortKey.value === key) {
            sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
            return
        }

        sortKey.value = key
        sortDirection.value = defaultDirectionFor(key)
    }

    function sortIndicator(key: SortKey): string {
        if (sortKey.value !== key) return ''
        return sortDirection.value === 'asc' ? '▲' : '▼'
    }

    function ariaSort(key: SortKey): 'ascending' | 'descending' | 'none' {
        if (sortKey.value !== key) return 'none'
        return sortDirection.value === 'asc' ? 'ascending' : 'descending'
    }
    
    function toggleSelectAll(event: Event) {
        const checked = (event.target as HTMLInputElement).checked
    
        if (checked) {
        selectedIds.value = [
            ...new Set([
                ...selectedIds.value,
                ...sortedUsers.value.map((user) => user.id),
            ]),
        ]
        return
        }
    
        const visibleIds = new Set(sortedUsers.value.map((user) => user.id))
        selectedIds.value = selectedIds.value.filter((id) => !visibleIds.has(id))
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

    .sortable-col {
        cursor: pointer;
        user-select: none;
        white-space: nowrap;

        &:hover {
            color: $primary;
        }
    }

    .sortable-label {
        display: inline-flex;
        align-items: center;
        gap: 0.15rem;
    }

    .sort-indicator {
        color: $primary;
        font-size: 0.85em;
        margin-left: 0.2rem;
    }
    
    .checkbox-col {
        width: 3rem;
        text-align: center;
    }
    
    .form-check-input {
        cursor: pointer;
    }
</style>

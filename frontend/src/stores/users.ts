import { ref } from 'vue'
import { defineStore } from 'pinia'
import api from '@/services/api'

export interface UserListItem {
  id: string
  name: string
  email: string
  emailConfirmed: boolean
  isActive: boolean
  createdAt: string
  updatedAt: string
  lastActivityAt: string
}

export const useUsersStore = defineStore('users', () => {
  const users = ref<UserListItem[]>([])

  async function fetchUsers() {
    const response = await api.get<UserListItem[]>('users')
    users.value = response.data
  }

  async function setUserBlocked(id: string, isBlocked: boolean) {
    await api.post(`users/${id}/block`, null, { params: { isBlocked } })
  }

  return { users, fetchUsers, setUserBlocked }
})

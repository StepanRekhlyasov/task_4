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

  async function deleteUnconfirmedUsers() {
    const response = await api.delete<{ deletedCount: number }>('users/unconfirmed')
    return response.data.deletedCount
  }

  async function deleteUser(id: string) {
    await api.delete(`users/${id}`)
  }

  async function resendConfirmationEmail(email: string) {
    await api.post('resendConfirmationEmail', { email })
  }

  return {
    users,
    fetchUsers,
    setUserBlocked,
    deleteUnconfirmedUsers,
    deleteUser,
    resendConfirmationEmail,
  }
})

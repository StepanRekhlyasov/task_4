import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { useUsersStore } from './users'

export const useAuthStore = defineStore('auth', () => {
  const isAuthenticated = ref(false)
//   const currentUser = computed(() => useUsersStore().users.find(user => user.id === user.id))

  return { isAuthenticated }
})

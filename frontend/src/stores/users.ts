import { ref } from 'vue'
import { defineStore } from 'pinia'
import api from '@/services/api'

export const useUsersStore = defineStore('users', () => {
  const users = ref([])

  function fetchUsers() {
    api.get('users').then(response => {
      users.value = response.data
    })
  }

  return { users, fetchUsers }
})

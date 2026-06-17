import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { useUsersStore } from './users'
import router from '@/router'
import api from '@/services/api'

export const useAuthStore = defineStore('auth', () => {

  const login = async (email: string, password: string) => {
    const response = await api.post('login', { email, password })
    if (response.status === 200) {
      localStorage.setItem('access_token', response.data.accessToken)
    }
    return response
  }

  const logout = () => {
    localStorage.removeItem('access_token')
    router.push('/login')
  }

  const register = async (name: string, email: string, password: string) => {
    return await api.post('register', { name, email, password })
  }

  const forgotPassword = async (email: string) => {
    return await api.post('forgotPassword', { email })
  }

  const resetPassword = async (resetCode: string, email: string, newPassword: string) => {
    return await api.post('resetPassword', { resetCode, email, newPassword })
  }

  return { login, logout, register, forgotPassword, resetPassword }
})  

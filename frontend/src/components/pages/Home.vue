<template>
    <h1>You did it!</h1>
    <p>
      <router-link to="/login">Login</router-link>
      <button @click="fetchUsers">Fetch Users</button>
    </p>
</template>
<script setup lang="ts">
import { useUsersStore } from '@/stores/users'

const usersStore = useUsersStore()

function fetchUsers() {
  usersStore.fetchUsers()
}
</script>
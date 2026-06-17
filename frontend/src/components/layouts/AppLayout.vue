<template>
    <div class="header">
        <button class="btn btn-primary" @click="authStore.logout()">Logout</button> 
    </div>
    <div class="container">
        <slot />
    </div>
</template>
<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
</script>
<style lang="scss" scoped>
.header {
    display: flex;
    justify-content: flex-end;
    padding: 1rem;
}
.container {
    padding: 1rem;
}
</style>
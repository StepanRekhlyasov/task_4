<template>
  <app-layout>
    <div class="container-fluid px-4">
      <home-toolbar v-model:selected-ids="selectedIds" v-model:filter-string="filterString"/>
      <home-table v-model:selected-ids="selectedIds" v-model:filter-string="filterString"/>
    </div>
  </app-layout>
</template>
<script setup lang="ts">
import HomeTable from './HomeTable.vue'
import HomeToolbar from './HomeToolbar.vue'
import { AppLayout } from '@/components/layouts'
import { ref } from 'vue'

const selectedIds = ref<string[]>([])
const filterString = ref<string>('')
</script>

<style scoped lang="scss">
.home-title {
  color: $body-color;
}
</style>
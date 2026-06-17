import { createApp } from 'vue'
import { createPinia } from 'pinia'
import Vue3Toastify from 'vue3-toastify';

import './styles/main.scss'
import "vue3-toastify/dist/index.css";

import App from './App.vue'
import router from './router'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)
app.use(Vue3Toastify, {
  autoClose: 3000,
  theme: "colored",
  pauseOnHover: true,
  position: "top-center",
  clearOnUrlChange: false, 
})
app.mount('#app')

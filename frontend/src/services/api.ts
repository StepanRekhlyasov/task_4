import axios from "axios"
import { toast } from "vue3-toastify";

const api = axios.create({
    baseURL: '/api',
    headers: {
        "Content-type": "application/json",
    },
})

api.interceptors.request.use((config) => {
    const token = localStorage.getItem("access_token");
    if (token && !config.headers.Authorization) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
})

api.interceptors.response.use(response => {
    return response
},
error => {
    if (error.response?.status === 401 && !window.location.pathname.includes('login')) {
        window.location.href = '/login'
    }
    toast.error(error.message)
    return Promise.reject(error)
})

export default api
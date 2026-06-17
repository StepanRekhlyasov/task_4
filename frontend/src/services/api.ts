import axios from "axios"
import { toast } from "vue3-toastify";
import router from "@/router"

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
    if (error.response?.status === 401 && router.currentRoute.value.name !== 'login') {
        router.push('/login')
    }
    if (axios.isAxiosError(error)) {
        const data = error.response?.data;
        const detail = data?.detail;
        const errors = data?.errors;
        if(errors) {
            Object.values(errors).forEach((error: any) => {
                toast.error(error)
            })
        } else {
            toast.error(detail)
        }
    }
    return Promise.reject(error)
})

export default api
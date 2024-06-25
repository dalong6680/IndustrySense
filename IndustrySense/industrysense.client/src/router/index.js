import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../components/Dashboard.vue'
import Temp  from '@/components/Temp.vue'

const routes = [
    {
        path: '/',
        name: 'Dashboard',
        component: Dashboard
    },
    {
        path:'/t',
        name: 'Temp',
        component: Temp
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router

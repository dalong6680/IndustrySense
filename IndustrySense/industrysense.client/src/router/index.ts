// router/index.ts
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import Layout from '@/components/Layout.vue';
import Home from '@/components/Home.vue';
import DeviceList from '@/components/DeviceList.vue';

const routes = [
    {
        path: '/',
        component: Layout,
        children: [
            {
                path: '',
                name: 'Home',
                component: Home,
            },
            {
                path: 'device',
                name: 'DeviceList',
                component: DeviceList,
            },
        ],
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;

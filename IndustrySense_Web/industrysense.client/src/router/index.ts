// router/index.ts
import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import Layout from '@/components/Layout.vue';
import Home from '@/components/Home.vue';
import DeviceList from '@/components/DeviceList.vue';
import ParsingRule from '@/components/ParsingRule.vue';
import Record from '@/components/Record.vue'
import Temp from '@/components/Temp.vue'

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
            {
                path: 'rule',
                name: 'ParsingRule',
                component: ParsingRule,
            },
            {
                path: 'record',
                name: 'Record',
                component: Record,
            },
            {
                path: 'temp',
                name: 'Temp',
                component: Temp,
            },
        ],
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;

import { createApp } from 'vue'
import App from '@/App.vue'
import router from './router'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import './styles/global.css';
import { InstallCodemirro } from "codemirror-editor-vue3"
const app = createApp(App)
app.use(router)
app.use(ElementPlus)
app.use(InstallCodemirro)
app.mount('#app')
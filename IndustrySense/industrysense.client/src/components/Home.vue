<template>
    <div class="card-container">
        <el-card class="card-item">
            <template #header>
                <div class="card-header">
                    <span>总设备数</span>
                </div>
            </template>
            <div>{{ deviceCount }}</div>
            <template #footer>
                <el-link href="/device" type="primary">去查看</el-link>
            </template>
        </el-card>
        <el-card class="card-item">
            <template #header>
                <div class="card-header">
                    <span>Lua解析脚本数</span>
                </div>
            </template>
            <div>{{ ruleCount }}</div>
            <template #footer>
                <el-link href="/device" type="primary">去查看</el-link>
            </template>
        </el-card>
    </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue';
import { getDeviceCount } from '@/api/device';
import { getRuleCount } from '@/api/parsingRule';

const deviceCount = ref(0);
const ruleCount = ref(0);

onMounted(async () => {
    deviceCount.value = await getDeviceCount();
    ruleCount.value = await getRuleCount();
});
</script>

<style scoped>
.card-container {
    display: flex;
    justify-content: space-between;
    max-width: 1000px;
    margin: 0 auto;
}

.card-item {
    flex: 1;
    margin: 0 10px;
    max-width: 480px;
}

.card-header {
    font-weight: bold;
}
</style>

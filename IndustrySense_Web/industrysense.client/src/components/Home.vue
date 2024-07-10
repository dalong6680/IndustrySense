<template>
    <el-row :gutter="16">
        <el-col :span="8">
            <div class="statistic-card">
                <el-statistic :value="deviceCount">
                    <template #title>
                        <div style="display: inline-flex; align-items: center">
                            总设备数
                        </div>
                    </template>
                </el-statistic>
            </div>
        </el-col>
        <el-col :span="8">
            <div class="statistic-card">
                <el-statistic :value="onlineDeviceCount">
                    <template #title>
                        <div style="display: inline-flex; align-items: center">
                            在线设备数
                        </div>
                    </template>
                </el-statistic>
            </div>
        </el-col>
        <el-col :span="8">
            <div class="statistic-card">
                <el-statistic :value="ruleCount">
                    <template #title>
                        <div style="display: inline-flex; align-items: center">
                            Lua解析脚本数
                        </div>
                    </template>
                </el-statistic>
            </div>
        </el-col>
    </el-row>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue';
import { getDeviceCount } from '@/api/device';
import { getRuleCount } from '@/api/parsingRule';
import { getOnlineDeviceCount } from '@/api/info';


const deviceCount = ref(0);
const onlineDeviceCount = ref(0);
const ruleCount = ref(0);

onMounted(async () => {
    deviceCount.value = await getDeviceCount();
    ruleCount.value = await getRuleCount();
    onlineDeviceCount.value = await getOnlineDeviceCount();
});
</script>

<style scoped>
:global(h2#card-usage ~ .example .example-showcase) {
    background-color: var(--el-fill-color) !important;
}

.el-statistic {
    --el-statistic-content-font-size: 28px;
}

.statistic-card {
    height: 100%;
    padding: 20px;
    border-radius: 4px;
    background-color: var(--el-bg-color-overlay);
}
</style>
<template>
    <div class="common-layout">
        <el-header class="custom-header">
            <div class="title">工业智能融合感知系统-管理后台</div>
            <div class="toolbar">
                <span>Admin</span>
            </div>
        </el-header>

        <el-container class="layout-container-demo">
            <el-aside width="200px">
                <el-scrollbar>
                    <el-menu router>
                        <el-menu-item index="/">首页</el-menu-item>
                        <el-menu-item index="device">设备列表</el-menu-item>
                    </el-menu>
                </el-scrollbar>
            </el-aside>

            <el-main>
                <router-view></router-view>
            </el-main>
        </el-container>
    </div>
</template>

<script setup lang="ts">
</script>

<style scoped>
.common-layout {
    height: 100vh;
    display: flex;
    flex-direction: column;
}

.custom-header {
    background-color: var(--el-color-primary-light-7);
    color: var(--el-text-color-primary);
    padding: 0 20px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.custom-header .toolbar {
    display: flex;
    align-items: center;
}

.custom-header .title {
    font-size: 18px;
    font-weight: bold;
}

.layout-container-demo {
    flex: 1;
    display: flex;
}

.layout-container-demo .el-aside {
    color: var(--el-text-color-primary);
    background: var(--el-color-primary-light-8);
}

.layout-container-demo .el-menu {
    border-right: none;
}

.layout-container-demo .el-main {
    padding: 20px;
}
</style>
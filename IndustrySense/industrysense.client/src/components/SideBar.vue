<template>
    <el-aside width="200px">
        <el-scrollbar>
            <el-menu>
                <el-menu-item index="/home">
                    <template #title>
                        <el-icon>
                            <house />
                        </el-icon>
                        首页
                    </template>
                </el-menu-item>
                <el-menu-item index="/device">
                    <template #title>
                        <el-icon>
                            <message />
                        </el-icon>
                        设备列表
                    </template>
                </el-menu-item>
            </el-menu>
        </el-scrollbar>
    </el-aside>
</template>
  
<script setup lang="ts">


</script>
  
<style scoped>
.el-aside {
    color: var(--el-text-color-primary);
    background: var(--el-color-primary-light-8);
}

.el-menu {
    border-right: none;
}

.el-scrollbar {
    height: 100%;
}
</style>
  
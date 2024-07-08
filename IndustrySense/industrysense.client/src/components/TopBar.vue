<template>
    <el-header class="custom-header">
        <div class="title">工业智能融合感知系统-管理后台</div>
        <div class="toolbar">
            <el-dropdown>
                <el-icon style="margin-right: 8px; margin-top: 1px">
                    <setting />
                </el-icon>
                <template #dropdown>
                    <el-dropdown-menu>
                        <el-dropdown-item>View</el-dropdown-item>
                        <el-dropdown-item>Add</el-dropdown-item>
                        <el-dropdown-item>Delete</el-dropdown-item>
                    </el-dropdown-menu>
                </template>
            </el-dropdown>
            <span>Admin</span>
        </div>
    </el-header>
</template>
  
<script setup lang="ts">
</script>
  
<style scoped>
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
</style>

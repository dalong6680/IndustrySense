<template>
    <div class="complex-table">
        <div class="container">
            <div class="complex-header">
                <div class="complex-header-action">
                    <el-button type="warning" @click="refresh">
                        <el-icon class="iconfont icon-sync"></el-icon>
                    </el-button>
                    <el-button type="primary">
                        <el-icon class="iconfont icon-plus"></el-icon>
                        <span>添加</span>
                    </el-button>
                    <el-button type="danger" @click="delSelectData">
                        <el-icon class="iconfont icon-delete"></el-icon>
                        <span>删除</span>
                    </el-button>
                    <el-button type="success">
                        <el-icon class="iconfont icon-upload"></el-icon>
                        <span>导出</span>
                    </el-button>
                </div>
                <div class="complex-header-input">
                    <el-input placeholder="请输入标题" style="width: 400px"></el-input>
                </div>
            </div>
            <div class="complex-content">
                <el-table v-loading="loading" class="complex-content-table" :data="tableData" :stripe="true"
                    height="calc(100vh - 250px)" @selection-change="handleSelectionChange">
                    <el-table-column type="selection" width="55" align="center"></el-table-column>
                    <el-table-column prop="deviceId" label="设备ID" width="80" align="center"></el-table-column>
                    <el-table-column prop="deviceName" label="设备名称" align="center"></el-table-column>
                    <el-table-column prop="deviceIpAddress" label="设备IP地址" width="200" align="center"></el-table-column>
                    <el-table-column prop="parsingRuleId" label="解析规则ID" width="100" align="center"></el-table-column>
                    <el-table-column label="操作" width="250" align="center">
                        <template #default="scope">
                            <el-button type="primary">编辑</el-button>
                            <el-button type="danger" @click="delData(scope.row.deviceId)">删除</el-button>
                        </template>
                    </el-table-column>
                </el-table>
                <div class="complex-content-paging">
                    <el-pagination v-model:page-size="page.page_size" v-model:currentPage="page.page_number" background
                        :page-sizes="[20, 40, 60, 80]" layout="total,sizes, prev, pager, next" :total="page.total"
                        @size-change="handleSizeChange" @current-change="handleCurrentChange" />
                </div>
            </div>
        </div>
    </div>
</template>
  
<script setup lang="ts">
import { ref, reactive, onMounted, toRefs } from 'vue'
import { getDevices } from '@/api/device'
import { ElMessage } from 'element-plus';

interface IDevice {
    deviceId: number;
    deviceName: string;
    deviceIpAddress: string;
    parsingRuleId: number;
}

const state = reactive({
    devices: [] as IDevice[],
    tableData: [] as IDevice[],
    loading: false,
    ids: [] as number[],
    page: {
        total: 0,
        page_size: 20,
        page_number: 1
    }
})

const { tableData, loading, page } = toRefs(state)

onMounted(() => {
    getData()
})

const getData = async () => {
    state.loading = true
    const res = await getDevices()
    const { resultList } = res
    state.devices = resultList
    state.page.total = resultList.length
    pagination(resultList)
    state.loading = false
}

const pagination = (datas: IDevice[]) => {
    const { page_number, page_size } = state.page
    state.tableData = datas.filter((item, index) => {
        return index < page_number * page_size && index >= (page_number - 1) * page_size
    })
}

const delData = (id: number) => {
    state.devices = state.devices.filter((item) => {
        return item.deviceId !== id
    })
    state.page.total = state.devices.length
    pagination(state.devices)
}

const delSelectData = () => {
    const { ids } = state
    if (ids.length === 0) {
        ElMessage.warning('请选择设备')
        return false
    }
    ids.forEach((itemId) => {
        delData(itemId)
    })
    ElMessage.success('删除成功')
}

const refresh = () => {
    state.page.page_number = 1
    state.page.page_size = 20
    getData()
}

const handleSelectionChange = (deviceList: IDevice[]) => {
    state.ids = deviceList.map(device => device.deviceId)
}

const handleSizeChange = (size: number) => {
    state.page.page_size = size
    pagination(state.devices)
}

const handleCurrentChange = (page: number) => {
    state.page.page_number = page
    pagination(state.devices)
}
</script>
  
<style scoped>
.complex-table {
    height: 100%;
}

.container .complex-header {
    display: flex;
    justify-content: space-between;
    background: white;
    padding: 20px;
    border-top: 1px solid #eee;
}

.complex-header-action {
    flex: 1;
}

.complex-header-input {
    display: flex;
}

.complex-header-input .el-input {
    margin-right: 15px;
}

.container .complex-content {
    margin: 20px;
    padding: 20px;
    border-radius: 4px;
    background: white;
}

.complex-content-table {
    background: #f5f5f5;
}

.complex-content-paging {
    display: flex;
    align-items: center;
    justify-content: flex-end;
    background: white;
    padding-top: 15px;
}
</style>
  
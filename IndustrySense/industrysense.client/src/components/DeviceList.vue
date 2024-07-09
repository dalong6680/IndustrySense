<template>
    <div class="complex-table">
        <div class="container">
            <div class="complex-header">
                <div class="complex-header-action">
                    <el-button type="warning" @click="refresh">
                        <span>刷新</span>
                    </el-button>
                    <el-button type="primary" @click="dialogFormVisible = true">
                        <span>添加</span>
                    </el-button>
                    <el-button type="danger" @click="deleteSelectedDevices">
                        <span>删除</span>
                    </el-button>
                </div>
                <div class="complex-header-input">
                    <el-input placeholder="请输入标题" style="width: 250px"></el-input>
                    <el-button type="success">
                        <span>搜索</span>
                    </el-button>
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
                            <el-button type="danger" @click="deleteDevice(scope.row.deviceId)">删除</el-button>
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

        <!-- 添加设备的对话框 -->
        <el-dialog title="添加设备" v-model="dialogFormVisible">
            <el-form :model="newDevice">
                <el-form-item label="设备ID">
                    <el-input v-model="newDevice.DeviceId"></el-input>
                </el-form-item>
                <el-form-item label="设备名称">
                    <el-input v-model="newDevice.DeviceName"></el-input>
                </el-form-item>
                <el-form-item label="设备IP地址">
                    <el-input v-model="newDevice.DeviceIpAddress"></el-input>
                </el-form-item>
                <el-form-item label="解析规则ID">
                    <el-input v-model="newDevice.ParsingRuleId"></el-input>
                </el-form-item>
            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogFormVisible = false">取消</el-button>
                <el-button type="primary" @click="addDevice">添加</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, toRefs } from 'vue'
import { getDevices, deleteDevice, addDevice, deleteDeviceById } from '@/api/device'
import { ElMessage } from 'element-plus'

const dialogFormVisible = ref(false)

interface IDevice {
    deviceId: number
    deviceName: string
    deviceIpAddress: string
    parsingRuleId: number
}

const state = reactive({
    devices: [] as IDevice[],
    tableData: [] as IDevice[],
    loading: false,
    ids: [] as number[],
    page: {
        total: 0,
        page_size: 20,
        page_number: 1,
    },
})

const newDevice = reactive({
    DeviceId: 0,
    DeviceName: '',
    DeviceIpAddress: '',
    ParsingRuleId: 0,
})

const { tableData, loading, page } = toRefs(state)

onMounted(() => {
    getData()
})

const getData = async () => {
    state.loading = true
    try {
        const res = await getDevices()
        const { resultList } = res
        state.devices = resultList
        state.page.total = resultList.length
        pagination(resultList)
    } catch (error) {
        ElMessage.error('获取设备列表失败')
    } finally {
        state.loading = false
    }
}

const pagination = (datas: IDevice[]) => {
    const { page_number, page_size } = state.page
    state.tableData = datas.filter((item, index) => {
        return index < page_number * page_size && index >= (page_number - 1) * page_size
    })
}

const deleteDevice = async (id: number) => {
    try {
        await deleteDeviceById(id)
        ElMessage.success('设备删除成功')
        getData()
    } catch (error) {
        ElMessage.error('删除设备失败')
    }
}

const deleteSelectedDevices = async () => {
    if (state.ids.length === 0) {
        ElMessage.warning('请选择设备')
        return
    }
    try {
        await Promise.all(
            state.ids.map((id) => deleteDevice(id))
        )
        ElMessage.success('设备删除成功')
        getData()
    } catch (error) {
        ElMessage.error('删除设备失败')
    }
}

const addDevice = async () => {
    try {
        await addDevice({ ...newDevice })
        ElMessage.success('设备添加成功')
        closeAddDialog()
        getData()
    } catch (error) {
        ElMessage.error('添加设备失败')
    }
}

const handleSelectionChange = (deviceList: IDevice[]) => {
    state.ids = deviceList.map((device) => device.deviceId)
}

const handleSizeChange = (size: number) => {
    state.page.page_size = size
    pagination(state.devices)
}

const handleCurrentChange = (page: number) => {
    state.page.page_number = page
    pagination(state.devices)
}

const refresh = () => {
    state.page.page_number = 1
    state.page.page_size = 20
    getData()
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
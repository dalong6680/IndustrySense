<template>
    <div class="complex-table">
        <div class="container">
            <div class="complex-header">
                <div class="complex-header-action">
                    <el-button type="warning" @click="refresh">
                        <span>刷新</span>
                    </el-button>
                    <el-button type="primary" @click="addDialogVisible = true">
                        <span>添加</span>
                    </el-button>
                    <el-button type="danger" @click="deleteSelectedDevices">
                        <span>删除</span>
                    </el-button>
                </div>
                <div class="complex-header-input">
                    <el-input v-model="searchKeyword" placeholder="请输入设备名称" style="width: 250px"></el-input>
                    <el-button type="success" @click="searchDevices">
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
                    <el-table-column label="操作" align="center">
                        <template #default="scope">
                            <el-button type="success" @click="goToRecords(scope.row.deviceId)">查看记录</el-button>
                            <el-button type="primary"
                                @click="doEditDevice0(scope.row.deviceId, scope.row.deviceName, scope.row.deviceIpAddress, scope.row.parsingRuleId)">编辑</el-button>
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
        <el-dialog title="添加设备" v-model="addDialogVisible" width="500">
            <el-form :model="newDevice">
                <el-form-item label="设备ID" :label-width=120>
                    <el-input v-model="newDevice.DeviceId"></el-input>
                </el-form-item>
                <el-form-item label="设备名称" :label-width=120>
                    <el-input v-model="newDevice.DeviceName"></el-input>
                </el-form-item>
                <el-form-item label="设备IP地址" :label-width=120>
                    <el-input v-model="newDevice.DeviceIpAddress"></el-input>
                </el-form-item>
                <el-form-item label="解析规则ID" :label-width=120>
                    <el-input v-model="newDevice.ParsingRuleId"></el-input>
                </el-form-item>
            </el-form>
            <template #footer>
                <div slot="footer" class="dialog-footer">
                    <el-button @click="addDialogVisible = false">取消</el-button>
                    <el-button type="primary" @click="doAddDevice">添加</el-button>
                </div>
            </template>
        </el-dialog>

        <!-- 编辑设备的对话框 -->
        <el-dialog title="编辑设备" v-model="editDialogVisible" width="500">
            <el-form :model="editDevice">
                <el-form-item label="设备ID" :label-width="120">
                    <el-input v-model="editDevice.DeviceId" disabled></el-input>
                </el-form-item>
                <el-form-item label="设备名称" :label-width="120">
                    <el-input v-model="editDevice.DeviceName"></el-input>
                </el-form-item>
                <el-form-item label="设备IP地址" :label-width="120">
                    <el-input v-model="editDevice.DeviceIpAddress"></el-input>
                </el-form-item>
                <el-form-item label="Lua解析脚本ID" :label-width="120">
                    <el-input v-model="editDevice.ParsingRuleId"></el-input>
                </el-form-item>
            </el-form>
            <template #footer>
                <div slot="footer" class="dialog-footer">
                    <el-button @click="editDialogVisible = false">取消</el-button>
                    <el-button type="primary" @click="doEditDevice">保存</el-button>
                </div>
            </template>
        </el-dialog>

    </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, toRefs } from 'vue'
import { getDevices, addDevice, deleteDeviceById, updateDevice } from '@/api/device'
import { ElMessage } from 'element-plus'
import { useRouter } from 'vue-router'

const router = useRouter()

const goToRecords = (deviceId: number) => {
    router.push({ path: '/record', query: { deviceId } })
}
let addDialogVisible = ref(false)
let editDialogVisible = ref(false)

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
        page_number: 1
    },
    searchKeyword: ''
})

const newDevice = reactive({
    DeviceId: '',
    DeviceName: '',
    DeviceIpAddress: '',
    ParsingRuleId: '',
})

const editDevice = reactive({
    DeviceId: '',
    DeviceName: '',
    DeviceIpAddress: '',
    ParsingRuleId: ''
})

const { tableData, loading, page, searchKeyword } = toRefs(state)

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
            state.ids.map((id) => deleteDeviceById(id))
        )
        ElMessage.success('设备删除成功')
        getData()
    } catch (error) {
        ElMessage.error('删除设备失败')
    }
}

const doAddDevice = async () => {
    try {
        await addDevice(newDevice)
        ElMessage.success('设备添加成功')
        addDialogVisible.value = false
        getData()  // 刷新设备列表
    } catch (error) {
        console.log(error)
        ElMessage.error('添加设备失败')
    }
}
const doEditDevice0 = async (deviceId: number, deviceName: string, deviceIpAddress: string, parsingRuleId: number) => {
    editDevice.DeviceId = deviceId;
    editDevice.DeviceName = deviceName;
    editDevice.DeviceIpAddress = deviceIpAddress;
    editDevice.ParsingRuleId = parsingRuleId;

    editDialogVisible.value = true;
}
const doEditDevice = async () => {
    try {

        await updateDevice(editDevice.DeviceId, editDevice)
        ElMessage.success('设备更新成功')
        editDialogVisible.value = false
        getData()  // 刷新设备列表
    } catch (error) {
        console.log(error)
        ElMessage.error('更新设备失败')
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
    state.searchKeyword = ''
    getData()
}

const searchDevices = () => {
    const keyword = state.searchKeyword.trim().toLowerCase()
    if (!keyword) {
        state.tableData = [...state.devices]
        return
    }
    state.tableData = state.devices.filter(device =>
        device.deviceName.toLowerCase().includes(keyword)
    )
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
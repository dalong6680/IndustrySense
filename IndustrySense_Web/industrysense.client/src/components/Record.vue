<template>
    <div class="complex-table">
        <div class="container">
            <div class="complex-header">
                <div class="complex-header-input">
                    <el-input v-model="searchKeyword" placeholder="请输入记录内容" style="width: 250px"></el-input>
                    <el-button type="success" @click="searchRecords">
                        <span>搜索</span>
                    </el-button>
                    <el-button type="primary" @click="gototemp">
                        <span>查看实时数据</span>
                    </el-button>
                </div>
            </div>
            <div class="complex-content">
                <el-table v-loading="loading" class="complex-content-table" :data="tableData" :stripe="true"
                    height="calc(100vh - 250px)">
                    <el-table-column prop="deviceId" label="设备ID" width="100" align="center"></el-table-column>
                    <el-table-column prop="recordId" label="记录ID" width="100" align="center"></el-table-column>
                    <el-table-column prop="timestamp" label="时间" align="center"></el-table-column>
                    <el-table-column prop="content" label="内容" align="center"></el-table-column>
                    <el-table-column prop="parsedContent" label="解析后内容" align="center"></el-table-column>
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
import { getRecords } from '@/api/record'
import { ElMessage } from 'element-plus'
import { useRouter } from 'vue-router'

interface IRecord {
    recordId: number
    timestamp: string
    content: string
    parsedContent?: string
    deviceId: number
}

const router = useRouter()
const gototemp = () => {
    router.push({ path: '/temp' })


}
const state = reactive({
    records: [] as IRecord[],
    tableData: [] as IRecord[],
    loading: false,
    page: {
        total: 0,
        page_size: 20,
        page_number: 1
    },
    searchKeyword: ''
})

const { tableData, loading, page, searchKeyword } = toRefs(state)

onMounted(() => {
    getData()
})

const getData = async () => {
    state.loading = true
    try {
        const res = await getRecords(-1)
        state.records = res
        state.page.total = res.length
        pagination(res)
    } catch (error) {
        ElMessage.error('获取记录列表失败')
    } finally {
        state.loading = false
    }
}

const pagination = (datas: IRecord[]) => {
    const { page_number, page_size } = state.page
    state.tableData = datas.filter((item, index) => {
        return index < page_number * page_size && index >= (page_number - 1) * page_size
    })
}

const handleSizeChange = (size: number) => {
    state.page.page_size = size
    pagination(state.records)
}

const handleCurrentChange = (page: number) => {
    state.page.page_number = page
    getData()
}

const refresh = () => {
    state.page.page_number = 1
    state.page.page_size = 20
    state.searchKeyword = ''
    getData()
}

const searchRecords = () => {
    const keyword = state.searchKeyword.trim().toLowerCase()
    if (!keyword) {
        state.tableData = [...state.records]
        return
    }
    state.tableData = state.records.filter(record =>
        record.content.toLowerCase().includes(keyword) || (record.parsedContent && record.parsedContent.toLowerCase().includes(keyword))
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

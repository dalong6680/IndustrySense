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
                    <el-button type="danger" @click="deleteSelectedRules">
                        <span>删除</span>
                    </el-button>
                </div>
                <div class="complex-header-input">
                    <el-input v-model="searchKeyword" placeholder="请输入解析规则名称" style="width: 250px"></el-input>
                    <el-button type="success" @click="searchRules">
                        <span>搜索</span>
                    </el-button>
                </div>
            </div>
            <div class="complex-content">
                <el-table v-loading="loading" class="complex-content-table" :data="tableData" :stripe="true"
                    height="calc(100vh - 250px)" @selection-change="handleSelectionChange">
                    <el-table-column type="selection" width="50" align="center"></el-table-column>
                    <el-table-column prop="parsingRuleId" label="解析规则ID" width="100" align="center"></el-table-column>
                    <el-table-column prop="name" label="解析规则名称" align="center"></el-table-column>
                    <el-table-column prop="script" label="脚本" width="200" align="center"></el-table-column>
                    <el-table-column label="操作" width="250" align="center">
                        <template #default="scope">
                            <el-button type="primary"
                                @click="doEditRule(scope.row.parsingRuleId, scope.row.name, scope.row.script)">编辑</el-button>
                            <el-button type="danger" @click="deleteRule(scope.row.parsingRuleId)">删除</el-button>
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

        <!-- 添加规则的对话框 -->
        <el-dialog title="添加解析规则" v-model="addDialogVisible" width="500">
            <el-form :model="newRule">
                <el-form-item label="解析规则名称" :label-width=120>
                    <el-input v-model="newRule.Name"></el-input>
                </el-form-item>
                <el-form-item label="脚本" :label-width=120>
                    <el-input v-model="newRule.Script"></el-input>
                </el-form-item>
            </el-form>
            <template #footer>
                <div slot="footer" class="dialog-footer">
                    <el-button @click="addDialogVisible = false">取消</el-button>
                    <el-button type="primary" @click="doAddRule">添加</el-button>
                </div>
            </template>
        </el-dialog>

        <!-- 编辑规则的对话框 -->
        <el-dialog title="编辑解析规则" v-model="editDialogVisible" width="500">
            <el-form :model="editRule">
                <el-form-item label="解析规则名称" :label-width="120">
                    <el-input v-model="editRule.Name"></el-input>
                </el-form-item>
                <el-form-item label="脚本" :label-width="120">
                    <el-input v-model="editRule.Script"></el-input>
                </el-form-item>
            </el-form>
            <template #footer>
                <div slot="footer" class="dialog-footer">
                    <el-button @click="editDialogVisible = false">取消</el-button>
                    <el-button type="primary" @click="saveEditRule">保存</el-button>
                    <el-button type="info" @click="openScriptEditor">编辑脚本</el-button>
                </div>
            </template>
        </el-dialog>

        <!-- 脚本编辑对话框 -->
        <el-dialog title="编辑脚本" v-model="scriptEditorVisible" width="800" :close-on-click-modal="false">
            <div style="height: 400px; width: 100%">
                <Codemirror v-model:value="currentScript" :options="codeMirrorOptions" @input="onScriptInput">
                </Codemirror>
            </div>
            <template #footer>
                <div slot="footer" class="dialog-footer">
                    <el-button @click="scriptEditorVisible = false">取消</el-button>
                    <el-button type="primary" @click="saveScript">保存</el-button>
                </div>
            </template>
        </el-dialog>
    </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, toRefs } from 'vue'
import { getParsingRules, addParsingRule, deleteParsingRuleById, updateParsingRule } from '@/api/parsingRule'
import { ElMessage } from 'element-plus'
import "codemirror/mode/javascript/javascript.js"
import Codemirror from "codemirror-editor-vue3"
import type { CmComponentRef } from "codemirror-editor-vue3"
import type { Editor, EditorConfiguration } from "codemirror"

const scriptEditorVisible = ref(false);
const currentScript = ref('');
const codeMirrorOptions: EditorConfiguration = {
    // 语言及语法模式
    mode: 'text/x-lua',
    // 主题
    theme: 'neat',
    // 显示函数
    line: true,
    // 显示行号
    lineNumbers: true,
    // 软换行
    lineWrapping: true,
    // tab宽度
    tabSize: 4,
    // 允许拖入的文件类型
    allowDropFileTypes: ['text/x-lua'],
    cursorScrollMargin: 5,
    extraKeys: {},
    // 高亮行功能
    styleActiveLine: true,
    // 调整scrollbar样式功能
    // scrollbarStyle: 'overlay',
    // 自动括号匹配功能
    matchBrackets: true,
    autofocus: true,
    autoRefresh: true,
    // #region 代码折叠
    foldGutter: true,
    // foldOptions: { scanUp: true },
    gutters: [
        'CodeMirror-linenumbers',
        'CodeMirror-foldgutter',
        'CodeMirror-lint-markers'
    ],
    // #endregion
    showHint: true,
    lint: true,
    hintOptions: {
        // 避免由于提示列表只有一个提示信息时，自动填充
        completeSingle: false
    }
};

const onScriptInput = (value: string) => {
    currentScript.value = value;
};

const saveScript = () => {
    console.log('Saving script:', currentScript.value);
    // 保存脚本后，将新的脚本内容填写进脚本输入框
    if (editDialogVisible.value) {
        editRule.Script = currentScript.value;
    } else if (addDialogVisible.value) {
        newRule.Script = currentScript.value;
    }
    scriptEditorVisible.value = false;
};

let addDialogVisible = ref(false)
let editDialogVisible = ref(false)
let scriptToSave = ref('')

interface IParsingRule {
    parsingRuleId: number
    name: string
    script: string
}

const state = reactive({
    rules: [] as IParsingRule[],
    tableData: [] as IParsingRule[],
    loading: false,
    ids: [] as number[],
    page: {
        total: 0,
        page_size: 20,
        page_number: 1
    },
    searchKeyword: ''
})

const newRule = reactive({
    Name: '',
    Script: '',
})

const editRule = reactive({
    ParsingRuleId: '',
    Name: '',
    Script: ''
})

const { tableData, loading, page, searchKeyword } = toRefs(state)

onMounted(() => {
    getData()
})

const getData = async () => {
    state.loading = true
    try {
        const res = await getParsingRules()
        const { resultList } = res
        state.rules = resultList
        state.page.total = resultList.length
        pagination(resultList)
    } catch (error) {
        ElMessage.error('获取规则列表失败')
    } finally {
        state.loading = false
    }
}

const pagination = (datas: IParsingRule[]) => {
    const { page_number, page_size } = state.page
    state.tableData = datas.filter((item, index) => {
        return index < page_number * page_size && index >= (page_number - 1) * page_size
    })
}

const deleteRule = async (id: number) => {
    try {
        await deleteParsingRuleById(id)
        ElMessage.success('规则删除成功')
        getData()
    } catch (error) {
        ElMessage.error('删除规则失败')
    }
}

const deleteSelectedRules = async () => {
    if (state.ids.length === 0) {
        ElMessage.warning('请选择规则')
        return
    }
    try {
        await Promise.all(
            state.ids.map((id) => deleteParsingRuleById(id))
        )
        ElMessage.success('规则删除成功')
        getData()
    } catch (error) {
        ElMessage.error('删除规则失败')
    }
}

const doAddRule = async () => {
    newRule.Script = scriptToSave.value
    try {
        await addParsingRule(newRule)
        ElMessage.success('规则添加成功')
        addDialogVisible.value = false
        getData()  // 刷新规则列表
    } catch (error) {
        console.log(error)
        ElMessage.error('添加规则失败')
    }
}

const doEditRule = async (parsingRuleId: number, name: string, script: string) => {
    editRule.ParsingRuleId = parsingRuleId;
    editRule.Name = name;
    editRule.Script = script;

    // 在编辑规则对话框打开时，将当前脚本填写进 Codemirror 编辑器
    currentScript.value = script;
    editDialogVisible.value = true;
}

const saveEditRule = async () => {
    editRule.Script = scriptToSave.value
    try {
        await updateParsingRule(editRule.ParsingRuleId, editRule)
        ElMessage.success('规则更新成功')
        editDialogVisible.value = false
        getData()  // 刷新规则列表
    } catch (error) {
        console.log(error)
        ElMessage.error('更新规则失败')
    }
}

const handleSelectionChange = (ruleList: IParsingRule[]) => {
    state.ids = ruleList.map((rule) => rule.parsingRuleId)
}

const handleSizeChange = (size: number) => {
    state.page.page_size = size
    pagination(state.rules)
}

const handleCurrentChange = (page: number) => {
    state.page.page_number = page
    pagination(state.rules)
}

const refresh = () => {
    state.page.page_number = 1
    state.page.page_size = 20
    state.searchKeyword = ''
    getData()
}

const searchRules = () => {
    const keyword = state.searchKeyword.trim().toLowerCase()
    if (!keyword) {
        state.tableData = [...state.rules]
        return
    }
    state.tableData = state.rules.filter(rule =>
        rule.name.toLowerCase().includes(keyword)
    )
}

const openScriptEditor = () => {
    scriptEditorVisible.value = true;
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
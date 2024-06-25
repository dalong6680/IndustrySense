<template>
  <div class="common-layout">
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

    <el-container class="layout-container-demo">
      <el-aside width="200px">
        <el-scrollbar>
          <el-menu :default-openeds="['1']">
            <el-sub-menu index="1">
              <template #title>
                <el-icon>
                  <message />
                </el-icon>
                Navigator One
              </template>
              <el-menu-item-group>
                <el-menu-item index="1-1">设备1</el-menu-item>
                <el-menu-item index="1-2">设备2</el-menu-item>
                <el-menu-item index="1-3">设备3</el-menu-item>
                <el-menu-item index="1-4">设备4</el-menu-item>
              </el-menu-item-group>
            </el-sub-menu>
            <el-sub-menu index="2">
              <template #title>
                <el-icon><icon-menu /></el-icon>
                Navigator Two
              </template>
              <el-menu-item-group>
                <el-menu-item index="2-1">Option 1</el-menu-item>
                <el-menu-item index="2-2">Option 2</el-menu-item>
                <el-menu-item index="2-3">Option 3</el-menu-item>
                <el-menu-item index="2-4">Option 4</el-menu-item>
              </el-menu-item-group>
            </el-sub-menu>
            <el-menu-item index="3">
              <template #title>
                <el-icon>
                  <setting />
                </el-icon>
                Navigator Three
              </template>
            </el-menu-item>
          </el-menu>
        </el-scrollbar>
      </el-aside>

      <el-container>


        <el-main>
          <el-header>
            <span>设备数据</span>
            <el-tag type="success" style="float: right;">在线</el-tag>
          </el-header>

          <el-space :wrap="true">
            <el-card v-for="elec in electricData" :key="elec.index" class="box-card" style="width: 250px">
              <template #header>
                <div class="card-header">
                  <span>{{ elec.name }}</span>
                  <el-button class="button" type="text" @click="showChart(elec.name, elec.index)">详情</el-button>
                </div>
              </template>
              <div class="text item">
                {{ elec.value }}
              </div>
            </el-card>
          </el-space>

          <el-dialog v-model:visible="popup" width="50%">
            <span>请选择日期，默认当天：</span>
            <vue3-datepicker v-model="chartDate" :format="format" @closed="showSelectedDateData"></vue3-datepicker>
            <el-divider></el-divider>
            <div id="chart" style="height: 400px;"></div>
            <template v-slot:footer>
              <span class="dialog-footer">
                <el-button @click="closepopup">关闭</el-button>
              </span>
            </template>
          </el-dialog>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axios from 'axios';
import Vue3Datepicker from 'vue3-datepicker';
import { ElMessage } from 'element-plus'

const electricData = ref([]);
const popup = ref(false);
const chartDate = ref(new Date());
const format = 'yyyy-MM-dd';

const fetchElectricData = async () => {
  try {
    const response = await axios.get('https://localhost:7210/api/ElectricData');
    electricData.value = response.data;
  } catch (error) {
    console.error('Failed to fetch electric data:', error);
    ElMessage('Failed to fetch electric data:' + error);
  }
};

const showChart = (name, index) => {
  popup.value = true;
  // 在此处添加显示图表的逻辑
};

const closepopup = () => {
  popup.value = false;
};

const showSelectedDateData = () => {
  // 在此处添加根据选定日期显示数据的逻辑
};

onMounted(() => {
  fetchElectricData();
});
</script>

<style scoped>
.common-layout {
  height: 100vh;
  /* 设置容器高度为视窗高度 */
  display: flex;
  /* 使用 Flexbox 布局 */
  flex-direction: column;
  /* 主轴方向为垂直方向 */
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
  /* 主体部分占据剩余空间 */
  display: flex;
  /* 使用 Flexbox 布局 */
}

.layout-container-demo .el-aside {
  color: var(--el-text-color-primary);
  background: var(--el-color-primary-light-8);
}

.layout-container-demo .el-menu {
  border-right: none;
}

.layout-container-demo .el-main {
  padding: 0;
}

.layout-container-demo .el-scrollbar {
  height: 100%;
  /* 滚动条高度占满容器 */
}
</style>


<!-- <template>
    <el-container style="height: 100vh;">
        <el-aside width="200px" style="background-color: #2d2d2d;">
            <h3 style="color: white; margin: 20px;">设备列表</h3>
            <el-menu :default-active="currentButton" @select="handleSelect" router>
                <el-menu-item v-for="device in buttons" :key="device" :index="device">
                    {{ device }}
                </el-menu-item>
            </el-menu>
        </el-aside>

        <el-container>
            <el-header style="background-color: #2d2d2d; color: white;">
                <span>设备数据</span>
                <el-tag type="success" style="float: right;">在线</el-tag>
            </el-header>

            <el-main>
                <el-row :gutter="20">
                    <el-col :span="12" v-for="elec in electricData" :key="elec.index">
                        <el-card>
                            <p>{{ elec.name }}</p>
                            <p>{{ elec.value }}</p>
                            <el-button type="primary" @click="showChart(elec.name, elec.index)">详情</el-button>
                        </el-card>
                    </el-col>
                </el-row>

                <el-dialog :visible.sync="popup" width="50%">
                    <span>请选择日期，默认当天：</span>
                    <vue3-datepicker v-model="chartDate" :format="format" @closed="showSelectedDateData"></vue3-datepicker>
                    <el-divider></el-divider>
                    <div id="chart" style="height: 400px;"></div>
                    <span slot="footer" class="dialog-footer">
                        <el-button @click="closepopup">关闭</el-button>
                    </span>
                </el-dialog>
            </el-main>
        </el-container>
    </el-container>
</template>

<script>
    import * as echarts from 'echarts';
    import Vue3Datepicker from 'vue3-datepicker';

    export default {
        name: 'dashboard',
        components: {
            Vue3Datepicker
        },
        data() {
            return {
                currentButton: "设备一",
                buttons: ["设备一", "设备二", "设备三"],
                electricData: [
                    { index: 1, name: "电压", value: "220V" },
                    { index: 2, name: "电流", value: "5A" },
                    { index: 3, name: "功率", value: "1100W" }
                ],
                popup: false,
                chartDate: new Date(),
                format: 'yyyy-MM-dd'
            };
        },
        methods: {
            handleSelect(device) {
                this.currentButton = device;
            },
            showChart(name, index) {
                this.popup = true;
                // 在此处添加显示图表的逻辑
            },
            closepopup() {
                this.popup = false;
            },
            showSelectedDateData() {
                // 在此处添加根据选定日期显示数据的逻辑
            }
        }
    };
</script>

<style>
    /* 你可以根据需要调整样式 */
</style> -->

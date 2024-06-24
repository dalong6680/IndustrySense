<template>
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
    name: 'Dashboard',
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
</style>
  
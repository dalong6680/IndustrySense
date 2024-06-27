<template>
  <div>
    <el-header style="background-color: #2d2d2d; color: white;">
      <span>实时温度数据</span>
    </el-header>
    <el-main>
      <div id="chart" style="width: 100%; height: 400px;"></div>
    </el-main>
  </div>
</template>

<script>
import * as echarts from 'echarts';

export default {
  data() {
    return {
      ws: null, // WebSocket 连接
    };
  },
  created() {
    //绑定事件
    window.addEventListener('beforeunload', e => this.closeWebsocket(e))
  },
  mounted() {
    // 初始化图表
    this.initChart();

    // 初始化 WebSocket 连接
    this.ws = new WebSocket('wss://localhost:7210/temp');

    this.ws.onopen = () => {
      console.log("WebSocket 连接已建立");
    };
      this.ws.onclose = (evt) => {
          console.log("WebSocket 已关闭");
      };
    this.ws.onmessage = (event) => {
      const data = Number(event.data);
      this.updateChart(data);
    };

    this.ws.onerror = (error) => {
      console.error("WebSocket 错误: ", error);
    };
  },
  beforeUnmount() {
    //卸载事件
    window.removeEventListener('beforeunload', e => this.closeWebsocket(e))
  },
  methods: {
    // 初始化图表
    initChart() {
      this.chart = echarts.init(document.getElementById('chart'));
      const option = {
        title: {
          text: '实时温度数据',
        },
        tooltip: {
          trigger: 'axis',
        },
        xAxis: {
          type: 'category',
          boundaryGap: false,
          data: [],
        },
        yAxis: {
          type: 'value',
          min: -20,
          max: 40,
        },
        series: [
          {
            name: '温度',
            type: 'line',
            data: [],
          },
        ],
      };
      this.chart.setOption(option);
    },
    // 更新图表数据
    updateChart(data) {
      const option = this.chart.getOption();
      option.xAxis[0].data.push(new Date().toLocaleTimeString());
      option.series[0].data.push(data);

      if (option.xAxis[0].data.length > 20) {
        option.xAxis[0].data.shift();
        option.series[0].data.shift();
      }

      this.chart.setOption(option);
    },
    // 关闭 WebSocket 连接
    closeWebsocket(e) {
      if (this.ws) {
        this.ws.close();
      }
    },
  },



};
</script>

<style scoped>
#chart {
  width: 100%;
  height: 400px;
}
</style>_e_evt

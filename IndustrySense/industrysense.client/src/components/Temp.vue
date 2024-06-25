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
  
  <script setup>
  import * as echarts from 'echarts';
  import { onMounted } from 'vue';
  
  let chart;
  
  const initChart = () => {
    chart = echarts.init(document.getElementById('chart'));
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
    chart.setOption(option);
  };
  
  const updateChart = (data) => {
    const option = chart.getOption();
    option.xAxis[0].data.push(new Date().toLocaleTimeString());
    option.series[0].data.push(data);
  
    if (option.xAxis[0].data.length > 20) {
      option.xAxis[0].data.shift();
      option.series[0].data.shift();
    }
  
    chart.setOption(option);
  };
  
  onMounted(() => {
    initChart();
  
    const ws = new WebSocket('wss://localhost:7210/ws');
    ws.onmessage = (event) => {
      const data = Number(event.data);
      updateChart(data);
    };
  });
  </script>
  
  <style scoped>
  #chart {
    width: 100%;
    height: 400px;
  }
  </style>
  
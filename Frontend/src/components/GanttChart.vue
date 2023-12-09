<template>
  <div class="container-fluid text-center mt-3">
    <h4>Діаграма Ганта для проєкту:</h4>
    <BarChart :chart-data="chartData" :options="chartOptions" :style="style" />
    <button type="button" class="btn btn-lg btn-primary m-2" @click="backToPlanning">
      Назад до планування проєкту
    </button>
  </div>
</template>

<script setup lang="ts">
import { BarChart } from "vue-chart-3";
import ChartDataLabels from "chartjs-plugin-datalabels";
import autocolors from "chartjs-plugin-autocolors";
import { Chart, registerables } from "chart.js";
Chart.register(...registerables, ChartDataLabels, autocolors);

const props = defineProps(["chartData"]);
const emit = defineEmits(["backToPlanning"]);

const chartOptions = {
  indexAxis: "y",
  scales: {
    x: {
      title: {
        text: "Дні",
        display: true,
        font: {
          size: 16,
        },
      },
      stacked: true,
      ticks: {
        stepSize: 1,
        font: {
          size: 16,
        },
      },
    },
    y: {
      title: {
        text: "Задачі",
        display: true,
        font: {
          size: 16,
        },
      },
      stacked: true,
      ticks: {
        font: {
          size: 16,
        },
        callback: function (value: any, index: any) {
          var label = this.getLabelForValue(value);
          return label.length > 30 ? label.slice(0, 29) + "..." : label;
        },
      },
    },
  },
  plugins: {
    legend: {
      display: true,
      labels: {
        filter: function (item: any, context: any) {
          return item.text !== undefined;
        },
      },
    },
    tooltip: {
      enabled: false,
    },
    datalabels: {
      font: {
        size: 16,
      },
      formatter: (value: any, context: any) => {
        return context.dataset.label;
      },
      display: (context: any) => {
        return context.dataset.data[context.dataIndex] !== 0;
      },
      color: (context: any) => {
        return isLightColor(context.dataset.backgroundColor) ? "dimgrey" : "lightgray";
      },
    },
    autocolors: {
      mode: "dataset",
    },
  },
};
const style = {
  height: "500px",
  width: "1500px",
  position: "relative",
};

function backToPlanning() {
  emit("backToPlanning");
}

function isLightColor(color: any) {
  const rgb = color.match(/\d+/g).map(Number);
  const brightness = (rgb[0] * 299 + rgb[1] * 587 + rgb[2] * 114) / 1000;
  return brightness > 128;
}
</script>

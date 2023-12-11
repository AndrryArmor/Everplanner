<template>
  <div class="m-3">
    <h4 class="ms-5">
      Результати планування проєкту "{{ projectStats.name }}" за {{ projectStats.titleTrait }}:
    </h4>
    <template v-if="projectStats.endingTime > 0">
      <p>
        Час виконання проєкту: <b>{{ projectStats.endingTime.toFixed(2) }} днів.</b><br />
        Необхідна кількість працівників:
        <b>{{ projectStats.usedWorkers }}/{{ projectStats.workers }}</b>
      </p>
      <b>Діаграма Ганта:</b>
      <div class="overflow-x-auto">
        <BarChart :chart-data="chartData" :options="chartOptions" :style="style" />
      </div>
    </template>
    <template v-else>
      <p>Проєкт неможливо виконати за {{ projectStats.expectedProjectDuration }} дні(в).</p>
    </template>
    <button type="button" class="btn btn-lg btn-primary m-2" @click="backToPlanning">
      Назад до планування проєкту
    </button>
  </div>
</template>

<script setup>
import { BarChart } from "vue-chart-3";
import ChartDataLabels from "chartjs-plugin-datalabels";
import autocolors from "chartjs-plugin-autocolors";
import { Chart, registerables } from "chart.js";
Chart.register(...registerables, ChartDataLabels, autocolors);

const props = defineProps({
  chartData: {
    type: Object,
    required: true,
  },
  projectStats: {
    type: Object,
    reqiured: true,
  },
});
const emit = defineEmits(["backToPlanning"]);

const chartOptions = {
  responsive: true,
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
        callback: function (value, index) {
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
        filter: function (item, context) {
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
      formatter: (value, context) => {
        return context.dataset.label;
      },
      display: (context) => {
        return context.dataset.data[context.dataIndex] !== 0;
      },
      color: (context) => {
        return isLightColor(context.dataset.backgroundColor) ? "dimgrey" : "lightgray";
      },
    },
    autocolors: {
      mode: "dataset",
    },
  },
};
const style = {
  width: `max(100vw - 2em - 20px, ${500 + 31 * props.projectStats.endingTime}px)`,
  "max-width": `max(100vw - 2em - 20px, ${500 + 31 * props.projectStats.endingTime}px)`,
  height: `max(100px + 25vh, ${100 + 40 * props.projectStats.tasks}px)`,
  "max-height": `max(100px + 25vh, ${100 + 40 * props.projectStats.tasks}px)`,
};

function backToPlanning() {
  emit("backToPlanning");
}

function isLightColor(color) {
  const rgb = color.match(/\d+/g).map(Number);
  const brightness = (rgb[0] * 299 + rgb[1] * 587 + rgb[2] * 114) / 1000;
  return brightness > 128;
}
</script>

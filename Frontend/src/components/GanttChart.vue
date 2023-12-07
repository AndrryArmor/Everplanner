<template>
  <div class="container-fluid text-center">
    <h4>Діаграма Ганта для проєкту:</h4>
    <Bar id="my-chart-id" :options="chartOptions" :data="chartData" :style="style" />
    <button type="button" class="btn btn-lg btn-primary m-2" @click="backToPlanning">
      Назад до планування проєкту
    </button>
  </div>
</template>

<script setup>
import { Bar } from "vue-chartjs";
import ChartDataLabels from "chartjs-plugin-datalabels";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
} from "chart.js";
ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, ChartDataLabels);

const props = defineProps(["chartData"]);
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
        text: "Номери задач",
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
      },
    },
  },
  plugins: {
    legend: {
      display: false,
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
</script>

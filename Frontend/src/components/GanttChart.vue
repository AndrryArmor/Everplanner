<template>
  <div class="container-fluid text-center">
    <h4>Діаграма Ганта для проєкту:</h4>
    <Bar id="my-chart-id" :options="chartOptions" :data="chartData" :style="style" />
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
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

const chartData = {
  labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"],
  datasets: [
    {
      datalabels: {
        labels: {
          title: null,
        },
      },
      data: [0, 0, 0, 2.5, 3.333, 6, 5.5, 6, 8.333, 9],
      backgroundColor: "rgba(0, 0, 0, 0)",
    },
    {
      label: "1",
      data: [0, 3.333, 0, 0, 2.667, 2.333, 0, 0, 0.667, 1.667],
      backgroundColor: "rgb(255, 160, 100)",
    },
    {
      label: "2",
      data: [2.5, 0, 0, 3, 0, 0, 0, 2, 0, 0],
      backgroundColor: "rgb(160, 120, 180)",
    },
    {
      label: "3",
      data: [0, 0, 3.333, 0, 0, 0, 1.667, 0, 0, 0],
      backgroundColor: "rgb(120, 255, 40)",
    },
    {
      label: "4",
      data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
      backgroundColor: "rgb(70, 90, 255)",
    },
  ],
};
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
</script>

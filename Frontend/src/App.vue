<template>
  <Planner v-if="mode === Modes.ShowPlanner" @plan-project="planProject" />
  <GanttChart
    v-else-if="mode === Modes.ShowChart"
    :chart-data="chartData"
    @back-to-planning="backToPlanning"
  />
</template>

<script setup>
import { ref, computed } from "vue";
import Planner from "./components/Planner.vue";
import GanttChart from "./components/GanttChart.vue";

const Modes = {
  ShowPlanner: "ShowPlanner",
  ShowChart: "ShowChart",
};
const mode = ref(Modes.ShowPlanner);

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

function planProject(tasks, workers) {
  mode.value = Modes.ShowChart;
}

function backToPlanning() {
  mode.value = Modes.ShowPlanner;
}
</script>

<style scoped></style>

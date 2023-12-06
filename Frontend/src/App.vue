<template>
  <Planner v-if="mode === Modes.ShowPlanner" @plan-project="planProject" />
  <GanttChart v-else-if="mode === Modes.ShowChart" />
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

function planProject(tasks, workers) {
  mode.value = Modes.ShowChart;
}
</script>

<style scoped></style>

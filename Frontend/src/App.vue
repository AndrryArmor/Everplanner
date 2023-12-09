<template>
  <Planner v-if="mode === Modes.ShowPlanner" @plan-project="planProject" />
  <GanttChart
    v-else-if="mode === Modes.ShowChart"
    :chart-data="chartData"
    :project-name="projectName"
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
const chartData = ref();
const projectName = ref();

async function planProject(tasks, workers) {
  //const project = getTemplateProject();
  const project = {
    id: 0,
    name: "Test project",
    tasks: tasks,
    workers: workers,
  };
  var chartDataResponse;
  try {
    chartDataResponse = await fetch("https://localhost:7229/Everplanner/PlanProject", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(project),
    })
      .then(async (response) => {
        if (!response.ok) {
          const text = await response.text();
          throw new Error(JSON.parse(text).detail);
        }
        return response.json();
      })
      .then((projectData) => {
        return generateChartDataFromPlannedProject(projectData);
      });
  } catch (error) {
    alert(`Проєкт не вдалося спланувати через наступну помилку: ${error.message}`);
  }

  if (chartDataResponse) {
    chartData.value = chartDataResponse;
    mode.value = Modes.ShowChart;
  }
}

function getTemplateProject() {
  return {
    id: 0,
    name: "Test project",
    tasks: [
      {
        id: 1,
        name: "Створити діаграму",
        complexity: 5,
        parentTasks: [],
        availableWorkers: [1, 2, 3, 4],
      },
      {
        id: 2,
        name: "Створити діаграму",
        complexity: 10,
        parentTasks: [],
        availableWorkers: [1, 2, 3, 4],
      },
      {
        id: 3,
        name: "Створити діаграму",
        complexity: 2,
        parentTasks: [],
        availableWorkers: [1, 2, 3, 4],
      },
      {
        id: 4,
        name: "Створити діаграму",
        complexity: 6,
        parentTasks: [],
        availableWorkers: [1, 2, 3, 4],
      },
      {
        id: 5,
        name: "Створити діаграму",
        complexity: 8,
        parentTasks: [1],
        availableWorkers: [1, 2, 3, 4],
      },
      {
        id: 6,
        name: "Створити діаграму",
        complexity: 7,
        parentTasks: [1, 2, 3],
        availableWorkers: [1, 2, 3, 4],
      },
      {
        id: 7,
        name: "Створити діаграму",
        complexity: 1,
        parentTasks: [3, 4],
        availableWorkers: [1, 2, 3, 4],
      },
      {
        id: 8,
        name: "Створити діаграму",
        complexity: 4,
        parentTasks: [5],
        availableWorkers: [1, 2, 3, 4],
      },
      {
        id: 9,
        name: "Створити діаграму",
        complexity: 2,
        parentTasks: [6, 7],
        availableWorkers: [1, 2, 3, 4],
      },
      {
        id: 10,
        name: "Створити діаграму",
        complexity: 5,
        parentTasks: [8, 9],
        availableWorkers: [1, 2, 3, 4],
      },
    ],
    workers: [
      {
        id: 1,
        name: "Співробітник 1",
        salary: 150,
        developmentVelocity: 15,
      },
      {
        id: 2,
        name: "Співробітник 2",
        salary: 100,
        developmentVelocity: 10,
      },
      {
        id: 3,
        name: "Співробітник 3",
        salary: 30,
        developmentVelocity: 3,
      },
      {
        id: 4,
        name: "Співробітник 4",
        salary: 20,
        developmentVelocity: 2,
      },
    ],
  };
}

function generateChartDataFromPlannedProject(project) {
  projectName.value = project.name;
  const labels = new Array(project.tasks.length);
  const shift = new Array(project.tasks.length);
  const workerBars = project.workers.map((worker) => {
    return {
      label: worker.name,
      data: new Array(project.tasks.length),
    };
  });

  for (let i = 0; i < project.tasks.length; i++) {
    const task = project.tasks[i];
    labels[i] = task.name;
    shift[i] = task.executionStart;
    for (let j = 0; j < project.workers.length; j++) {
      const worker = project.workers[j];
      workerBars[j].data[i] = task.executor === worker.id ? task.executionDuration : 0;
    }
  }

  return {
    labels: labels,
    datasets: [
      {
        datalabels: {
          labels: {
            title: null,
          },
        },
        data: shift,
        backgroundColor: "rgba(0, 0, 0, 0)",
      },
      ...workerBars,
    ],
  };
}

function backToPlanning() {
  mode.value = Modes.ShowPlanner;
}
</script>

<style scoped></style>

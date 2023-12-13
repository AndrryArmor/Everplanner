<template>
  <TheNavbar />
  <router-view></router-view>
</template>

<script setup>
import { ref } from "vue";
import TheNavbar from "./components/TheNavbar.vue";

const Modes = {
  ShowLogin: "ShowLogin",
  ShowProjects: "ShowProjects",
  ShowPlanner: "ShowPlanner",
  ShowChart: "ShowChart",
};

const mode = ref(Modes.ShowLogin);
const chartData = ref(null);
const projectStats = ref(null);

function showProject(project) {
  mode.value = Modes.ShowPlanner;
}

function backToProjects() {
  mode.value = Modes.ShowProjects;
}

async function planProjectForMinimalTime(tasks, workers) {
  const project = {
    id: 0,
    name: "Test project",
    tasks: tasks,
    workers: workers,
  };
  var chartDataResponse;
  try {
    chartDataResponse = await fetch(
      "https://localhost:7229/Everplanner/PlanProjectForMinimalTime",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(project),
      }
    )
      .then(async (response) => {
        if (!response.ok) {
          const text = await response.text();
          throw new Error(JSON.parse(text).detail);
        }
        return response.json();
      })
      .then((projectData) => {
        return generateChartDataFromPlannedProject(projectData, "мінімальний час");
      });
  } catch (error) {
    alert(error.message);
  }

  if (chartDataResponse) {
    chartData.value = chartDataResponse;
    mode.value = Modes.ShowChart;
  }
}

async function planProjectForMinimalWorkersCount(tasks, workers) {
  const expectedProjectDuration = Number.parseInt(
    prompt("Введіть очікувану тривалість проєкту:"),
    10
  );
  const project = {
    id: 0,
    name: "Test project",
    tasks: tasks,
    workers: workers,
    expectedProjectDuration: expectedProjectDuration,
  };
  var chartDataResponse;
  try {
    chartDataResponse = await fetch(
      "https://localhost:7229/Everplanner/PlanProjectForMinimalWorkersCount",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(project),
      }
    )
      .then(async (response) => {
        if (!response.ok) {
          const text = await response.text();
          throw new Error(JSON.parse(text).detail);
        }
        return response.json();
      })
      .then((projectData) => {
        return generateChartDataFromPlannedProject(
          projectData,
          "мінімальну кількість співробітників",
          expectedProjectDuration
        );
      });
  } catch (error) {
    alert(error.message);
  }

  if (chartDataResponse) {
    chartData.value = chartDataResponse;
    mode.value = Modes.ShowChart;
  }
}

function generateChartDataFromPlannedProject(project, titleTrait, expectedProjectDuration) {
  projectStats.value = {
    name: project.name,
    titleTrait: titleTrait,
    endingTime: project.endingTime,
    tasks: project.tasks.length,
    workers: project.workers.length,
    usedWorkers: project.usedWorkersCount,
  };
  if (expectedProjectDuration) {
    projectStats.value.expectedProjectDuration = expectedProjectDuration;
  }

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

<style lang="scss">
@import "@/assets/variables.scss";
.table {
  td {
    padding-top: 0.25em;
    padding-bottom: 0.25em;

    button.action {
      border-width: 0px;
      margin: -0.25em -0.25em -0.25em -0.25em;
    }
  }

  .btn-close {
    font-size: 0.75em;
  }

  .inline-item {
    padding: 0.25rem 0.5rem;
    border: 1px solid;
    border-radius: 0.25rem;
    font-size: 0.875rem;
  }

  .dropdown {
    position: static;

    .dropdown-menu {
      height: auto;
      max-height: 200px;
      overflow-x: hidden;
    }
  }
}
</style>

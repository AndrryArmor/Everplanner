<template>
  <div
    v-if="isLoading"
    class="spinner-border text-primary position-absolute top-50 start-50"
    style="width: 3rem; height: 3rem"
  ></div>
  <div v-else class="m-3">
    <h4 class="ms-5">
      Результати планування проєкту "{{ plannedProject.name }}" за
      {{ getModeTitle($route.query.mode) }}:
    </h4>
    <template v-if="plannedProject.endingTime > 0">
      <p>
        Час виконання проєкту: <b>{{ plannedProject.endingTime.toFixed(2) }} днів.</b><br />
        Необхідна кількість працівників:
        <b>{{ plannedProject.usedWorkersCount }}/{{ plannedProject.workers.length }}</b>
      </p>
      <b>Діаграма Ганта:</b>
      <div class="overflow-x-auto">
        <BarChart
          :chart-data="generateChartDataFromPlannedProject()"
          :options="chartOptions"
          :style="style"
        />
      </div>
    </template>
    <template v-else>
      <p>
        Проєкт неможливо виконати{{
          route.query.expectedProjectDuration
            ? ` за ${route.query.expectedProjectDuration} днів`
            : ""
        }}.
      </p>
    </template>
    <button type="button" class="btn btn-lg btn-primary m-2" @click="backToPlanning">
      Назад до планування проєкту
    </button>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRoute } from "vue-router";
import router from "../router";
import { BarChart } from "vue-chart-3";
import ChartDataLabels from "chartjs-plugin-datalabels";
import autocolors from "chartjs-plugin-autocolors";
import { Chart, registerables } from "chart.js";
import { defaults } from "chart.js";
Chart.register(...registerables, ChartDataLabels, autocolors);

const isLoading = ref(true);
const route = useRoute();

const plannedProject = ref(null);

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
        callback: function (value) {
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
        filter: function (item) {
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

const style = computed(() => {
  if (isLoading) {
    return;
  }

  return {
    width: `max(100vw - 2em - 20px, ${500 + 31 * plannedProject.value.endingTime}px)`,
    "max-width": `max(100vw - 2em - 20px, ${500 + 31 * plannedProject.value.endingTime}px)`,
    height: `max(100px + 25vh, ${100 + 40 * plannedProject.value.tasks}px)`,
    "max-height": `max(100px + 25vh, ${100 + 40 * plannedProject.value.tasks}px)`,
  };
});

function isLightColor(color) {
  const rgb = color.match(/\d+/g).map(Number);
  const brightness = (rgb[0] * 299 + rgb[1] * 587 + rgb[2] * 114) / 1000;
  return brightness > 128;
}

function generateChartDataFromPlannedProject() {
  const labels = new Array(plannedProject.value.tasks.length);
  const shift = new Array(plannedProject.value.tasks.length);
  const workerBars = plannedProject.value.workers.map((worker) => {
    return {
      label: worker.name,
      data: new Array(plannedProject.value.tasks.length),
    };
  });

  for (let i = 0; i < plannedProject.value.tasks.length; i++) {
    const task = plannedProject.value.tasks[i];
    labels[i] = task.name;
    shift[i] = task.executionStart;
    for (let j = 0; j < plannedProject.value.workers.length; j++) {
      const worker = plannedProject.value.workers[j];
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
  router.push({
    path: `/users/${route.params.userId}/projects/${route.params.projectId}`,
  });
}

function getModeTitle(mode) {
  switch (mode) {
    case "MinimalTime":
      return "мінімальний час";
    case "MinimalWorkersCount":
      return "мінімальну кількість співробітників";
    default:
      alert(`${mode} є недійсним значенням для способу планування проєкту.`);
  }
}

onMounted(async () => {
  try {
    var queryString = new URLSearchParams(route.query).toString();
    plannedProject.value = await fetch(
      `https://localhost:7229/api/users/${route.params.userId}/projects/${route.params.projectId}/plan?${queryString}`
    ).then(async (response) => {
      const res = await response.text();
      if (!response.ok) {
        throw new Error(res);
      }
      return JSON.parse(res);
    });
  } catch (error) {
    alert(error.message);
  }
  isLoading.value = false;
});
</script>

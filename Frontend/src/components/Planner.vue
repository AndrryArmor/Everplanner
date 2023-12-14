<template>
  <div
    v-if="isLoading"
    class="spinner-border text-primary position-absolute top-50 start-50"
    style="width: 3rem; height: 3rem"
  ></div>
  <div v-else>
    <h1 class="ms-5 mt-3">Проєкт "{{ project.name }}"</h1>
    <form class="container-fluid mt-3 workers">
      <h4 class="ms-5">Співробітники</h4>
      <div class="row overflow-x-auto">
        <div class="col-auto">
          <table class="table align-middle m-0">
            <thead class="text-center">
              <tr>
                <th v-for="header in workersTableHeaders">
                  {{ header }}
                </th>
              </tr>
            </thead>
            <tbody>
              <PlannerWorker
                v-for="(worker, index) in project.workers"
                :key="worker.id"
                :row-index="index + 1"
                :worker="worker"
                :dollar-sign="dollarSign"
                :development-velocity-metric="developmentVelocityMetric"
                @delete-worker="deleteWorker"
              />
              <PlannerNewWorker
                :row-index="project.workers.length + 1"
                :dollar-sign="dollarSign"
                :development-velocity-metric="developmentVelocityMetric"
                @add-new-worker="addNewWorker"
              />
            </tbody>
          </table>
        </div>
      </div>
    </form>
    <form class="container-fluid mt-3 tasks">
      <h4 class="ms-5">Задачі</h4>
      <div class="row overflow-x-auto">
        <div class="col-auto">
          <table class="table align-middle m-0">
            <thead class="text-center">
              <tr>
                <th v-for="header in tasksTableHeaders">
                  {{ header }}
                </th>
              </tr>
            </thead>
            <tbody>
              <PlannerTask
                v-for="(task, index) in project.tasks"
                :key="task.id"
                :row-index="index + 1"
                :task="task"
                :tasks="project.tasks"
                :workers="project.workers"
                :story-points-sign="storyPointsSign"
                @delete-task="deleteTask"
                @add-parent-task="addParentTask"
                @delete-parent-task="deleteParentTask"
                @add-available-worker="addAvailableWorker"
                @delete-available-worker="deleteAvailableWorker"
              />
              <PlannerNewTask
                :row-index="project.tasks.length + 1"
                :tasks="project.tasks"
                :workers="project.workers"
                :story-points-sign="storyPointsSign"
                @add-new-task="addTask"
              />
            </tbody>
          </table>
        </div>
      </div>
    </form>
    <div class="container-fluid my-3 actions">
      <div class="row gy-3">
        <div class="col-lg-auto">
          <button
            type="button"
            class="btn btn-lg btn-primary w-100"
            @click="planProject(Modes.MinimalTime)"
          >
            Cпланувати проєкт за мінімальний час
          </button>
        </div>
        <div class="col-lg-auto">
          <button
            type="button"
            class="btn btn-lg btn-primary w-100"
            @click="planProject(Modes.MinimalWorkersCount)"
          >
            Cпланувати проєкт за мінімальну кількість співробітників
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watchEffect, onMounted } from "vue";
import { useRoute } from "vue-router";
import router from "../router";
import PlannerWorker from "./PlannerWorker.vue";
import PlannerNewWorker from "./PlannerNewWorker.vue";
import PlannerTask from "./PlannerTask.vue";
import PlannerNewTask from "./PlannerNewTask.vue";

const emit = defineEmits([
  "back-to-projects",
  "plan-project-for-minimal-time",
  "plan-project-for-minimal-workers-count",
]);

const dollarSign = "$";
const storyPointsSign = "SP";
const developmentVelocityMetric = `${storyPointsSign}/тиж.`;
const Modes = {
  MinimalTime: "MinimalTime",
  MinimalWorkersCount: "MinimalWorkersCount",
};

const isLoading = ref(true);
const route = useRoute();

const project = ref(null);

const workersTableHeaders = ["", "№", "ПІБ", "Зарплата", "Швидкість розробки"];

async function addNewWorker(worker) {
  try {
    const newWorkerId = await fetch(
      `https://localhost:7229/api/users/${route.params.userId}/projects/${route.params.projectId}/workers`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(worker),
      }
    ).then(async (response) => {
      const res = await response.text();
      if (!response.ok) {
        throw new Error(res);
      }
      return parseInt(res);
    });

    worker.id = newWorkerId;
    project.value.workers = [...project.value.workers, worker];
  } catch (error) {
    alert(error.message);
  }
}

async function deleteWorker(workerId) {
  try {
    await fetch(
      `https://localhost:7229/api/users/${route.params.userId}/projects/${route.params.projectId}/workers/${workerId}`,
      {
        method: "DELETE",
      }
    ).then(async (response) => {
      if (!response.ok) {
        throw new Error(await response.text());
      }
    });

    project.value = await getProject();
  } catch (error) {
    alert(error.message);
  }
}

const tasksTableHeaders = [
  "",
  "№",
  "Назва задачі",
  "Складність",
  "Залежить від",
  "Доступні співробітники для виконання",
];

async function addTask(task) {
  try {
    const newTaskId = await fetch(
      `https://localhost:7229/api/users/${route.params.userId}/projects/${route.params.projectId}/tasks`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(task),
      }
    ).then(async (response) => {
      const res = await response.text();
      if (!response.ok) {
        throw new Error(res);
      }
      return parseInt(res);
    });

    task.id = newTaskId;
    project.value.tasks = [...project.value.tasks, task];
  } catch (error) {
    alert(error.message);
  }
}

async function updateTask(task) {
  try {
    await fetch(
      `https://localhost:7229/api/users/${route.params.userId}/projects/${route.params.projectId}/tasks/${task.id}`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(task),
      }
    ).then(async (response) => {
      if (!response.ok) {
        throw new Error(await response.text());
      }
    });
  } catch (error) {
    alert(error.message);
  }
}

async function deleteTask(taskId) {
  try {
    await fetch(
      `https://localhost:7229/api/users/${route.params.userId}/projects/${route.params.projectId}/tasks/${taskId}`,
      {
        method: "DELETE",
      }
    ).then(async (response) => {
      if (!response.ok) {
        throw new Error(await response.text());
      }
    });

    project.value = await getProject();
  } catch (error) {
    alert(error.message);
  }
}

async function addParentTask(taskId, parentTaskId) {
  var foundTask = project.value.tasks.find((task) => task.id == taskId);
  updateTask(foundTask);
  foundTask.parentTasks = [...foundTask.parentTasks, parentTaskId];
}

async function deleteParentTask(taskId, parentTaskId) {
  var foundTask = project.value.tasks.find((task) => task.id == taskId);
  updateTask(foundTask);
  foundTask.parentTasks = foundTask.parentTasks.filter((task) => task != parentTaskId);
}

async function addAvailableWorker(taskId, workerId) {
  var foundTask = project.value.tasks.find((task) => task.id == taskId);
  updateTask(foundTask);
  foundTask.availableWorkers = [...foundTask.availableWorkers, workerId];
}

async function deleteAvailableWorker(taskId, workerId) {
  var foundTask = project.value.tasks.find((task) => task.id == taskId);
  updateTask(foundTask);
  foundTask.availableWorkers = foundTask.availableWorkers.filter((task) => task != workerId);
}

function planProject(mode) {
  if (mode === Modes.MinimalTime) {
    router.push({
      path: `/users/${route.params.userId}/projects/${route.params.projectId}/plan`,
      query: { mode: mode },
    });
  } else if (mode === Modes.MinimalWorkersCount) {
    const expectedProjectDuration = Number.parseInt(
      prompt("Введіть очікувану тривалість проєкту:"),
      10
    );
    router.push({
      path: `/users/${route.params.userId}/projects/${route.params.projectId}/plan`,
      query: { mode: mode, expectedProjectDuration: expectedProjectDuration },
    });
  } else {
    alert(`${mode} є недійсним значенням для способу планування проєкту.`);
  }
}

async function getProject() {
  var project = null;
  try {
    project = await fetch(
      `https://localhost:7229/api/users/${route.params.userId}/projects/${route.params.projectId}`
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

  return project;
}

onMounted(async () => {
  project.value = await getProject();
  isLoading.value = false;
});
</script>

<style scoped lang="scss">
@import "@/assets/_variables.scss";

$td-badges-width: 500px;
$td-worker-name-width: 300px;
$td-salary-width: 150px;
$td-task-name-width: 300px;
$td-velocity-width: 200px;
$td-complexity-width: 125px;

.workers .table {
  th:nth-child(1),
  :deep(td:nth-child(1)) {
    width: $td-action-width;
    min-width: $td-action-width;
    max-width: $td-action-width;
    text-align: center;
  }

  th:nth-child(2),
  :deep(td:nth-child(2)) {
    width: $td-id-width;
    min-width: $td-id-width;
    max-width: $td-id-width;
    text-align: center;
  }

  th:nth-child(3),
  :deep(td:nth-child(3)) {
    width: $td-worker-name-width;
    min-width: $td-worker-name-width;
    max-width: $td-worker-name-width;
  }

  th:nth-child(4),
  :deep(td:nth-child(4)) {
    width: $td-salary-width;
    min-width: $td-salary-width;
    max-width: $td-salary-width;
    text-align: center;
  }

  th:nth-child(5),
  :deep(td:nth-child(5)) {
    width: $td-velocity-width;
    min-width: $td-velocity-width;
    max-width: $td-velocity-width;
    text-align: center;
  }
}

.tasks .table {
  th:nth-child(1),
  :deep(td:nth-child(1)) {
    width: $td-action-width;
    min-width: $td-action-width;
    max-width: $td-action-width;
    text-align: center;
  }

  th:nth-child(2),
  :deep(td:nth-child(2)) {
    width: $td-id-width;
    min-width: $td-id-width;
    max-width: $td-id-width;
    text-align: center;
  }

  th:nth-child(3),
  :deep(td:nth-child(3)) {
    width: $td-task-name-width;
    min-width: $td-task-name-width;
    max-width: $td-task-name-width;
  }

  th:nth-child(4),
  :deep(td:nth-child(4)) {
    width: $td-complexity-width;
    min-width: $td-complexity-width;
    max-width: $td-complexity-width;
    text-align: center;
  }

  th:nth-child(5),
  :deep(td:nth-child(5)) {
    width: $td-badges-width;
    min-width: $td-badges-width;
    max-width: $td-badges-width;
  }

  th:nth-child(6),
  :deep(td:nth-child(6)) {
    width: $td-badges-width;
    min-width: $td-badges-width;
    max-width: $td-badges-width;
  }
}
</style>

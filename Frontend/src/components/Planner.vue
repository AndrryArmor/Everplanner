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
                :new-worker-id="newWorkerId"
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
                :new-task-id="newTaskId"
                :tasks="project.tasks"
                :workers="project.workers"
                :story-points-sign="storyPointsSign"
                @add-new-task="addNewTask"
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
import Project from "./Project.vue";

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

function backToProjects() {
  emit("back-to-projects");
}

const workersTableHeaders = ["", "№", "ПІБ", "Зарплата", "Швидкість розробки"];

const newWorkerId = computed(() => {
  if (project.value.workers.length === 0) {
    return 1;
  }

  const sortedWorkers = [...project.value.workers].sort(
    (worker1, worker2) => worker1.id - worker2.id
  );
  return sortedWorkers[sortedWorkers.length - 1].id + 1;
});

function addNewWorker(worker) {
  project.value.workers = [...project.value.workers, worker];
}

function deleteWorker(workerId) {
  project.value.workers = project.value.workers.filter((worker) => worker.id != workerId);
}

const tasksTableHeaders = [
  "",
  "№",
  "Назва задачі",
  "Складність",
  "Залежить від",
  "Доступні співробітники для виконання",
];

const newTaskId = computed(() => {
  if (project.value.tasks.length === 0) {
    return 1;
  }

  var sortedTasks = [...project.value.tasks].sort((task1, task2) => task1.id - task2.id);
  return sortedTasks[sortedTasks.length - 1].id + 1;
});

// Видаляє неіснуючі батьківські задачі зі списку батьків кожної з задач
// і неіснуючих співробітників із списку доступнимх співробітників кожної з задач.
watchEffect(() => {
  if (isLoading.value) {
    return;
  }

  project.value.tasks.forEach((task) => {
    task.parentTasks = task.parentTasks.filter((parentTask) => {
      // Шукає чи є серед усіх задач хоча б одна, яка дорівнює parentTask.
      // Якщо нема - значить parentTask це видалена задача.
      return project.value.tasks.some((t) => t.id == parentTask);
    });

    task.availableWorkers = task.availableWorkers.filter((worker) => {
      // Шукає чи є серед усіх співробітників хоча б один, який дорівнює worker.
      // Якщо нема - значить worker це видалений співробітник.
      return project.value.workers.some((w) => w.id == worker);
    });
  });
});

function addNewTask(task) {
  project.value.tasks = [...project.value.tasks, task];
}

function deleteTask(taskId) {
  project.value.tasks = project.value.tasks.filter((task) => task.id != taskId);
}

function addParentTask(taskId, parentTaskId) {
  var foundTask = project.value.tasks.find((task) => task.id == taskId);
  foundTask.parentTasks = [...foundTask.parentTasks, parentTaskId];
}

function deleteParentTask(taskId, parentTaskId) {
  var foundTask = project.value.tasks.find((task) => task.id == taskId);
  foundTask.parentTasks = foundTask.parentTasks.filter((task) => task != parentTaskId);
}

function addAvailableWorker(taskId, workerId) {
  var foundTask = project.value.tasks.find((task) => task.id == taskId);
  foundTask.availableWorkers = [...foundTask.availableWorkers, workerId];
}

function deleteAvailableWorker(taskId, workerId) {
  var foundTask = project.value.tasks.find((task) => task.id == taskId);
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

onMounted(async () => {
  try {
    project.value = await fetch(
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

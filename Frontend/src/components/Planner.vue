<template>
  <form class="container-fluid mt-3 workers">
    <h4 class="ms-5">Співробітники</h4>
    <div class="row overflow-x-auto">
      <div class="col-auto">
        <table class="table m-0">
          <thead>
            <tr>
              <th v-for="header in workersTableHeaders">
                {{ header }}
              </th>
            </tr>
          </thead>
          <tbody>
            <PlannerWorker
              v-for="worker in workers"
              :key="worker.id"
              :worker="worker"
              :dollar-sign="dollarSign"
              :development-velocity-metric="developmentVelocityMetric"
              @delete-worker="deleteWorker"
            />
            <PlannerNewWorker
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
        <table class="table m-0">
          <thead>
            <tr>
              <th v-for="header in tasksTableHeaders">
                {{ header }}
              </th>
            </tr>
          </thead>
          <tbody>
            <PlannerTask
              v-for="task in tasks"
              :key="task.id"
              :task="task"
              :tasks="tasks"
              :workers="workers"
              :story-points-sign="storyPointsSign"
              @delete-task="deleteTask"
              @add-parent-task="addParentTask"
              @delete-parent-task="deleteParentTask"
              @add-available-worker="addAvailableWorker"
              @delete-available-worker="deleteAvailableWorker"
            />
            <PlannerNewTask
              :new-task-id="newTaskId"
              :tasks="tasks"
              :workers="workers"
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
        <button type="button" class="btn btn-lg btn-primary" @click="planProjectForMinimalTime">
          Cпланувати проєкт за мінімальний час
        </button>
      </div>
      <div class="col-lg-auto">
        <button
          type="button"
          class="btn btn-lg btn-primary"
          @click="planProjectForMinimalWorkersCount"
        >
          Cпланувати проєкт за мінімальну кількість співробітників
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watchEffect } from "vue";
import PlannerWorker from "./PlannerWorker.vue";
import PlannerNewWorker from "./PlannerNewWorker.vue";
import PlannerTask from "./PlannerTask.vue";
import PlannerNewTask from "./PlannerNewTask.vue";

const emit = defineEmits(["planProjectForMinimalTime", "planProjectForMinimalWorkersCount"]);

const dollarSign = "$";
const storyPointsSign = "SP";
const developmentVelocityMetric = `${storyPointsSign}/тиж.`;

const workersTableHeaders = ["", "ID", "ПІБ", "Зарплата", "Швидкість розробки"];
const workers = ref([
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
]);

const newWorkerId = computed(() => {
  if (workers.value.length === 0) {
    return 1;
  }

  const sortedWorkers = [...workers.value].sort((worker1, worker2) => worker1.id - worker2.id);
  return sortedWorkers[sortedWorkers.length - 1].id + 1;
});

function addNewWorker(worker) {
  workers.value = [...workers.value, worker];
}

function deleteWorker(workerId) {
  workers.value = workers.value.filter((worker) => worker.id != workerId);
}

const tasksTableHeaders = [
  "",
  "ID",
  "Назва задачі",
  "Складність",
  "Залежить від",
  "Доступні співробітники для виконання",
];
const tasks = ref([
  {
    id: 1,
    name: "Задача 1",
    complexity: 5,
    parentTasks: [],
    availableWorkers: [1, 2, 3, 4],
  },
  {
    id: 2,
    name: "Задача 2",
    complexity: 10,
    parentTasks: [],
    availableWorkers: [1, 2, 3, 4],
  },
  {
    id: 3,
    name: "Задача 3",
    complexity: 2,
    parentTasks: [],
    availableWorkers: [1, 2, 3, 4],
  },
  {
    id: 4,
    name: "Задача 4",
    complexity: 6,
    parentTasks: [],
    availableWorkers: [1, 2, 3, 4],
  },
  {
    id: 5,
    name: "Задача 5",
    complexity: 8,
    parentTasks: [1],
    availableWorkers: [1, 2, 3, 4],
  },
  {
    id: 6,
    name: "Задача 6",
    complexity: 7,
    parentTasks: [1, 2, 3],
    availableWorkers: [1, 2, 3, 4],
  },
  {
    id: 7,
    name: "Задача 7",
    complexity: 1,
    parentTasks: [3, 4],
    availableWorkers: [1, 2, 3, 4],
  },
  {
    id: 8,
    name: "Задача 8",
    complexity: 4,
    parentTasks: [5],
    availableWorkers: [1, 2, 3, 4],
  },
  {
    id: 9,
    name: "Задача 9",
    complexity: 2,
    parentTasks: [6, 7],
    availableWorkers: [1, 2, 3, 4],
  },
  {
    id: 10,
    name: "Задача 10",
    complexity: 5,
    parentTasks: [8, 9],
    availableWorkers: [1, 2, 3, 4],
  },
]);

const newTaskId = computed(() => {
  if (tasks.value.length === 0) {
    return 1;
  }

  var sortedTasks = [...tasks.value].sort((task1, task2) => task1.id - task2.id);
  return sortedTasks[sortedTasks.length - 1].id + 1;
});

// Видаляє неіснуючі батьківські задачі зі списку батьків кожної з задач
// і неіснуючих співробітників із списку доступнимх співробітників кожної з задач.
watchEffect(() => {
  tasks.value.forEach((task) => {
    task.parentTasks = task.parentTasks.filter((parentTask) => {
      // Шукає чи є серед усіх задач хоча б одна, яка дорівнює parentTask.
      // Якщо нема - значить parentTask це видалена задача.
      return tasks.value.some((t) => t.id == parentTask);
    });

    task.availableWorkers = task.availableWorkers.filter((worker) => {
      // Шукає чи є серед усіх співробітників хоча б один, який дорівнює worker.
      // Якщо нема - значить worker це видалений співробітник.
      return workers.value.some((w) => w.id == worker);
    });
  });
});

function addNewTask(task) {
  tasks.value = [...tasks.value, task];
}

function deleteTask(taskId) {
  tasks.value = tasks.value.filter((task) => task.id != taskId);
}

function addParentTask(taskId, parentTaskId) {
  var foundTask = tasks.value.find((task) => task.id == taskId);
  foundTask.parentTasks = [...foundTask.parentTasks, parentTaskId];
}

function deleteParentTask(taskId, parentTaskId) {
  var foundTask = tasks.value.find((task) => task.id == taskId);
  foundTask.parentTasks = foundTask.parentTasks.filter((task) => task != parentTaskId);
}

function addAvailableWorker(taskId, workerId) {
  var foundTask = tasks.value.find((task) => task.id == taskId);
  foundTask.availableWorkers = [...foundTask.availableWorkers, workerId];
}

function deleteAvailableWorker(taskId, workerId) {
  var foundTask = tasks.value.find((task) => task.id == taskId);
  foundTask.availableWorkers = foundTask.availableWorkers.filter((task) => task != workerId);
}

function planProjectForMinimalTime() {
  emit("planProjectForMinimalTime", tasks.value, workers.value);
}

function planProjectForMinimalWorkersCount() {
  emit("planProjectForMinimalWorkersCount", tasks.value, workers.value);
}
</script>

<style scoped lang="scss">
.table {
  vertical-align: middle;

  th {
    text-align: center;
  }

  :deep(td) {
    padding-top: 0.25em;
    padding-bottom: 0.25em;

    button.action {
      border-width: 0px;
      margin: -0.25em -0.25em -0.25em -0.25em;
    }
  }

  :deep(.btn-close) {
    font-size: 0.75em;
  }

  :deep(.inline-item) {
    padding: 0.25rem 0.5rem;
    border: 1px solid;
    border-radius: 0.25rem;
    font-size: 0.875rem;
  }

  :deep(.dropdown) {
    position: static;

    .dropdown-menu {
      height: auto;
      max-height: 200px;
      overflow-x: hidden;
    }
  }
}

$td-trashbin-width: 30px;
$td-id-width: 50px;
$td-badges-width: 500px;
$td-worker-name-width: 300px;
$td-salary-width: 150px;
$td-task-name-width: 300px;
$td-velocity-width: 200px;
$td-complexity-width: 125px;

.workers .table {
  th:nth-child(1),
  :deep(td:nth-child(1)) {
    width: $td-trashbin-width;
    min-width: $td-trashbin-width;
    max-width: $td-trashbin-width;
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
    width: $td-trashbin-width;
    min-width: $td-trashbin-width;
    max-width: $td-trashbin-width;
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

.actions button {
  width: 100%;
}
</style>

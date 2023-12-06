<template>
  <form>
    <div class="container-fluid mt-3 workers">
      <h4 class="ms-5">Співробітники</h4>
      <div class="row overflow-x-auto">
        <div class="col-auto">
          <table class="table m-0">
            <thead>
              <th v-for="header in workersTableHeaders">
                {{ header }}
              </th>
            </thead>
            <tbody>
              <PlannerWorker
                v-for="worker in workers"
                :key="worker.id"
                :worker="worker"
                :dollar-sign="dollarSign"
                :development-velocity-metric="developmentVelocityMetric"
              />
              <PlannerNewWorker
                :new-worker-id="getNewWorkerId()"
                :dollar-sign="dollarSign"
                :development-velocity-metric="developmentVelocityMetric"
              />
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <div class="container-fluid mt-3 tasks">
      <h4 class="ms-5">Задачі</h4>
      <div class="row overflow-x-auto">
        <div class="col-auto">
          <table class="table m-0">
            <thead>
              <th v-for="header in tasksTableHeaders">
                {{ header }}
              </th>
            </thead>
            <tbody>
              <PlannerTask
                v-for="task in tasks"
                :key="task.id"
                :task="task"
                :tasks="tasks"
                :workers="workers"
                :story-points-sign="storyPointsSign"
              />
              <PlannerNewTask
                :new-task-id="getNewTaskId()"
                :tasks="tasks"
                :workers="workers"
                :story-points-sign="storyPointsSign"
              />
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <button type="button" class="btn btn-lg btn-primary m-2">Cпланувати проєкт</button>
  </form>
</template>

<script setup>
import { ref, computed } from "vue";
import PlannerWorker from "./PlannerWorker.vue";
import PlannerNewWorker from "./PlannerNewWorker.vue";
import PlannerTask from "./PlannerTask.vue";
import PlannerNewTask from "./PlannerNewTask.vue";

const dollarSign = "$";
const storyPointsSign = "SP";
const developmentVelocityMetric = `${storyPointsSign}/тиж.`;

const workersTableHeaders = ["", "ID", "ПІБ", "Зарплата", "Швидкість розробки"];
const workers = ref([
  {
    id: 0,
    name: "Ачілов А. В.",
    salary: 1000,
    developmentVelocity: 15,
  },
  {
    id: 1,
    name: "Хвостенко О. Д.",
    salary: 800,
    developmentVelocity: 10,
  },
]);

function getNewWorkerId() {
  var sortedWorkers = workers.value.toSorted((worker1, worker2) => worker1.id - worker2.id);
  return sortedWorkers[sortedWorkers.length - 1].id + 1;
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
    id: 0,
    name: "Створити діаграму",
    complexity: 8,
    parentTasks: [],
    availableWorkers: [0, 1],
  },
  {
    id: 1,
    name: "Додати додавання спіробітників та задач",
    complexity: 15,
    parentTasks: [0],
    availableWorkers: [1],
  },
]);

function getNewTaskId() {
  var sortedTasks = tasks.value.toSorted((task1, task2) => task1.id - task2.id);
  return sortedTasks[sortedTasks.length - 1].id + 1;
}
</script>

<style scoped lang="scss">
.table {
  vertical-align: middle;

  th {
    text-align: center;
  }

  :deep(td) button.trashbin {
    border-width: 0px;
    margin: -0.25em -0.25em -0.25em -0.25em;
  }

  :deep(.btn-close) {
    font-size: 0.75em;
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

:deep(.inline-item) {
  padding: 0.25rem 0.5rem;
  border: 1px solid;
  border-radius: 0.25rem;
  font-size: 0.875rem;
}

:deep(.dropdown) {
  position: static;
}
</style>

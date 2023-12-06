<template>
  <form>
    <div class="container-fluid mt-3 workers">
      <h4 class="ms-5">Співробітники</h4>
      <div class="row overflow-x-auto">
        <div class="col-auto table-col">
          <table class="table m-0">
            <thead>
              <th v-for="header in workersTableHeaders">
                {{ header }}
              </th>
            </thead>
            <tbody>
              <tr v-for="worker in workers">
                <td>
                  <button type="button" class="btn btn-outline-danger p-1 trashbin">
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
                <td>{{ worker.id }}</td>
                <td>{{ worker.name }}</td>
                <td>{{ worker.salary }}{{ dollarSign }}</td>
                <td>{{ worker.developmentVelocity }} {{ developmentVelocityMetric }}</td>
              </tr>
              <tr>
                <td>
                  <button type="button" class="btn btn-outline-danger p-1 trashbin">
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
                <td>
                  {{ newWorker.id }}
                </td>
                <td>
                  <input type="text" class="form-control form-control-sm" v-model="newWorker.name" />
                </td>
                <td>
                  <div class="input-group">
                    <input type="number" min="0" class="form-control form-control-sm" v-model="newWorker.salary" />
                    <span class="input-group-text">{{ dollarSign }}</span>
                  </div>
                </td>
                <td>
                  <div class="input-group">
                    <input type="number" min="0" class="form-control form-control-sm"
                      v-model="newWorker.developmentVelocity" />
                    <span class="input-group-text">{{ developmentVelocityMetric }}</span>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <div class="container-fluid mt-3 tasks">
      <h4 class="ms-5">Задачі</h4>
      <div class="row overflow-x-auto">
        <div class="col-auto table-col">
          <table class="table m-0">
            <thead>
              <th v-for="header in tasksTableHeaders">
                {{ header }}
              </th>
            </thead>
            <tbody>
              <tr v-for="task in tasks" :key="task.id">
                <td>
                  <button type="button" class="btn btn-outline-danger p-1 trashbin">
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
                <td>{{ task.id }}</td>
                <td>{{ task.name }}</td>
                <td>{{ task.complexity }} {{ storyPointsSign }}</td>
                <td>
                  <div class="container-fluid">
                    <div class="row gap-1">
                      <div v-for="parentTask in task.parentTasks" :key="parentTask"
                        class="col-auto border-primary text-primary inline-item">
                        {{ tasks[parentTask].name }}
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto dropdown p-0">
                        <button class="btn btn-sm btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown"
                          :disabled="!hasAnyParentTasks(task.id)">
                          Додати задачу
                        </button>
                        <ul class="dropdown-menu">
                          <li v-for="taskToAdd in getAvailableParentTasksToAdd(task.id)" :key="taskToAdd.id">
                            <a class="dropdown-item" href="#">{{ taskToAdd.name }}</a>
                          </li>
                        </ul>
                      </div>
                    </div>
                  </div>
                </td>
                <td>
                  <div class="container-fluid">
                    <div class="row gap-1">
                      <div v-for="worker in task.availableWorkers" :key="worker.id"
                        class="col-auto border-primary text-primary inline-item">
                        {{ workers[worker].name }}
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto p-0 dropdown">
                        <button class="btn btn-sm btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown"
                          :disabled="!hasAnyAvailableWorkers(task.id)">
                          Додати співробітника
                        </button>
                        <ul class="col-auto dropdown-menu">
                          <li v-for="worker in getAvailableWorkersForTask(task.id)" :key="worker.id">
                            <a class="dropdown-item" href="#">{{ worker.name }}</a>
                          </li>
                        </ul>
                      </div>
                    </div>
                  </div>
                </td>
              </tr>
              <tr>
                <td>
                  <button type="button" class="btn btn-outline-danger p-1 trashbin">
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
                <td>
                  {{ newTask.id }}
                </td>
                <td>
                  <input type="text" class="form-control form-control-sm" v-model="newTask.name" />
                </td>
                <td>
                  <div class="input-group">
                    <input type="number" min="0" max="100" class="form-control form-control-sm"
                      v-model="newTask.complexity" />
                    <span class="input-group-text">{{ storyPointsSign }}</span>
                  </div>
                </td>
                <td>
                  <div class="container-fluid">
                    <div class="row gap-1">
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 1
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 2
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 3
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 4
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 5
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto dropdown d-inline-block p-0">
                        <button class="btn btn-sm btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                          Додати...
                        </button>
                        <ul class="dropdown-menu">
                          <li><a class="dropdown-item" href="#">Задача 2</a></li>
                          <li><a class="dropdown-item" href="#">Задача 3</a></li>
                          <li><a class="dropdown-item" href="#">Задача 4</a></li>
                        </ul>
                      </div>
                    </div>
                  </div>
                </td>
                <td>
                  <div class="container-fluid">
                    <div class="row gap-1">
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Ачілов А. В.
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Ковальчук Б. Г
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Сніжний М. Г.
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Струменець Л. Д.
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block p-0 dropdown">
                        <button class="btn btn-sm btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                          Додати...
                        </button>
                        <ul class="col-auto dropdown-menu">
                          <li><a class="dropdown-item" href="#">Ачілов А. В.</a></li>
                          <li><a class="dropdown-item" href="#">Сніжний М. Г.</a></li>
                          <li><a class="dropdown-item" href="#">Струменець Л. Д.</a></li>
                        </ul>
                      </div>
                    </div>
                  </div>
                </td>
              </tr>
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

const dollarSign = '$';
const storyPointsSign = "SP";
const developmentVelocityMetric = `${storyPointsSign}/тиж.`;

const workersTableHeaders = ["", "ID", "ПІБ", "Зарплата", "Швидкість розробки"];
const workers = ref([
  {
    id: 0,
    name: "Ачілов А. В.",
    salary: 1000,
    developmentVelocity: 15
  },
  {
    id: 1,
    name: "Хвостенко О. Д.",
    salary: 800,
    developmentVelocity: 10
  }
]);
const newWorker = ref({
  id: getNewWorkerId(),
  name: "",
  salary: null,
  developmentVelocity: null
});

function getNewWorkerId() {
  var sortedWorkers = workers.value.toSorted((worker1, worker2) => worker1.id - worker2.id);
  return sortedWorkers[sortedWorkers.length - 1].id + 1;
};

const tasksTableHeaders = ["", "ID", "Назва задачі", "Складність", "Залежить від", "Доступні співробітники для виконання"];
const tasks = ref([
  {
    id: 0,
    name: "Створити діаграму",
    complexity: 8,
    parentTasks: [],
    availableWorkers: [0, 1]
  },
  {
    id: 1,
    name: "Додати додавання спіробітників та задач",
    complexity: 15,
    parentTasks: [0],
    availableWorkers: [1]
  }
]);
const newTask = ref({
  id: getNewTaskId(),
  name: "",
  complexity: null,
  parentTasks: [],
  availableWorkers: []
});

function getNewTaskId() {
  var sortedTasks = tasks.value.toSorted((task1, task2) => task1.id - task2.id);
  return sortedTasks[sortedTasks.length - 1].id + 1;
};

function getAvailableParentTasksToAdd(taskId) {
  return tasks.value.filter((task) =>
    task.id != taskId && !tasks.value[taskId].parentTasks.includes(task.id));
}

function hasAnyParentTasks(taskId) {
  return getAvailableParentTasksToAdd(taskId).length > 0;
}

function getAvailableWorkersForTask(taskId) {
  return workers.value.filter((worker) =>
    !tasks.value[taskId].availableWorkers.includes(worker.id));
}

function hasAnyAvailableWorkers(taskId) {
  return getAvailableWorkersForTask(taskId).length > 0;
}
</script>

<style scoped lang="scss">
@import "../assets/styles.scss";

.table-col {
  width: fit-content;
}

.table {

  th {
    text-align: center;
  }

  vertical-align: middle;
  width: fit-content;

  td button.trashbin {
    border-width: 0px;
    margin: -0.25em -0.25em -0.25em -0.25em;
  }

  .btn-close {
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
  td:nth-child(1) {
    width: $td-trashbin-width;
    min-width: $td-trashbin-width;
    max-width: $td-trashbin-width;
    text-align: center;
  }

  th:nth-child(2),
  td:nth-child(2) {
    width: $td-id-width;
    min-width: $td-id-width;
    max-width: $td-id-width;
    text-align: center;
  }

  th:nth-child(3),
  td:nth-child(3) {
    width: $td-worker-name-width;
    min-width: $td-worker-name-width;
    max-width: $td-worker-name-width;
  }

  th:nth-child(4),
  td:nth-child(4) {
    width: $td-salary-width;
    min-width: $td-salary-width;
    max-width: $td-salary-width;
    text-align: center;
  }

  th:nth-child(5),
  td:nth-child(5) {
    width: $td-velocity-width;
    min-width: $td-velocity-width;
    max-width: $td-velocity-width;
    text-align: center;
  }
}

.tasks .table {

  th:nth-child(1),
  td:nth-child(1) {
    width: $td-trashbin-width;
    min-width: $td-trashbin-width;
    max-width: $td-trashbin-width;
    text-align: center;
  }

  th:nth-child(2),
  td:nth-child(2) {
    width: $td-id-width;
    min-width: $td-id-width;
    max-width: $td-id-width;
    text-align: center;
  }

  th:nth-child(3),
  td:nth-child(3) {
    width: $td-task-name-width;
    min-width: $td-task-name-width;
    max-width: $td-task-name-width;
  }

  th:nth-child(4),
  td:nth-child(4) {
    width: $td-complexity-width;
    min-width: $td-complexity-width;
    max-width: $td-complexity-width;
    text-align: center;
  }

  th:nth-child(5),
  td:nth-child(5) {
    width: $td-badges-width;
    min-width: $td-badges-width;
    max-width: $td-badges-width;
  }

  th:nth-child(6),
  td:nth-child(6) {
    width: $td-badges-width;
    min-width: $td-badges-width;
    max-width: $td-badges-width;
  }
}

.inline-item {
  padding: 0.25rem 0.5rem;
  border: 1px solid;
  border-radius: 0.25rem;
  font-size: 0.875rem;
}

.dropdown {
  position: static;
}
</style>

<template>
  <tr>
    <td>
      <button type="button" class="btn btn-outline-danger p-1 trashbin">
        <i class="bi bi-trash"></i>
      </button>
    </td>
    <td>{{ newTask.id }}</td>
    <td>
      <input type="text" class="form-control form-control-sm" v-model="newTask.name" />
    </td>
    <td>
      <div class="input-group">
        <input
          type="number"
          min="0"
          max="100"
          class="form-control form-control-sm"
          v-model="newTask.complexity"
        />
        <span class="input-group-text">{{ storyPointsSign }}</span>
      </div>
    </td>
    <td>
      <div class="container-fluid">
        <div class="row gap-1">
          <div
            v-for="parentTaskId in newTask.parentTasks"
            :key="parentTaskId"
            class="col-auto border-primary text-primary inline-item"
          >
            {{ tasks[parentTaskId].name }}
            <button type="button" class="btn-close"></button>
          </div>
          <div class="col-auto dropdown p-0">
            <button
              class="btn btn-sm btn-primary dropdown-toggle"
              type="button"
              data-bs-toggle="dropdown"
              :disabled="!hasAnyParentTasksToAdd()"
            >
              Додати задачу
            </button>
            <ul class="dropdown-menu">
              <li v-for="taskToAdd in getAvailableParentTasksToAdd()" :key="taskToAdd.id">
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
          <div
            v-for="workerId in newTask.availableWorkers"
            :key="workerId"
            class="col-auto border-primary text-primary inline-item"
          >
            {{ workers[workerId].name }}
            <button type="button" class="btn-close"></button>
          </div>
          <div class="col-auto p-0 dropdown">
            <button
              class="btn btn-sm btn-primary dropdown-toggle"
              type="button"
              data-bs-toggle="dropdown"
              :disabled="!hasAnyAvailableWorkersForTaskToAdd()"
            >
              Додати співробітника
            </button>
            <ul class="col-auto dropdown-menu">
              <li v-for="worker in getAvailableWorkersForTaskToAdd()" :key="worker.id">
                <a class="dropdown-item" href="#">{{ worker.name }}</a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </td>
  </tr>
</template>

<script setup>
import { ref, computed } from "vue";

const props = defineProps(["newTaskId", "tasks", "workers", "storyPointsSign"]);
const emit = defineEmits(["addTask", "addParentTask", "addAvailableWorker"]);

const newTask = ref({
  id: props.newTaskId,
  name: "",
  complexity: null,
  parentTasks: [],
  availableWorkers: [],
});

function getAvailableParentTasksToAdd() {
  return props.tasks.filter((task) => !newTask.value.parentTasks.includes(task.id));
}

function hasAnyParentTasksToAdd() {
  return getAvailableParentTasksToAdd().length > 0;
}

function getAvailableWorkersForTaskToAdd() {
  return props.workers.filter((worker) => !newTask.value.availableWorkers.includes(worker.id));
}

function hasAnyAvailableWorkersForTaskToAdd() {
  return getAvailableWorkersForTaskToAdd().length > 0;
}
</script>

<style scoped lang="scss"></style>

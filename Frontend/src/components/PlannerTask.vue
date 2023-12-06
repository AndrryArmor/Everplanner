<template>
  <tr>
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
          <div
            v-for="parentTaskId in task.parentTasks"
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
              :disabled="!hasAnyParentTasks(task.id)"
            >
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
          <div
            v-for="workerId in task.availableWorkers"
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
              :disabled="!hasAnyAvailableWorkers(task.id)"
            >
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
</template>

<script setup>
import { ref, computed } from "vue";

const props = defineProps(["task", "tasks", "workers", "storyPointsSign"]);
const emit = defineEmits(["deleteTask", 'addParentTask', 'addAvailableWorker']);

function getAvailableParentTasksToAdd(taskId) {
  return props.tasks.filter(
    (task) => task.id != taskId && !props.tasks[taskId].parentTasks.includes(task.id)
  );
}

function hasAnyParentTasks(taskId) {
  return getAvailableParentTasksToAdd(taskId).length > 0;
}

function getAvailableWorkersForTask(taskId) {
  return props.workers.filter(
    (worker) => !props.tasks[taskId].availableWorkers.includes(worker.id)
  );
}

function hasAnyAvailableWorkers(taskId) {
  return getAvailableWorkersForTask(taskId).length > 0;
}
</script>

<style scoped lang="scss"></style>

<template>
  <tr>
    <td>
      <button type="button" class="btn btn-outline-danger p-1 action" @click="deleteTask">
        <i class="bi bi-trash"></i>
      </button>
    </td>
    <td>{{ rowIndex }}</td>
    <td>{{ task.name }}</td>
    <td>{{ task.complexity }} {{ storyPointsSign }}</td>
    <td>
      <div class="container-fluid">
        <div class="row gap-1">
          <div
            v-for="parentTaskId in task.parentTasks"
            :key="parentTaskId"
            ref="parentTasksItems"
            class="col-auto border-primary text-primary inline-item"
          >
            {{ tasks.find((t) => t.id === parentTaskId).name }}
            <button
              type="button"
              class="btn-close"
              @click="deleteParentTask(parentTaskId)"
            ></button>
          </div>
          <div class="col-auto dropdown p-0">
            <button
              class="btn btn-sm btn-primary dropdown-toggle"
              type="button"
              data-bs-toggle="dropdown"
              :disabled="!hasAnyParentTasksToAdd"
            >
              Додати задачу
            </button>
            <ul ref="dropdownAddParentTask" class="dropdown-menu">
              <li v-for="parentTask in availableParentTasksToAdd" :key="parentTask.id">
                <a class="dropdown-item" href="#" @click="addParentTask(parentTask.id)">
                  {{ parentTask.name }}
                </a>
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
            {{ workers.find((w) => w.id === workerId).name }}
            <button
              type="button"
              class="btn-close"
              @click="deleteAvailableWorker(workerId)"
            ></button>
          </div>
          <div class="col-auto p-0 dropdown">
            <button
              class="btn btn-sm btn-primary dropdown-toggle"
              type="button"
              data-bs-toggle="dropdown"
              :disabled="!hasAnyAvailableWorkersForTaskToAdd"
            >
              Додати співробітника
            </button>
            <ul ref="dropdownAddAvailableWorker" class="col-auto dropdown-menu">
              <li v-for="worker in availableWorkersForTaskToAdd" :key="worker.id">
                <a class="dropdown-item" href="#" @click="addAvailableWorker(worker.id)">
                  {{ worker.name }}
                </a>
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

const props = defineProps({
  rowIndex: Number,
  task: {
    type: Object,
    required: true,
  },
  tasks: {
    type: Array,
    required: true,
  },
  workers: {
    type: Array,
    required: true,
  },
  storyPointsSign: {
    type: String,
    required: true,
  },
});
const emit = defineEmits([
  "delete-task",
  "add-parent-task",
  "delete-parent-task",
  "add-available-worker",
  "delete-available-worker",
]);

const dropdownAddParentTask = ref(null);
const dropdownAddAvailableWorker = ref(null);
const parentTasksItems = ref([]);

const availableParentTasksToAdd = computed(() => {
  return props.tasks.filter((t) => t.id != props.task.id && !props.task.parentTasks.includes(t.id));
});

const hasAnyParentTasksToAdd = computed(() => {
  return availableParentTasksToAdd.value.length > 0;
});

const availableWorkersForTaskToAdd = computed(() => {
  return props.workers.filter((worker) => !props.task.availableWorkers.includes(worker.id));
});

const hasAnyAvailableWorkersForTaskToAdd = computed(() => {
  return availableWorkersForTaskToAdd.value.length > 0;
});

function deleteTask() {
  emit("delete-task", props.task.id);
}

function addParentTask(parentTaskId) {
  emit("add-parent-task", props.task.id, parentTaskId);
  dropdownAddParentTask.value.classList.remove("show");
}

function deleteParentTask(parentTaskId) {
  emit("delete-parent-task", props.task.id, parentTaskId);
}

function addAvailableWorker(workerId) {
  emit("add-available-worker", props.task.id, workerId);
  dropdownAddAvailableWorker.value.classList.remove("show");
}

function deleteAvailableWorker(workerId) {
  emit("delete-available-worker", props.task.id, workerId);
}
</script>

<style scoped lang="scss"></style>

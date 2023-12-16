<template>
  <tr>
    <td>
      <button type="button" class="btn btn-outline-success p-1 action" @click="addNewTask">
        <i class="bi bi-check-square"></i>
      </button>
    </td>
    <td>{{ rowIndex }}</td>
    <td>
      <input
        type="text"
        class="form-control form-control-sm"
        v-model="newTask.name"
        placeholder="Назва задачі"
      />
    </td>
    <td>
      <div class="input-group input-group-sm">
        <input
          type="number"
          min="0"
          max="100"
          class="form-control"
          v-model="newTask.complexity"
          placeholder="0"
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
            {{ tasks.find((t) => t.id == parentTaskId).name }}
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
            v-for="workerId in newTask.availableWorkers"
            :key="workerId"
            class="col-auto border-primary text-primary inline-item"
          >
            {{ workers.find((w) => w.id == workerId).name }}
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
import { ref, computed, watchEffect } from "vue";

const props = defineProps({
  rowIndex: Number,
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
const emit = defineEmits(["add-new-task"]);

const dropdownAddParentTask = ref(null);
const dropdownAddAvailableWorker = ref(null);

var newTask = resetNewTask();
function resetNewTask() {
  return ref({
    id: 0,
    name: "",
    complexity: null,
    parentTasks: [],
    availableWorkers: [],
  });
}

const availableParentTasksToAdd = computed(() => {
  return props.tasks.filter((task) => !newTask.value.parentTasks.includes(task.id));
});

const hasAnyParentTasksToAdd = computed(() => {
  return availableParentTasksToAdd.value.length > 0;
});

const availableWorkersForTaskToAdd = computed(() => {
  return props.workers.filter((worker) => !newTask.value.availableWorkers.includes(worker.id));
});

const hasAnyAvailableWorkersForTaskToAdd = computed(() => {
  return availableWorkersForTaskToAdd.value.length > 0;
});

function addNewTask() {
  emit("add-new-task", newTask.value);
  newTask = resetNewTask();
}

function addParentTask(parentTaskId) {
  newTask.value.parentTasks = [...newTask.value.parentTasks, parentTaskId];
  dropdownAddParentTask.value.classList.remove("show");
}

function deleteParentTask(parentTaskId) {
  newTask.value.parentTasks = newTask.value.parentTasks.filter((task) => task != parentTaskId);
}

function addAvailableWorker(workerId) {
  newTask.value.availableWorkers = [...newTask.value.availableWorkers, workerId];
  dropdownAddAvailableWorker.value.classList.remove("show");
}

function deleteAvailableWorker(workerId) {
  newTask.value.availableWorkers = newTask.value.availableWorkers.filter(
    (worker) => worker != workerId
  );
}
</script>

<style scoped lang="scss"></style>

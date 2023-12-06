<template>
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
            <button
              class="btn btn-sm btn-primary dropdown-toggle"
              type="button"
              data-bs-toggle="dropdown"
            >
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
            <button
              class="btn btn-sm btn-primary dropdown-toggle"
              type="button"
              data-bs-toggle="dropdown"
            >
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
</template>

<script setup>
import { ref, computed } from "vue";

const props = defineProps(["newTaskId", "tasks", "workers", "storyPointsSign"]);
const emit = defineEmits(["addTask", 'addParentTask', 'addAvailableWorker']);

const newTask = ref({
  id: props.newTaskId,
  name: "",
  complexity: null,
  parentTasks: [],
  availableWorkers: [],
});
</script>

<style scoped lang="scss"></style>

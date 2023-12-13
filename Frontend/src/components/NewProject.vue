<template>
  <tr @mouseover="removeHover" @mouseout="restoreHover">
    <td>
      <button type="button" class="btn btn-outline-success p-1 action" @click="addNewProject">
        <i class="bi bi-check-square"></i>
      </button>
    </td>
    <td>{{ rowIndex }}</td>
    <td>
      <input
        type="text"
        class="form-control form-control-sm"
        v-model="newProject.name"
        placeholder="Назва"
      />
    </td>
    <td>{{ newProject.workersCount }}</td>
    <td>{{ newProject.tasksCount }}</td>
  </tr>
</template>

<script setup>
import { ref, watchEffect } from "vue";

const props = defineProps({
  rowIndex: Number,
  newProjectId: {
    type: Number,
    required: true,
  },
});
const emit = defineEmits(["add-new-worker", "remove-hover", "restore-hover"]);

var newProject = resetNewProject();
function resetNewProject() {
  return ref({
    id: props.newProjectId,
    name: "",
    workersCount: 0,
    tasksCount: 0,
  });
}
watchEffect(() => (newProject.value.id = props.newProjectId));

function addNewProject() {
  emit("add-new-worker", newProject.value);
  newProject = resetNewProject();
}

function removeHover() {
  emit("remove-hover");
}

function restoreHover() {
  emit("restore-hover");
}
</script>

<style scoped lang="scss"></style>

<template>
  <tr @mouseover="removeHover" @mouseout="restoreHover">
    <td>
      <button type="submit" class="btn btn-outline-success p-1 action" @click="addNewProject">
        <i class="bi bi-check-square"></i>
      </button>
    </td>
    <td>{{ rowIndex }}</td>
    <td>
      <input
        type="text"
        class="form-control form-control-sm"
        v-model="newProjectName"
        placeholder="Назва"
      />
    </td>
    <td>0</td>
    <td>0</td>
  </tr>
</template>

<script setup>
import { ref } from "vue";

const props = defineProps({
  rowIndex: Number,
});
const emit = defineEmits(["add-new-project", "remove-hover", "restore-hover"]);

var newProjectName = ref(null);

function addNewProject() {
  emit("add-new-project", {
    id: 0,
    name: newProjectName.value,
    tasks: [],
    workers: [],
  });
  newProjectName.value = "";
}

function removeHover() {
  emit("remove-hover");
}

function restoreHover() {
  emit("restore-hover");
}
</script>

<style scoped lang="scss"></style>

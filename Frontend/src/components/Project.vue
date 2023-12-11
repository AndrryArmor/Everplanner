<template>
  <tr @click="openProject">
    <td>
      <button type="button" class="btn btn-outline-danger p-1 action" @click="deleteWorker">
        <i class="bi bi-trash"></i>
      </button>
    </td>
    <td>{{ project.id }}</td>
    <td>{{ project.name }}</td>
    <td>{{ project.workersCount }}</td>
    <td>{{ project.tasksCount }}</td>
  </tr>
</template>

<script setup>
const props = defineProps({
  project: {
    type: Object,
    required: true,
  },
});
const emit = defineEmits(["open-project", "delete-project"]);

function openProject() {
  emit("open-project", props.project.id);
}

function deleteWorker() {
  emit("delete-project", props.project.id);
}
</script>

<style scoped lang="scss"></style>

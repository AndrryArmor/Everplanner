<template>
  <tr @click="openProject">
    <td>
      <button type="button" class="btn btn-outline-danger p-1 action" @click.stop="deleteWorker">
        <i class="bi bi-trash"></i>
      </button>
    </td>
    <td>{{ rowIndex }}</td>
    <td>{{ project.name }}</td>
    <td>{{ project.workers }}</td>
    <td>{{ project.tasks }}</td>
  </tr>
</template>

<script setup>
const props = defineProps({
  rowIndex: Number,
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

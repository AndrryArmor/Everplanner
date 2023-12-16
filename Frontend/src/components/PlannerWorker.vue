<template>
  <tr>
    <td>
      <button type="button" class="btn btn-outline-danger p-1 action" @click="deleteWorker">
        <i class="bi bi-trash"></i>
      </button>
    </td>
    <td>{{ rowIndex }}</td>
    <td>{{ worker.name }}</td>
    <td>{{ worker.salary }}{{ dollarSign }}</td>
    <td>{{ worker.developmentVelocity }} {{ developmentVelocityMetric }}</td>
  </tr>
</template>

<script setup>
const props = defineProps({
  rowIndex: Number,
  worker: {
    type: Object,
    required: true,
  },
  dollarSign: {
    type: String,
    required: true,
  },
  developmentVelocityMetric: {
    type: String,
    required: true,
  },
});
const emit = defineEmits(["delete-worker"]);

function deleteWorker() {
  emit("delete-worker", props.worker.id);
}
</script>

<style scoped lang="scss"></style>

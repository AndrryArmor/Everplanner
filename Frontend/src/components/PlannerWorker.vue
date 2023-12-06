<template>
  <tr>
    <td>
      <button type="button" class="btn btn-outline-danger p-1 trashbin">
        <i class="bi bi-trash"></i>
      </button>
    </td>
    <td>{{ worker.id }}</td>
    <td>{{ worker.name }}</td>
    <td>{{ worker.salary }}{{ dollarSign }}</td>
    <td>{{ worker.developmentVelocity }} {{ developmentVelocityMetric }}</td>
  </tr>
</template>

<script setup>
import { ref, computed } from "vue";

const props = defineProps(["worker", "dollarSign", "developmentVelocityMetric"]);
const emit = defineEmits(["deleteWorker"]);
</script>

<style scoped lang="scss"></style>

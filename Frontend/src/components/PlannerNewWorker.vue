<template>
  <tr>
    <td>
      <button type="button" class="btn btn-outline-danger p-1 trashbin">
        <i class="bi bi-trash"></i>
      </button>
    </td>
    <td>
      {{ newWorker.id }}
    </td>
    <td>
      <input type="text" class="form-control form-control-sm" v-model="newWorker.name" />
    </td>
    <td>
      <div class="input-group">
        <input
          type="number"
          min="0"
          class="form-control form-control-sm"
          v-model="newWorker.salary"
        />
        <span class="input-group-text">{{ dollarSign }}</span>
      </div>
    </td>
    <td>
      <div class="input-group">
        <input
          type="number"
          min="0"
          max="1000"
          class="form-control form-control-sm"
          v-model="newWorker.developmentVelocity"
        />
        <span class="input-group-text">{{ developmentVelocityMetric }}</span>
      </div>
    </td>
  </tr>
</template>

<script setup>
import { ref, computed } from "vue";

const props = defineProps(["newWorkerId", "dollarSign", "developmentVelocityMetric"]);
const emit = defineEmits(["addWorker"]);

const newWorker = ref({
  id: props.newWorkerId,
  name: "",
  salary: null,
  developmentVelocity: null,
});
</script>

<style scoped lang="scss"></style>

<template>
  <tr>
    <td>
      <button type="button" class="btn btn-outline-success p-1 action" @click="addNewWorker">
        <i class="bi bi-check-square"></i>
      </button>
    </td>
    <td>{{ newWorker.id }}</td>
    <td>
      <input
        type="text"
        class="form-control form-control-sm"
        v-model="newWorker.name"
        placeholder="ПІБ"
      />
    </td>
    <td>
      <div class="input-group input-group-sm">
        <input
          type="number"
          min="0"
          class="form-control"
          v-model="newWorker.salary"
          placeholder="0"
        />
        <span class="input-group-text">{{ dollarSign }}</span>
      </div>
    </td>
    <td>
      <div class="input-group input-group-sm">
        <input
          type="number"
          min="0"
          max="1000"
          class="form-control"
          v-model="newWorker.developmentVelocity"
          placeholder="0"
        />
        <span class="input-group-text">{{ developmentVelocityMetric }}</span>
      </div>
    </td>
  </tr>
</template>

<script setup>
import { ref, watchEffect } from "vue";

const props = defineProps({
  newWorkerId: {
    type: Number,
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
const emit = defineEmits(["add-new-worker"]);

var newWorker = resetNewWorker();
function resetNewWorker() {
  return ref({
    id: props.newWorkerId,
    name: "",
    salary: null,
    developmentVelocity: null,
  });
}
watchEffect(() => (newWorker.value.id = props.newWorkerId));

function addNewWorker() {
  emit("add-new-worker", newWorker.value);
  newWorker = resetNewWorker();
}
</script>

<style scoped lang="scss"></style>

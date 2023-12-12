<template>
  <form class="container-fluid mt-3 projects">
    <h4 class="ms-5">Проєкти користувача Ачілов Андрій</h4>
    <div class="row overflow-x-auto">
      <div class="col-auto">
        <table class="table table-hover m-0">
          <thead>
            <tr>
              <th v-for="header in projectsTableHeaders" class="text-center align-middle">
                {{ header }}
              </th>
            </tr>
          </thead>
          <tbody>
            <Project
              v-for="project in projects"
              :key="project.id"
              :project="project"
              @open-project="openProject"
              @delete-project="deleteProject"
            />
            <NewProject :new-project-id="newProjectId" @add-new-worker="addNewProject" />
          </tbody>
        </table>
      </div>
    </div>
  </form>
</template>

<script setup>
import { ref, computed } from "vue";
import Project from "./Project.vue";

const emit = defineEmits("show-project");

const projectsTableHeaders = ["", "ID", "Назва", "Кількість працівників", "Кількість задач"];
const projects = ref([
  {
    id: 0,
    name: "Проєкт 1",
    workersCount: 4,
    tasksCount: 10,
  },
]);

const newProjectId = computed(() => {
  if (projects.value.length === 0) {
    return 0;
  }

  const sortedProjects = [...projects.value].sort(
    (project1, project2) => project1.id - project2.id
  );
  return sortedProjects[sortedProjects.length - 1].id + 1;
});

function openProject(projectId) {
  emit("show-project", projects.value.filter((project) => project.id === projectId)[0]);
}

function addNewProject(project) {
  projects.value = [...projects.value, project];
}

function deleteProject(projectId) {
  projects.value = projects.value.filter((project) => project.id != projectId);
}
</script>

<style scoped lang="scss">
@import "@/assets/_variables.scss";

$td-project-name-width: 300px;
$td-workers-count-width: 150px;
$td-tasks-count-width: 300px;

.projects .table {
  th:nth-child(1),
  :deep(td:nth-child(1)) {
    width: $td-action-width;
    min-width: $td-action-width;
    max-width: $td-action-width;
    text-align: center;
  }

  th:nth-child(2),
  :deep(td:nth-child(2)) {
    width: $td-id-width;
    min-width: $td-id-width;
    max-width: $td-id-width;
    text-align: center;
  }

  th:nth-child(3),
  :deep(td:nth-child(3)) {
    width: $td-project-name-width;
    min-width: $td-project-name-width;
    max-width: $td-project-name-width;
  }

  th:nth-child(4),
  :deep(td:nth-child(4)) {
    width: $td-workers-count-width;
    min-width: $td-workers-count-width;
    max-width: $td-workers-count-width;
    text-align: center;
  }

  th:nth-child(5),
  :deep(td:nth-child(5)) {
    width: $td-tasks-count-width;
    min-width: $td-tasks-count-width;
    max-width: $td-tasks-count-width;
    text-align: center;
  }
}
</style>

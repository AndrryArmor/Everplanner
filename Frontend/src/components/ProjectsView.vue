<template>
  <div
    v-if="isLoading"
    class="spinner-border text-primary position-absolute top-50 start-50"
    style="width: 3rem; height: 3rem"
  ></div>
  <form v-else class="container-fluid mt-3 projects">
    <h4 class="ms-5">Проєкти користувача {{ user.name }}</h4>
    <div class="row overflow-x-auto">
      <div class="col-auto">
        <table ref="projectsTable" class="table table-hover align-middle m-0">
          <thead class="text-center">
            <tr>
              <th v-for="header in projectTableHeaders">
                {{ header }}
              </th>
            </tr>
          </thead>
          <tbody>
            <ProjectPreview
              v-for="(project, index) in user.projects"
              :key="project.id"
              :row-index="index + 1"
              :project="project"
              @open-project="openProject"
              @delete-project="deleteProject"
            />
            <ProjectAddForm
              :row-index="user.projects.length + 1"
              @add-new-project="addNewProject"
              @remove-hover="removeHover"
              @restore-hover="restoreHover"
            />
          </tbody>
        </table>
      </div>
    </div>
  </form>
</template>

<script setup>
import { ref, onMounted } from "vue";
import ProjectPreview from "./ProjectPreview.vue";
import ProjectAddForm from "./ProjectAddForm.vue";
import { useRoute } from "vue-router";
import router from "../router";

const emit = defineEmits("show-project");

const isLoading = ref(true);
const route = useRoute();
const projectsTable = ref(null);
const projectTableHeaders = ["", "№", "Назва", "Кількість працівників", "Кількість задач"];

const user = ref(null);

function openProject(projectId) {
  router.push(`/users/${route.params.userId}/projects/${projectId}`);
}

async function addNewProject(project) {
  try {
    const projectId = await fetch(
      `https://localhost:7229/api/users/${route.params.userId}/projects`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(project),
      }
    ).then(async (response) => {
      const res = await response.text();
      if (response.status == 400) {
        throw new Error("Заповніть ім'я проєкту.");
      } else if (!response.ok) {
        throw new Error(res);
      }
      return parseInt(res);
    });

    const previewProject = {
      id: projectId,
      name: project.name,
      workers: project.workers.length,
      tasks: project.tasks.length,
    };
    user.value.projects = [...user.value.projects, previewProject];
  } catch (error) {
    alert(error.message);
  }
}

async function deleteProject(projectId) {
  try {
    await fetch(`https://localhost:7229/api/users/${route.params.userId}/projects/${projectId}`, {
      method: "DELETE",
    }).then(async (response) => {
      if (!response.ok) {
        throw new Error(await response.text());
      }
    });

    user.value.projects = user.value.projects.filter((project) => project.id != projectId);
  } catch (error) {
    alert(error.message);
  }
}

function removeHover() {
  projectsTable.value.classList.remove("table-hover");
}

function restoreHover() {
  projectsTable.value.classList.add("table-hover");
}

onMounted(async () => {
  try {
    user.value = await fetch(`https://localhost:7229/api/users/${route.params.userId}`).then(
      async (response) => {
        const res = await response.text();
        if (!response.ok) {
          throw new Error(res);
        }
        return JSON.parse(res);
      }
    );
  } catch (error) {
    alert(error.message);
  }
  isLoading.value = false;
});
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

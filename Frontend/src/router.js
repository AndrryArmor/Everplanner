import { createRouter, createWebHistory } from "vue-router";
import SignupView from "./components/SignupView.vue";
import LoginView from "./components/LoginView.vue";
import ProjectsView from "./components/ProjectsView.vue";
import ProjectView from "./components/ProjectView.vue";
import GanttChartView from "./components/GanttChartView.vue";

const routes = [
  { path: "/", redirect: "/login" },
  { path: "/signup", component: SignupView },
  { path: "/login", component: LoginView },
  { path: "/users/:userId/projects", component: ProjectsView },
  { path: "/users/:userId/projects/:projectId", component: ProjectView },
  { path: "/users/:userId/projects/:projectId/plan", component: GanttChartView },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

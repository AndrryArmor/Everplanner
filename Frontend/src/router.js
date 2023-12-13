import { createRouter, createWebHistory } from "vue-router";
import TheLogin from "./components/TheLogin.vue";
import TheSignup from "./components/TheSignup.vue";
import Projects from "./components/Projects.vue";
import Planner from "./components/Planner.vue";
import GanttChart from "./components/GanttChart.vue";

const routes = [
  { path: "/login", component: TheLogin },
  { path: "/signup", component: TheSignup },
  { path: "/users/:userId/projects", component: Projects },
  { path: "/users/:userId/projects/:projectId", component: Planner },
  { path: "/users/:userId/projects/:projectId/planning-results", component: GanttChart },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

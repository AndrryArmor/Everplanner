import { createRouter, createWebHistory } from "vue-router";
import TheLogin from "@/components/TheLogin.vue";
import Projects from "@/components/Projects.vue";
import Planner from "@/components/Planner.vue";
import GanttChart from "@/components/GanttChart.vue";

const routes = [
  { path: "/", component: TheLogin },
  { path: "/projects", component: Projects },
  { path: "/projects/project", component: Planner },
  { path: "/projects/project/planning-results", component: GanttChart },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

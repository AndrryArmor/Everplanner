import { createApp } from "vue";
import App from "@/App.vue";
import router from "@/router.js";

import "@/assets/styles.scss";
import * as bootstrap from "bootstrap";

createApp(App).use(router).mount("#app");

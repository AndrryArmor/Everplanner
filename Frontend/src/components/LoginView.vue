<template>
  <div class="container my-3">
    <div class="row justify-content-center">
      <div class="col-sm-auto" style="width: 550px">
        <form
          class="row row-cols-1 border rounded rounded-5 needs-validation"
          novalidate
          v-on:submit.prevent="login($event)"
        >
          <h1 class="fw-light my-4 text-center">Вхід в Everplanner</h1>
          <div class="col mb-4">
            <label for="inputEmail" class="form-label fs-5">Електронна пошта</label>
            <input
              v-model="email"
              type="email"
              class="form-control form-control-lg"
              id="inputEmail"
              placeholder="example@email.com"
              required
            />
            <div class="invalid-feedback">
              Будь ласка, введіть електронну пошту в форматі <i>example@email.com</i>.
            </div>
          </div>
          <div class="col mb-4">
            <label for="inputPassword" class="form-label fs-5">Пароль</label>
            <input
              v-model="password"
              type="password"
              class="form-control form-control-lg"
              id="inputPassword"
              required
            />
            <div class="invalid-feedback">Будь ласка, введіть пароль.</div>
          </div>
          <div class="col mb-3">
            <button type="submit" class="btn btn-lg btn-success w-100">Ввійти</button>
          </div>
          <div class="col mb-4 text-center">
            <span>
              Ви тут вперше? Тоді
              <router-link to="/signup" href="#">зареєструйтесь</router-link>
            </span>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import router from "../router";

const route = useRoute();

const email = ref("");
const password = ref("");

async function login(event) {
  const form = event.target;
  form.classList.add("was-validated");
  if (form.checkValidity()) {
    try {
      const userId = await fetch("https://localhost:7229/api/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ email: email.value, password: password.value }),
      }).then(async (response) => {
        const res = await response.text();
        if (!response.ok) {
          throw new Error(res);
        }
        return parseInt(res);
      });
      router.push(`/users/${userId}/projects`);
    } catch (error) {
      alert(error.message);
    }
  }
}

onMounted(() => {
  if (route.query.afterSignup == "true") {
    alert("Реєстрація пройшла успішно! Тепер ввійдіть у ваш акаунт.");
  }
});
</script>

<style scoped lang="scss"></style>

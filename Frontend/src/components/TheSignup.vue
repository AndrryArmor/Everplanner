<template>
  <div class="container my-3">
    <div class="row justify-content-center">
      <div class="col-sm-auto" style="width: 550px">
        <form
          class="row row-cols-1 border rounded rounded-5 needs-validation"
          novalidate
          v-on:submit.prevent="signup($event)"
        >
          <h2 class="fw-light my-4 text-center">Реєстрація в Everplanner</h2>
          <div class="col mb-3">
            <label for="inputName" class="form-label">Ім'я</label>
            <input v-model="name" type="text" class="form-control" id="inputName" required />
            <div class="invalid-feedback">Будь ласка, введіть ім'я.</div>
          </div>
          <div class="сol mb-3">
            <label for="inputEmail" class="form-label">Електронна пошта</label>
            <input v-model="email" type="email" class="form-control" id="inputEmail" required />
            <div class="invalid-feedback">Будь ласка, введіть електронну пошту.</div>
          </div>
          <div class="col mb-3">
            <label for="inputPassword" class="form-label">Пароль</label>
            <input
              ref="passwordInput"
              v-model="password"
              type="password"
              class="form-control"
              id="inputPassword"
              @input="checkPasswordsMatch"
              required
            />
            <div class="invalid-feedback">Будь ласка, введіть пароль.</div>
          </div>
          <div class="col mb-3">
            <label for="inputConfirmPassword" class="form-label">Підтвердіть пароль</label>
            <input
              ref="confirmPasswordInput"
              v-model="confirmPassword"
              type="password"
              class="form-control"
              id="inputConfirmPassword"
              @input="checkPasswordsMatch"
              required
            />
            <div class="invalid-feedback">Паролі не співпадають.</div>
          </div>
          <div class="col mb-2">
            <button type="submit" class="btn btn-success w-100">Зареєструватись</button>
          </div>
          <div class="col mb-3 text-center">
            <span
              >Уже маєте акаунт? Тоді <router-link to="/login" href="#">ввійдіть</router-link></span
            >
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
import router from "../router";

const name = ref("");
const email = ref("");
const password = ref("");
const confirmPassword = ref("");

const isTouched = ref(false);
const passwordInput = ref(null);
const confirmPasswordInput = ref(null);

const isPasswordsMatch = computed(() => password.value === confirmPassword.value);

function checkPasswordsMatch() {
  if (!isTouched.value) {
    return;
  }

  if (!isPasswordsMatch.value) {
    confirmPasswordInput.value.setCustomValidity("No match");
  } else {
    confirmPasswordInput.value.setCustomValidity("");
  }
}

function signup(event) {
  const form = event.target;
  form.classList.add("was-validated");

  const isValid = form.checkValidity();
  isTouched.value = true;
  checkPasswordsMatch();
  if (isValid && isPasswordsMatch.value) {
    router.push({ path: "/login", query: { afterSignup: true } });
  }
}
</script>

<style scoped lang="scss"></style>

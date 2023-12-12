<template>
  <div class="container my-3">
    <div class="row justify-content-center">
      <div class="col-sm-10 login-wrapper">
        <form class="container border rounded rounded-5 login">
          <h2 class="fw-light my-4 text-center">Реєстрація в Everplanner</h2>
          <div class="row mb-3 mx-1">
            <label for="inputName" class="col-12 col-form-label">Ім'я</label>
            <div class="col-12">
              <input type="text" class="form-control" id="inputName" />
            </div>
          </div>
          <div class="row mb-3 mx-1">
            <label for="inputEmail" class="col-12 col-form-label">Email</label>
            <div class="col-12">
              <input type="email" class="form-control" id="inputEmail" />
            </div>
          </div>
          <div class="row mb-3 mx-1">
            <label for="inputPassword" class="col-12 col-form-label">Пароль</label>
            <div class="col-12">
              <input type="password" class="form-control" id="inputPassword" />
            </div>
          </div>
          <div class="row mb-3 mx-1">
            <label for="inputConfirmPassword" class="col-12 col-form-label">Підтвердіть пароль</label>
            <div class="col-12">
              <input type="password" class="form-control" id="inputConfirmPassword" />
            </div>
          </div>
          <div class="row mb-2 mx-1">
            <div class="col">
              <button type="submit" class="btn btn-success w-100">Зареєструватись</button>
            </div>
          </div>
          <div class="row mb-3 mx-1">
            <div class="col text-center">
              <span>Уже маєте акаунт? Тоді <router-link to="/login" href="#">ввійдіть</router-link></span>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup></script>

<style scoped lang="scss">
.login-wrapper {
  width: 550px;
}
</style>

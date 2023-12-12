<template>
  <div class="container my-3">
    <div class="row justify-content-center">
      <div class="col-sm-10 login-wrapper">
        <form class="container border rounded rounded-5 login">
          <h1 class="fw-light my-4 text-center">Вхід в Everplanner</h1>
          <div class="row mb-4 mx-1">
            <label for="inputEmail" class="col-12 col-form-label fs-5">Email</label>
            <div class="col-12">
              <input type="email" class="form-control form-control-lg" id="inputEmail" />
            </div>
          </div>
          <div class="row mb-4 mx-1">
            <label for="inputPassword" class="col-12 col-form-label fs-5">Пароль</label>
            <div class="col-12">
              <input type="password" class="form-control form-control-lg" id="inputPassword" />
            </div>
          </div>
          <div class="row mb-3 mx-1">
            <div class="col">
              <button type="submit" class="btn btn-lg btn-success w-100">Ввійти</button>
            </div>
          </div>
          <div class="row mb-4 mx-1">
            <div class="col text-center">
              <span>Ви тут вперше? Тоді <router-link to="/signup" href="#">зареєструйтесь</router-link></span>
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

<template>
  <nav class="navbar shadow bg-gradient" style="background-color: #fcca7a">
    <div class="container-fluid">
      <router-link to="/" class="navbar-brand display-6 text-success fw-semibold">
        Everplanner
      </router-link>
      <ul class="navbar-nav me-auto">
        <li class="nav-item">
          <router-link to="/projects" class="nav-link active">Головна</router-link>
        </li>
      </ul>
      <ul class="navbar-nav">
        <li class="nav-item">
          <router-link to="/" class="btn btn-outline-danger" role="button">
            Вийти
            <i class="bi bi-box-arrow-right"></i>
          </router-link>
        </li>
      </ul>
    </div>
  </nav>
</template>

<script setup></script>

<style scoped lang="scss"></style>

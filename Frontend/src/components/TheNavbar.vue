<template>
  <nav class="navbar shadow bg-gradient bg-light">
    <div class="container-fluid">
      <router-link to="/login" class="navbar-brand text-success fw-light">
        Everplanner
      </router-link>
      <ul class="navbar-nav me-auto">
        <li class="nav-item">
          <router-link to="/projects" class="nav-link active">Головна</router-link>
        </li>
      </ul>
      <ul class="navbar-nav">
        <li class="nav-item">
          <router-link to="/login" class="btn btn-outline-danger" role="button">
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

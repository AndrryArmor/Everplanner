<template>
  <nav class="navbar navbar-expand-sm shadow bg-gradient bg-light">
    <div class="container-fluid">
      <router-link :to="getHomePage()" class="navbar-brand text-success fw-light">
        Everplanner
      </router-link>
      <button
        class="navbar-toggler"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#navbarSupportedContent"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <div v-show="route.params.userId" class="navbar-nav me-auto">
          <router-link to="/users/0/projects" class="nav-link">Проєкти</router-link>
        </div>
        <div v-show="route.params.userId" class="navbar-nav">
          <span class="nav-link active user-select-none">Ачілов Андрій</span>
          <router-link to="/login" class="btn btn-outline-danger" role="button">
            Вийти
            <i class="bi bi-box-arrow-right"></i>
          </router-link>
        </div>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { useRoute } from "vue-router";

const route = useRoute();

function getHomePage() {
  return route.params.userId ? `/users/${route.params.userId}/projects` : "/login";
}
</script>

<style scoped lang="scss"></style>

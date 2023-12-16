<template>
  <nav class="navbar navbar-expand-sm shadow bg-gradient bg-light">
    <div class="container-fluid">
      <router-link :to="homePath" class="navbar-brand text-success fw-light">
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
          <router-link :to="`/users/${route.params.userId}/projects`" class="nav-link"
            >Проєкти</router-link
          >
        </div>
        <div v-show="route.params.userId" class="navbar-nav">
          <span class="nav-link active user-select-none">{{ userName }}</span>
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
import { ref, computed, watch } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();

const userName = ref("");
const homePath = computed(() => {
  return route.params.userId ? `/users/${route.params.userId}/projects` : "/login";
});

watch(route, async () => {
  if (!route.params.userId) {
    userName.value = "";
  }

  try {
    userName.value = await fetch(
      `https://localhost:7229/api/users/${route.params.userId}/name`
    ).then(async (response) => {
      const res = await response.text();
      if (!response.ok) {
        throw new Error(res);
      }
      return res;
    });
  } catch (error) {
    console.error(error.message);
  }
});
</script>

<style scoped lang="scss"></style>

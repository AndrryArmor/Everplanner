<template>
  <TheNavbar />
  <router-view></router-view>
</template>

<script setup>
import TheNavbar from "./components/TheNavbar.vue";
</script>

<style lang="scss">
@import "@/assets/variables.scss";
.table {
  td {
    padding-top: 0.25em;
    padding-bottom: 0.25em;

    button.action {
      border-width: 0px;
      margin: -0.25em -0.25em -0.25em -0.25em;
    }
  }

  .btn-close {
    font-size: 0.75em;
  }

  .inline-item {
    padding: 0.25rem 0.5rem;
    border: 1px solid;
    border-radius: 0.25rem;
    font-size: 0.875rem;
  }

  .dropdown {
    position: static;

    .dropdown-menu {
      height: auto;
      max-height: 200px;
      overflow-x: hidden;
    }
  }
}
</style>

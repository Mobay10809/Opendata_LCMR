<template>
  <img alt="Vue logo" src="./assets/logo.png">
  <RevenuePage msg="Welcome to Your Vue.js App" />
</template>

<script>
import RevenuePage from './components/RevenuePage.vue'

export default {
  name: 'App',
  components: {
    RevenuePage
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>

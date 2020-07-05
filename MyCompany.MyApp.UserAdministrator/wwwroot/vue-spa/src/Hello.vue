<template>
  <div>Hello!</div>
</template>

<script>
  import axios from 'axios'
  export default {
    name: 'hello',
    data() {
      return {
        user: null
      }
    },
    methods: {
      authGuard(next) {
        if (store.getters.isAuthenticated) {
           next();
          return;
        }
        next('/login');
      }
    },
    created() {
      this.authGuard();
    }

  }
</script>

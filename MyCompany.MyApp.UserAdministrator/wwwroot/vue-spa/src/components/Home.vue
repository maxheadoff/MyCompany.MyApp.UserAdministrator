<template>
  <div>
    <h1 class="__company_name">MyCompany MyApp user administrator</h1>
    <h2>Hello!</h2>
    <p v-show="!AuthState">Please, login</p>
    <p v-show="AuthState">Welcome,{{username}}!</p>
  </div>
</template>

<script>
  export default {
    name: 'hello',
    data() {
      return {
        username: null
      }
    },
    created() {
      this.username = $cookies.get('user-login');
      this.AuthState = $cookies.get('isAuthenticated') == 'true';
    }
  }
</script>
<style>
  .__company_name{

  }

</style>

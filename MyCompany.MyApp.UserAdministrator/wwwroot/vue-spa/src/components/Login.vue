<template>
  <div>
    <form class="login" @submit.prevent="login" v-bind:style="[formStyle]" >
      <h1>Sign in</h1>
      <label>User name</label>
      <input required v-model="username" type="text" placeholder="Snoopy" />
      <label>Password</label>
      <input required v-model="password" type="password" placeholder="Password" />
      <hr />
      <button type="submit">Login</button>
    </form>
    <input type="button" v-on:click="logout"  v-bind:style="[outStyle]" value="logout" />
  </div>
</template>

<script>
  import axios from 'axios'
  import router from '../router'
  export default {
    name: 'Login',
   
    data() {
      return {
        isAuthenticated: 'false',
        username: null,
        password: null,
        endpoint: 'https://localhost:44378/api/token'
      }
    },
    computed: {
      formStyle: function () {
        if (this.isAuthenticated == 'true')
          return { 'visibility': 'hidden' }
        else
          return { 'visibility': 'visible' }
      },
      outStyle: function () {
        if (this.isAuthenticated == 'true')
          return { 'visibility': 'visible' }
        else
          return { 'visibility': 'hidden' }
      }
    },
    methods: {
      login: function () {
        const { username, password } = this;
        const loginData = 'grant_type=password&username=' + username + '&password=' + password;
        axios.post(this.endpoint, loginData, {
          headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
        })
          .then(resp => {
            const token = resp.data.access_token;
            const user = resp.data.username;
            $cookies.set('user-token', token); // store the token in localstorage
            $cookies.set('user-login', user);
            $cookies.set('isAuthenticated', true);
            console.log('user:' + user + ' logged');
          })
          .catch(err => {
            console.log(err);
            $cookies.remove('user-token'); // if the request fails, remove any possible user token if possible
            $cookies.remove('user-login');
            $cookies.set('isAuthenticated', false);
          })
        this.isAuthenticated = $cookies.get('isAuthenticated');
        this.$forceUpdate();
        router.push('/');
      },
      logout: function () {
        console.log('logout');
        $cookies.remove('user-token'); // if the request fails, remove any possible user token if possible
        $cookies.remove('user-login');
        $cookies.set('isAuthenticated', false);
        this.isAuthenticated = $cookies.get('isAuthenticated');
        this.$forceUpdate();
      }
    },
    created() {
      this.isAuthenticated = $cookies.get('isAuthenticated');
    }
  }
</script>


<template>
  <div>
    <form  @submit.prevent="login" v-bind:class="{active: !AuthState ,hidden:AuthState }"  >
      <h1>Sign in</h1>
      <label>User name</label>
      <input required v-model="username" type="text" placeholder="Snoopy" />
      <label>Password</label>
      <input required v-model="password" type="password" placeholder="Password" />
      <hr />
      <button type="submit">Login</button>
    </form>
    <div v-bind:class="{active: AuthState ,hidden:!AuthState }">
      <p>Hello,{{username}}</p>
      <input type="button" v-on:click="logout"  value="logout" />
    </div>
  </div>
</template>

<script>
  import axios from 'axios'
  import router from '../router'
  export default {
    name: 'Login',
   
    data() {
      return {
        AuthState: false,
        username: null,
        password: null,
        endpoint: 'https://localhost:44378/api/token'
      }
    },
    computed: {
      isAuthenticated: function () {
        if (this.username == null)
          console.log('no user');
        return $cookies.get('isAuthenticated') == 'true';
      },
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
            this.AuthState = $cookies.get('isAuthenticated') == 'true';
          })
          .catch(err => {
            console.log(err);
            $cookies.remove('user-token'); // if the request fails, remove any possible user token if possible
            $cookies.remove('user-login');
            $cookies.set('isAuthenticated', false);
            this.AuthState = $cookies.get('isAuthenticated') == 'true';
          })
        
        //this.$forceUpdate();
      },
      logout: function () {
        console.log('logout');
        $cookies.remove('user-token'); // if the request fails, remove any possible user token if possible
        $cookies.remove('user-login');
        $cookies.set('isAuthenticated', false);
        this.AuthState = $cookies.get('isAuthenticated') == 'true';
        this.$forceUpdate();
      }
    },
    created() {
      this.AuthState = $cookies.get('isAuthenticated')=='true';
    }
  }
</script>
<style>
  .active {
    visibility: visible;
    color: red;
  }

  .hidden {
    visibility: hidden;
  }
  .login{
    color:green;
  }
</style>

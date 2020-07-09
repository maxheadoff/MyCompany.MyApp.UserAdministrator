
<template>
  <div>
    <div class="error-message">
      <p v-show="!success">Error receiving data from server,please, try later. Message:{{error_message}} <input type="button" value="x" v-on:click="clean" /> </p>
    </div>
    <form @submit.prevent="login" v-bind:class="{active: !AuthState ,hidden:AuthState }">
      <h1>Sign in</h1>
      <label>User name</label>
      <input required v-model="username" type="text" placeholder="Snoopy" />
      <label>Password</label>
      <input required v-model="password" type="password" placeholder="Password" />
      <hr />
      <button type="submit" class="login_button" id="login_button"></button>
    </form>
    <div v-bind:class="{active: AuthState ,hidden:!AuthState }">
      <p>Hello,{{username}}</p>
      <input type="button" v-on:click="logout" value=""  class="login_button" id="logout_button"/>
    </div>
  </div>
</template>

<script>
  import axios from 'axios'
  export default {
    name: 'Login',
   
    data() {
      return {
        AuthState: false,
        username: null,
        password: null,
        endpoint: 'https://localhost:44378/api/token',
        success: true,
        error_message: null
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
            this.username = resp.data.username;
            $cookies.set('user-token', token); // store the token in localstorage
            $cookies.set('user-login', this.username );
            $cookies.set('isAuthenticated', true);
            console.log('user:' + this.username + ' logged');
            this.AuthState = $cookies.get('isAuthenticated') == 'true';
          })
          .catch(err => {
            console.log(err);
            $cookies.remove('user-token'); // if the request fails, remove any possible user token if possible
            $cookies.remove('user-login');
            $cookies.set('isAuthenticated', false);
            this.AuthState = $cookies.get('isAuthenticated') == 'true';
            this.success = false;
            this.error_message = err;
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
      },
      clean: function () {
        this.success = true;
      }
    },
    created() {
      this.AuthState = $cookies.get('isAuthenticated') == 'true';
      this.username = $cookies.get('user-login');
    }
  }
</script>
<style>
  .active {
    visibility: visible;
    color: red;
  }
  .login_button {
    background-position: center;
    background-repeat: no-repeat;
    background-color: transparent;
    background-size: contain;
    border: 1px dotted;
    border-color: brown;
    border-radius: 18px;
    cursor: pointer; /* make the cursor like hovering over an <a> element */
    vertical-align: middle;
    width: 100px;
    height: 60px;
    margin: 8px;
  }
    .login_button:hover {
      border: 3px outset green;
      border-radius: 25px;
      font-weight: bold;
      background-color: beige;
    }

    #login_button {
      background-image: url(../assets/login.png);
    }
    #logout_button {
      background-image: url(../assets/logout.png);
    }

  .hidden {
    visibility: hidden;
  }
  .login{
    color:green;
  }
</style>

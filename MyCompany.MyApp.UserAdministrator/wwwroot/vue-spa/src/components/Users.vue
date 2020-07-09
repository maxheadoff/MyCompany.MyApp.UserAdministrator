<template>
  <div class="users" v-if="users">
    <div class="error-message">
      <p v-show="!success">Error receiving data from server,please, try later. Message:{{error_message}} <input type="button" value="x" v-on:click="clean" /> </p>
    </div>
    <input v-show="AuthState" class="new_button" type="button" value="" v-on:click="create_user" />
    <user-com :id="user.id"
              :key="user.id"
              v-for="user in users"
              class="User__View"
              v-bind:user_id=user.id
              v-on:refresh="refresh"
              v-on:user_deleted="user_deleted">
    </user-com>
  </div>
</template>

<script>
  import axios from 'axios'
  import user from './User.vue'
  export default {
    name: 'users',
    components: {
      'user-com': user
    },
    data() {
      return {
        users: [],
        endpoint: 'https://localhost:44378/api/users/',
        JWTToken: null,
        success: true,
        error_message: null
      }
    },
    methods: {
      getAllUsers() {
        axios.get(this.endpoint, { headers: { "Authorization": `Bearer ${this.JWTToken}`}})
          .then(response => {
            this.users = response.data;
            console.log('userlist received,count:' + this.users.length);
            this.$forceUpdate();
            this.success = true;
          })
          .catch(error => {
            console.log('---error:' + error);
            if (error.response.status === 401) {
              $cookies.remove('user-token'); // if the request fails, remove any possible user token if possible
              $cookies.remove('user-login');
              $cookies.set('isAuthenticated', false);
            }
            this.success = false;
              this.error_message = error;
          })
      },
      create_user: function () {
        var new_user = {
          name: 'new_user', login: 'new_user', id: 0 
        }
        this.users.push(new_user);
      },
      refresh: function (id) {
        this.getAllUsers();
      },
      user_deleted: function (id) {
        var index = this.users.findIndex(item => item.id == id);
        this.$delete(this.users, index);
        //this.$forceUpdate();
      },
      clean: function () {
        this.success = true;
      }
    },
    created() {
      this.JWTToken = $cookies.get('user-token');
      this.AuthState = $cookies.get('isAuthenticated') == 'true';
      this.getAllUsers();
    }
  }
</script>
<style>
  .users {
    float: none;
  }
  .User__View {
    float: none;
    margin-bottom: 4px;
    
  }
    .User__View:hover {
      border: groove;
      margin: 6px;
    }
  .new_button {
    background-image: url(../assets/add.png);
    background-position: center;
    background-repeat: no-repeat;
    background-color: antiquewhite;
    background-size: contain;
    border:2px solid;
    border-color:brown;
    border-radius:18px;
    cursor: pointer; /* make the cursor like hovering over an <a> element */
    vertical-align: middle;
    width: 100px;
    height: 60px;
    margin: 8px;
  }
    .new_button:hover {
      border: 3px outset green;
      border-radius: 25px;
      font-weight: bold;
      background-color: beige;
    }
   
</style>

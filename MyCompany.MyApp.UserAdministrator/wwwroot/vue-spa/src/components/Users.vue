<template>
  <div class="users" v-if="users">
    <input class="new_button" type="button" value="Create new user" v-on:click="create_user" />
    <user-com
              v-for="user in users"
              class="User__View"
              v-bind:user_id=user.id
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
        users: null,
        endpoint: 'https://localhost:44378/api/users/',
        JWTToken: null
      }
    },
    methods: {
      getAllUsers() {
        axios.get(this.endpoint, { headers: { "Authorization": `Bearer ${this.JWTToken}`}})
          .then(response => {
            this.users = response.data;
          })
          .catch(error => {
            console.log('---error:' + error);
            console.log(error);
          })
      },
      create_user: function () {
        var new_user = {
          name: 'new_user', login: 'new_user', id: 0 
        }
        this.users.push(new_user);
      },
      user_deleted: function (id) {
        this.users = this.users.filter(item => item.id !== id);
      }
    },
    created() {
      this.JWTToken = $cookies.get('user-token');
      this.getAllUsers();
    }
  }
</script>
<style>
  .users{
   float:none;
  }
  .User__View{
    float:none;
  }
  .new_button{
      color:darkred;
      font-weight:bold;
      border:groove;
      margin-bottom:10px;
      padding:6px;
  }
</style>

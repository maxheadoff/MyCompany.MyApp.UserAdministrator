<template>
  <div class="users" v-if="users">
    <input class="new_button" type="button" value="" v-on:click="create_user" />
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
        JWTToken: null
      }
    },
    methods: {
      getAllUsers() {
        axios.get(this.endpoint, { headers: { "Authorization": `Bearer ${this.JWTToken}`}})
          .then(response => {
            this.users = response.data;
            console.log('userlist received,count:' + this.users.length);
            this.$forceUpdate();
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
      refresh: function (id) {
        this.getAllUsers();
      },
      user_deleted: function (id) {
        var index = this.users.findIndex(item => item.id == id);
        this.$delete(this.users, index);
        //this.$forceUpdate();
      }
    },
    created() {
      this.JWTToken = $cookies.get('user-token');
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
   
</style>

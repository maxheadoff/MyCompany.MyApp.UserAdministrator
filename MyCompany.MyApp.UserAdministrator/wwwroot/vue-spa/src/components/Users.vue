<template>
  <div class="users" v-if="users">
    User list
    <div v-for="user in users" class="User__View">
        <h1 class="User__Name">{{ user.name }}</h1>
        {{ user.login }}{{ user.id }}
        <input type="button" value="Delete" />
        <input type="button" value="Edit" />
    </div>
  </div>
</template>

<script>
  import axios from 'axios'
  export default {
    name: 'app',
    data() {
      return {
        users: null,
        endpoint: 'https://localhost:44378/api/users/'
      }
    },
    methods: {
      getAllUsers() {
        axios.get(this.endpoint)
          .then(response => {
            this.users = response.data;
          })
          .catch(error => {
            console.log('---error:' + error);
            console.log(error);
          })
      }
    },
    created() {
      this.getAllUsers();
    }
  }
</script>
<style>
  .users{
    float: left;
  }
  .User__View{
    float:top;
  }
</style>

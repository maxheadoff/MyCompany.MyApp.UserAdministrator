<template>
  <div class="user" v-if="user">
    <h1 class="User__Name">{{ user.name }}</h1>
    <p class="User__Login">{{ user.login }}</p>
    <p class="User__id">{{ user.id }}</p>
  </div>
</template>

<script>
  import axios from 'axios';

  export default {
    props: ['id'],
    data() {
      return {
        user: null,
        endpoint: 'https://localhost:44378/api/users/',
      }
    },
    methods: {
      getUser(id) {
        axios(this.endpoint + id)
          .then(response => {
            this.user = response.data
          })
          .catch(error => {
            console.log(error)
          })
      }
    },
    created() {
      this.getUser(this.id);
    },
    watch: {
      '$route'() {
        this.getUser(this.id);
      }
    }
  }
</script>


////-----------error
<template>
  <div>
    <form class="login" @submit.prevent="login">
      <h1>Sign in</h1>
      <label>User name</label>
      <input required v-model="username" type="text" placeholder="Snoopy" />
      <label>Password</label>
      <input required v-model="password" type="password" placeholder="Password" />
      <hr />
      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script>
  import axios from 'axios';
  import { loginRoutine } from './login.js'

  export default {
    props: ['id'],
    data() {
      return {
        user: null,
        endpoint: 'https://localhost:44378/api/users/',
      }
    },
    methods: {
        login : function () {
          const { username, password } = this
          loginRoutine({ username, password }).then(() => {
            this.$router.push('/')
          })
      }
    }
  }
</script>

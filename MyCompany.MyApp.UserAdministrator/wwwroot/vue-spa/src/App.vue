<template>
  <div id="app">
    <header>
      <h1>Vue.js SPA</h1>
    </header>
    <main>
      <aside class="sidenav">
        <router-link to ="/">
          <p>Home</p>
        </router-link>
        <router-link to= "/login">
          <p>Login</p>
        </router-link>
        <router-link to="/Users">
          <p>Users</p>
        </router-link>
      </aside>
      <div class="content">
        <router-view></router-view>
      </div>
    </main>
  </div>
</template>

<script>
  import axios from 'axios'
  export default {
  name: 'app',
  data () {
    return {
      users: null,
      endpoint:'https://localhost:44378/api/users/'
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
main{
  height:100%;
}

  .sidenav {
    height: 100%;
    width: 200px;
    position: fixed;
    z-index: 1;
    top: 0;
    left: 0;
    background-color: #111;
    overflow-x: hidden;
    padding-top: 20px;
  }
    .sidenav a {
      padding: 8px 8px 8px 32px;
      text-decoration: none;
      font-size: 25px;
      color: #818181;
      display: block;
      transition: 0.3s;
    }

      /* When you mouse over the navigation links, change their color */
      .sidenav a:hover {
        color: #f1f1f1;
      }

    /* Position and style the close button (top right corner) */
    .sidenav .closebtn {
      position: absolute;
      top: 0;
      right: 25px;
      font-size: 36px;
      margin-left: 50px;
    }

  /* Style page content - use this if you want to push the page content to the right when you open the side navigation */
  .content {
    transition: margin-left .5s;
    padding: 20px;
    margin-left: 200px;
  }

  /* On smaller screens, where height is less than 450px, change the style of the sidenav (less padding and a smaller font size) */
  @media screen and (max-height: 450px) {
    .sidenav {
      padding-top: 15px;
    }

      .sidenav a {
        font-size: 18px;
      }
  }

 .app{
   height:100%;
 }

 .content{
   float:left;
 }

</style>

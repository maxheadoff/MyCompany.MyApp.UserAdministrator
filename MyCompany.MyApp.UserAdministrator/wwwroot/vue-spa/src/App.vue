<template>
  <div class="content" id="app">
    
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
  export default {
  name: 'app',
  data () {
    return {
      users: null,
      endpoint:'https://localhost:44378/api/users/'
    }
    }
}
</script>

<style>
main{
  height:100%;
  width:100%;
  margin:0px;
}

  .sidenav {
    height: 100%;
    width: 10%;
    position: fixed;
    z-index: 1;
    top: 0;
    left: 0;
    background-color:darkgrey;
    overflow-x: hidden;
    padding-top: 20px;
    float:left;
  }
    .sidenav a {
      padding: 8px 8px 8px 32px;
      text-decoration: none;
      font-size: 25px;
      color: #212121;
      display: block;
      transition: 0.3s;
    }

      /* When you mouse over the navigation links, change their color */
      .sidenav a:hover {
        color: #f9f9f9;
      }


  /* Style page content - use this if you want to push the page content to the right when you open the side navigation */
  .content {
    transition: margin-left .9s;
    background-color: burlywood;
    height: 100%;
    width: 80%;
    float: left;
    padding-left:7%;
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

 #app{
   height:100%;
   width:100%;
 }

</style>

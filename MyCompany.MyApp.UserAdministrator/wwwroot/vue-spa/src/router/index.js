import Vue from "vue";
import Router from "vue-router";
import Home from '../components/Home.vue'
import Users from '../components/Users.vue'
import Login from '../components/Login.vue'
import VueCookies from 'vue-cookies'

Vue.use(VueCookies);
Vue.use(Router);
Vue.$cookies.config('1d');

const ifNotAuthenticated = (to, from, next) => {
  if (!Vue.$cookies.get('isAuthenticated')) {
    next();
    return;
  }
  next('/');
};

const ifAuthenticated = (to, from, next) => {
  if (Vue.$cookies.get('isAuthenticated')) {
    next();
    return;
  }
  next("/login");
};

export default new Router({
  mode: "history",
  routes: [
    {
      path: "/",
      name: "Home",
      component: Home
    },
    {
      path: "/users",
      name: "Users",
      component: Users,
      beforeEnter: ifAuthenticated
    },
    {
      path: "/login",
      name: "Login",
      component: Login,
      //beforeEnter: ifNotAuthenticated
    }
  ]
});

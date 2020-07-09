import Vue from 'vue'
import router from './router'
import App from './App.vue'
import VueCookies from 'vue-cookies'
//import user from './components/User.vue'

Vue.use(VueCookies)
//Vue.component('user', user);

new Vue({
  el: '#app',
  render: h => h(App),
  router
})

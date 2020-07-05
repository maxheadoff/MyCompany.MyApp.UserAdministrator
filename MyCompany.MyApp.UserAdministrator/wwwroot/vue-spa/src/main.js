import Vue from 'vue'
import router from './router'
import App from './App.vue'
import VueCookies from 'vue-cookies'

Vue.use(VueCookies)

new Vue({
  el: '#app',
  render: h => h(App),
  router
})

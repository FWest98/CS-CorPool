import 'core-js/stable';
import 'regenerator-runtime/runtime';
import Vue from 'vue';
import './plugins/axios';
import vuetify from './plugins/vuetify';
import App from './App.vue';
import router from './router';
import store from '@/store/index';
import './registerServiceWorker';
import dateFilter from '@/filters/date.filter';
import { Datetime } from 'vue-datetime';
import 'vue-datetime/dist/vue-datetime.css';
Vue.component('datetime', Datetime);
Vue.config.productionTip = false;

Vue.filter('date', dateFilter);

// https://jasonwatmore.com/post/2018/07/06/vue-vuex-jwt-authentication-tutorial-example
router.beforeEach((to, from, next) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/login', '/register'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('token');
  const now = new Date();

  // TODO check expiration date here
  // now.getTime() > loggedIn.expiry

  if (authRequired && !loggedIn) {
    localStorage.removeItem('token');
    return next('/login');
  }

  next();
});


new Vue({
  vuetify,
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');

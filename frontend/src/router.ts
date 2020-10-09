import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';

Vue.use(Router);

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('./views/Login.vue'),
    },
    {
      path: '/offer-a-ride',
      name: 'offer-a-ride',
      component: () => import('./views/OfferARide.vue'),
    },
    {
      path: '/find-a-ride',
      name: 'find-a-ride',
      component: () => import('./views/FindARide.vue'),
    },
    {
      path: '/profile',
      name: 'profile',
      component: () => import('./views/Profile.vue'),
    },
  ],
});


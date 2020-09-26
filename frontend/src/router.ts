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
      path: '/offer-a-ride',
      name: 'offer-a-ride',
      // route level code-splitting
      // this generates a separate chunk (about.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('./views/OfferARide.vue'),
    },
    {
      path: '/find-a-ride',
      name: 'find-a-ride',
      component: () => import('./views/FindARide.vue'),
    },
  ],
});

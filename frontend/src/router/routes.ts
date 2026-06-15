export default [
    {
      path: '/login',
      name: 'login',
      component: () => import('@/components/pages/Login.vue'),
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('@/components/pages/NotFound.vue'),
    },
    {
      path: '/',
      name: 'home',
      component: () => import('@/components/pages/Home.vue'),
    }
  ]
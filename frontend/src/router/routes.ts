export default [
    {
      path: '/login',
      name: 'login',
      component: () => import('@/components/pages/Login.vue'),
      params: { registrationSuccess: 0 },
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/components/pages/Register.vue'),
    },
    {
      path: '/forgot-password',
      name: 'forgot-password',
      component: () => import('@/components/pages/ForgotPassword.vue'),
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
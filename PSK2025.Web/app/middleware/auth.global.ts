import type { RouteLocationNormalized } from 'vue-router'
import { useUserStore } from '~/store/user'

export default defineNuxtRouteMiddleware(async (to: RouteLocationNormalized) => {
  // 404 exclusion
  if (!(to.matched.length > 0)) {
    return
  }

  const userSt = useUserStore()

  if (['/signin', '/signin/', '/signup', '/signup/', '/'].includes(to.path) && userSt.isLoggedIn()) {
    return navigateTo('/dashboard')
  }

  if (!['/signin', '/signin/', '/signup', '/signup/', '/'].includes(to.path) && !userSt.isLoggedIn()) {
    return navigateTo('/signin')
  }
})

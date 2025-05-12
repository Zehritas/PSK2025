import { useUserStore } from '~/store/user'
import { getErrorMessage } from '~/utils/api'

export default defineNuxtPlugin((nuxtApp) => {
  const userSt = useUserStore()
  const toast = useToast()
  const config = useRuntimeConfig()

  const $apiFetch = $fetch.create({
    baseURL: config.public.apiUrl as string,
    retry: false,
    onRequest: async ({ options }) => {
      if ('guest' in options && options.guest === true) {
        return
      }

      try {
        const accessToken = await userSt.getAccessToken()

        options.headers.append('Authorization', `Bearer ${accessToken}`)
      } catch (e) {
        console.debug(e)

        toast.add({
          title: 'Your session has expired',
          description: 'Please sign in again to continue using the application.',
          color: 'error'
        })
        userSt.logout()
        await navigateTo('/signin')
      }
    },
    onRequestError: async (context) => {
      if ('showError' in context.options && context.options.showError === false) {
        return
      }

      await nuxtApp.runWithContext(() => {
        toast.add({
          title: 'An error has occurred',
          description: getErrorMessage(context),
          color: 'error'
        })
      })
    },
    onResponseError: async (context) => {
      if (
        context.response.status === 401 && (
          !('handleUnauthorized' in context.options) ||
          context.options.handleUnauthorized !== false
        )
      ) {
        await nuxtApp.runWithContext(async () => {
          toast.add({
            title: 'Your session has expired',
            description: 'Please sign in again to continue using the application.',
            color: 'error'
          })
          userSt.logout()
          await navigateTo('/signin')
        })

        return
      }

      if ('showError' in context.options && context.options.showError === false) {
        return
      }

      await nuxtApp.runWithContext(() => {
        toast.add({
          title: 'An error has occurred',
          description: getErrorMessage(context),
          color: 'error'
        })
      })
    }
  })

  return {
    provide: {
      apiFetch: $apiFetch
    }
  }
})

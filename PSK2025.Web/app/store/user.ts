import { jwtDecode } from 'jwt-decode'
import type { RefreshRequest, RefreshResponse } from '~/types/auth'

interface JWTMeta {
  aud: string
  exp: number
  iat: number
  iss: string
  jti: string
  nbf: number
  role: string
  sub: string
  unique_name: string
}

export const useUserStore = defineStore(
  'user',
  () => {
    const accessToken = ref<string | null>(null)
    const refreshToken = ref<string | null>(null)
    const refreshTokenExpiry = ref<number | null>(null)
    const refreshPromise = ref<Promise<void> | null>(null)

    const setAccessToken = (access: string): void => {
      accessToken.value = access
    }

    const setRefreshToken = (refresh: string): void => {
      refreshToken.value = refresh
      refreshTokenExpiry.value = Date.now() + 601200000 // 167 hours
    }

    const jwtMeta = computed((): JWTMeta | null => {
      if (accessToken.value) {
        return jwtDecode<JWTMeta>(accessToken.value)
      }

      return null
    })

    const isLoggedIn = (): boolean => {
      return accessToken.value !== null && jwtMeta.value !== null && jwtMeta.value.exp > (Date.now() - 60000) / 1000
    }

    const canRefresh = (): boolean => {
      return refreshToken.value !== null && refreshTokenExpiry.value !== null && refreshTokenExpiry.value > Date.now()
    }

    const refresh = (): Promise<void> => {
      if (refreshPromise.value) {
        return refreshPromise.value
      }

      if (!canRefresh()) {
        throw new Error('No refresh token')
      }

      refreshPromise.value = (async () => {
        try {
          const resp = await useApiDollarFetch<RefreshResponse>('/api/refresh', {
            method: 'POST',
            body: {
              token: refreshToken.value
            } as RefreshRequest,
            showError: false
          })

          setAccessToken(resp.token)
          setRefreshToken(resp.refreshToken)
        } catch (err) {
          logout()

          throw err
        } finally {
          refreshPromise.value = null
        }
      })()

      return refreshPromise.value
    }

    const logout = (): void => {
      accessToken.value = null
      refreshToken.value = null
      refreshTokenExpiry.value = null
      refreshPromise.value = null
    }

    const getAccessToken = async (): Promise<string> => {
      if (!isLoggedIn()) {
        try {
          await refresh()
          // eslint-disable-next-line @typescript-eslint/no-unused-vars
        } catch (_) { /* empty */
        }
      }

      if (accessToken.value) {
        return accessToken.value
      }

      useToast().add({
        title: 'Your session has expired',
        description: 'Please sign in again to continue using the application.',
        color: 'error'
      })

      await navigateTo('/signin')

      throw new Error('No access token')
    }

    return {
      accessToken,
      refreshToken,
      refreshTokenExpiry,
      setAccessToken,
      setRefreshToken,
      isLoggedIn,
      canRefresh,
      refresh,
      logout,
      getAccessToken
    }
  },
  {
    persist: {
      key: 'cs-user',
      storage: piniaPluginPersistedstate.cookies()
    }
  }
)

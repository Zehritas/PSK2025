import { getErrorMessage } from '~/utils/api'
import { useUserStore } from '~/store/user'

type UseFetchRequest<T> = Parameters<typeof useFetch<T>>[0]
type UseFetchOpts<T> = Parameters<typeof useFetch<T>>[1]
type UseDollarFetchRequest<T> = Parameters<typeof $fetch<T>>[0]
type UseDollarFetchOpts<T> = Parameters<typeof $fetch<T>>[1]

type ApiFetchOptions<T> = UseFetchOpts<T> & {
  showError?: boolean;
  guest?: boolean;
};

type ApiDollarFetchOptions<T> = UseDollarFetchOpts<T> & {
  showError?: boolean;
  guest?: boolean;
};

export const useApiFetch = <T, O extends ApiFetchOptions<T> = ApiFetchOptions<T>>(
  request: UseFetchRequest<T>,
  opts?: O
): ReturnType<typeof useFetch<T>> => {
  const toast = useToast()
  const runtimeConfig = useRuntimeConfig()
  const userStore = useUserStore()

  opts = adjustOptions<T, O>(toast, runtimeConfig, userStore, opts)

  return useFetch<T>(request, opts)
}

export const useApiDollarFetch = <T, O extends ApiDollarFetchOptions<T> = ApiDollarFetchOptions<T>>(
  request: UseDollarFetchRequest<T>,
  opts?: O
): ReturnType<typeof $fetch<T>> => {
  const toast = useToast()
  const runtimeConfig = useRuntimeConfig()
  const userStore = useUserStore()

  opts = adjustOptions<T, O>(toast, runtimeConfig, userStore, opts)

  return $fetch<T>(request, opts)
}

const adjustOptions = <T, O extends ApiFetchOptions<T> | ApiDollarFetchOptions<T>>(
  toast: ReturnType<typeof useToast>,
  runtimeConfig: ReturnType<typeof useRuntimeConfig>,
  userStore: ReturnType<typeof useUserStore>,
  opts?: O
): O => {
  if (!opts) {
    opts = {} as O
  }

  if (opts.showError !== false) {
    if (opts.onRequestError === undefined) {
      opts.onRequestError = (context) => {
        toast.add({
          title: 'An error has occurred',
          description: getErrorMessage(context),
          color: 'error'
        })
      }
    }

    if (opts.onResponseError === undefined) {
      opts.onResponseError = async (context) => {
        if (context.response.status === 401) {
          useToast().add({
            title: 'Your session has expired',
            description: 'Please sign in again to continue using the application.',
            color: 'error'
          })
          await navigateTo('/signin')

          return
        }

        toast.add({
          title: 'An error has occurred',
          description: getErrorMessage(context),
          color: 'error'
        })
      }
    }
  }

  if (opts.retry === undefined) {
    opts.retry = false
  }

  if (opts.baseURL === undefined) {
    opts.baseURL = runtimeConfig.public.apiUrl as string
  }

  if (opts.guest !== true) {
    opts.onRequest = async (context) => {
      try {
        const accessToken = await userStore.getAccessToken()

        context.options.headers.append('Authorization', `Bearer ${accessToken}`)
      } catch (e) {
        opts.onRequestError = undefined
        opts.onResponseError = undefined

        throw e
      }
    }
  }

  return opts
}

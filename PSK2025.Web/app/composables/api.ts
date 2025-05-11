type UseFetchRequest<T> = Parameters<typeof useFetch<T>>[0]
type UseFetchOpts<T> = Parameters<typeof useFetch<T>>[1]
type UseDollarFetchRequest<T> = Parameters<typeof $fetch<T>>[0]
type UseDollarFetchOpts<T> = Parameters<typeof $fetch<T>>[1]

type ApiFetchOptions<T> = UseFetchOpts<T> & {
  showError?: boolean;
  guest?: boolean;
  handleUnauthorized?: boolean;
};

type ApiDollarFetchOptions<T> = UseDollarFetchOpts<T> & {
  showError?: boolean;
  guest?: boolean;
  handleUnauthorized?: boolean;
};

export const useApiFetch = <T, O extends ApiFetchOptions<T> = ApiFetchOptions<T>>(
  request: UseFetchRequest<T>,
  opts?: O
): ReturnType<typeof useFetch<T>> => {
  return useFetch<T>(request, {
    ...opts,
    $fetch: useNuxtApp().$apiFetch
  })
}

export const useApiDollarFetch = <T, O extends ApiDollarFetchOptions<T> = ApiDollarFetchOptions<T>>(
  request: UseDollarFetchRequest<T>,
  opts?: O
): ReturnType<typeof $fetch<T>> => {
  return useNuxtApp().$apiFetch<T>(request, opts)
}

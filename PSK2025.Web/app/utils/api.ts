export const getErrorMessage = (error: Error | unknown): string => {
  if (error != null && typeof error === 'object') {
    if (
      'data' in error &&
      typeof error.data === 'object' &&
      error.data != null &&
      'title' in error.data &&
      typeof error.data.title === 'string'
    ) {
      return error.data.title
    }

    if (
      'name' in error &&
      error.name === 'FetchError'
    ) {
      return 'We had trouble reaching the server. Please try again shortly.'
    }
  }

  return 'Something went wrong. Please try again in a moment.'
}

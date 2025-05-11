export const getErrorMessage = (error: Error | unknown): string => {
  if (error != null && typeof error === 'object') {
    if (
      'data' in error &&
      typeof error.data === 'object' &&
      error.data != null
    ) {
      if (
        'title' in error.data &&
        typeof error.data.title === 'string'
      ) {
        return error.data.title
      }

      if (
        'description' in error.data &&
        typeof error.data.description === 'string'
      ) {
        return error.data.description
      }
    }

    if (
      'response' in error &&
      typeof error.response === 'object' &&
      error.response != null &&
      '_data' in error.response &&
      typeof error.response._data === 'object' &&
      error.response._data != null &&
      'title' in error.response._data &&
      typeof error.response._data.title === 'string'
    ) {
      return error.response._data.title
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

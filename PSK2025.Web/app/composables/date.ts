import moment from 'moment-timezone'

export const useFormatDate = (value: moment.Moment) => {
  return value.format('YYYY-MM-DD')
}

export const useFormatDateTime = (value: moment.Moment) => {
  return value.format('YYYY-MM-DD HH:mm:ss')
}

export const useFormatApiDate = (value: Date | string | null | undefined) => {
  const m = useParseApiDate(value)

  if (!m) {
    return ''
  }

  return useFormatDate(m)
}

export const useFormatApiDateTime = (value: Date | string | null | undefined) => {
  const m = useParseApiDate(value)

  if (!m) {
    return ''
  }

  return useFormatDateTime(m)
}

export const useParseApiDate = (value: Date | string | null | undefined) => {
  if (!value) {
    return null
  }

  const m = moment.tz(value, 'UTC')

  return m.tz(getUserTimezone())
}

export const useNewUserDate = () => {
  return useNewUserDateTime().hours(0).minutes(0).seconds(0).milliseconds(0)
}

export const useNewUserDateTime = () => {
  return moment.tz(getUserTimezone())
}

const getUserTimezone = () => {
  return 'UTC'
}

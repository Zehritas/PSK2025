import { process } from 'std-env'

export default defineNuxtConfig({
  devtools: { enabled: true },

  modules: [
    '@nuxt/ui-pro',
    '@nuxt/eslint',
    '@nuxt/fonts',
    '@nuxt/icon',
    '@pinia/nuxt',
    'pinia-plugin-persistedstate/nuxt',
    '@vueuse/nuxt',
  ],

  runtimeConfig: {
    public: {
      apiUrl: '',
    }
  },

  css: ['~/assets/css/main.css'],

  future: {
    compatibilityVersion: 4
  },

  compatibilityDate: '2024-11-27',

  app: {
    head: {
      link: [
        { rel: 'apple-touch-icon', sizes: '180x180', href: '/apple-touch-icon.png' },
        { rel: 'icon', type: 'image/png', sizes: '32x32', href: '/favicon-32x32.png' },
        { rel: 'icon', type: 'image/png', sizes: '16x16', href: '/favicon-16x16.png' },
        { rel: 'shortcut icon', type: 'image/x-icon', href: '/favicon.ico' },
        { rel: 'manifest', href: '/site.webmanifest' }
      ]
    }
  },

  vite: {
    esbuild: {
      pure: process.env.NODE_ENV ? ['console.debug', 'console.trace'] : [],
    },
  },

})

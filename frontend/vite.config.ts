import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

export default defineConfig({
  server: {
    host: true,
    port: 8000,
    allowedHosts: ['abzabza.ru'],
    watch: { usePolling: true },
    proxy: {
      '/api': {
        target: 'http://backend:8080',
        changeOrigin: true,
      },
      '/phpmyadmin': {
        target: 'http://phpmyadmin:80',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/phpmyadmin\/?/, '/'),
      },
    },
  },
  plugins: [
    vue(),
    vueDevTools(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
  css: {
    preprocessorOptions: {
      scss: {
        silenceDeprecations: ['mixed-decls', 'color-functions', 'global-builtin', 'import', 'if-function'],
        additionalData: `@import "@/styles/variables.scss";`,
      },
    }
  },
})

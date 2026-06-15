<template>
  <div class="not-found d-flex align-items-center justify-content-center min-vh-100">
    <div class="not-found__card text-center">
      <h1 class="not-found__code">404</h1>
      <div class="not-found__divider mx-auto mb-4"></div>
      <p class="not-found__title mb-2">Page not found</p>
      <p class="not-found__hint text-secondary mb-4">
        Please navigate to the main page.
      </p>
      <router-link class="btn btn-primary px-4" to="/">To the main page</router-link>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.not-found {
  &__code {
    font-size: 7rem;
    font-weight: 800;
    line-height: 1;
  }

  &__divider {
    width: 12rem;
    height: 3px;
    background: linear-gradient(90deg, $primary, $secondary);
    border-radius: 2px;
  }

  &__title {
    font-size: 1.4rem;
    font-weight: 600;
  }

  &__hint {
    font-size: 0.95rem;
  }
  .btn {
    background: linear-gradient(90deg, $primary, $secondary);
  }
}
</style>

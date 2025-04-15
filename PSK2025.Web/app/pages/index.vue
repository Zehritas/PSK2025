<template>
  <div class="min-h-screen flex flex-col">
    <div class="my-auto">
      <div
          class="w-full max-w-(--ui-container) mx-auto px-4 sm:px-6 lg:px-8 flex flex-col lg:grid py-24 sm:py-32
          lg:py-40 gap-16 sm:gap-y-24 lg:grid-cols-2 lg:items-center -mt-15">
        <div>
          <h1 class="text-5xl sm:text-7xl text-pretty tracking-tight font-bold text-(--ui-text-highlighted)">
            CS Student
          </h1>
          <p class="text-lg sm:text-xl/8 text-(--ui-text-muted) text-pretty mt-6">
            A collaborative task and group management tool built to simplify university projects — organize your team,
            assign responsibilities with deadlines, track task progress, and keep communication clear and focused in one
            unified space.
          </p>
          <div class="mt-10 flex flex-wrap gap-x-6 gap-y-3">
            <UButton to="/register" size="xl">
              Get started
            </UButton>

            <UButton to="/login" size="xl" color="neutral" variant="outline">
              Login
            </UButton>
          </div>
        </div>
        <div class="ring ring-(--ui-border-accented) overflow-hidden rounded-[calc(var(--ui-radius)*1.5)]">
          <img src="~/assets/img/landing.png" alt="Dashboard example">
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: 'guest',
})
</script>

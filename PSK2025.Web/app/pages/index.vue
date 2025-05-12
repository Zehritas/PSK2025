<template>
  <div
    class="flex flex-col lg:grid py-24 sm:py-32
          lg:py-40 gap-16 sm:gap-y-24 lg:grid-cols-2 lg:items-center -mt-15">
    <div>
      <h1 class="text-5xl sm:text-7xl text-pretty tracking-tight font-bold text-(--ui-text-highlighted)">
        <span class="text-primary">Co</span>Student
      </h1>
      <p class="text-lg sm:text-xl/8 text-(--ui-text-muted) text-pretty mt-6">
        A collaborative task and group management tool built to simplify university projects — organize your team,
        assign responsibilities with deadlines, track task progress, and keep communication clear and focused in one
        unified space.
      </p>
      <div class="mt-10 flex flex-wrap gap-x-6 gap-y-3">
        <UButton to="/signup" size="xl">
          Get started
        </UButton>

        <UButton to="/signin" size="xl" color="neutral" variant="outline">
          Sign In
        </UButton>
      </div>
    </div>
    <div class="ring ring-(--ui-border-accented) overflow-hidden rounded-[calc(var(--ui-radius)*1.5)]">
      <img src="~/assets/img/landing.png" alt="Dashboard example">
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: 'guest'
})
useSeoMeta({
  title: 'CoStudent'
})
</script>

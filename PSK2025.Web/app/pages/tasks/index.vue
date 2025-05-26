<template>
  <UDashboardPanel id="tasks">
    <template #header>
      <UDashboardNavbar title="Tasks">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <TaskCreate />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <TaskTable />
    </template>
  </UDashboardPanel>
</template>

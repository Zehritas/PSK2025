<template>
  <UDashboardPanel id="projects">
    <template #header>
      <UDashboardNavbar title="Projects">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <ProjectAddModal />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <ProjectTable />
    </template>
  </UDashboardPanel>
</template>

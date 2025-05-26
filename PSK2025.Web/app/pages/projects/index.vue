<template>
  <UDashboardPanel id="projects">
    <template #header>
      <UDashboardNavbar title="Projects">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <ProjectCreate />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <ProjectTable />
      <ProjectEdit />
    </template>
  </UDashboardPanel>
</template>

<template>
  <UDashboardPanel id="members">
    <template #header>
      <UDashboardNavbar title="Members">
        <template #leading>
          <UDashboardSidebarCollapse />
        </template>

        <template #right>
          <MemberCreate />
        </template>
      </UDashboardNavbar>
    </template>

    <template #body>
      <MemberTable />
    </template>
  </UDashboardPanel>
</template>

import type { Project } from '~/types/project'
import type { PaginatedList } from '~/types/list'

export const useProjectStore = defineStore(
  'project',
  () => {
    const projects = ref<Record<string, Project> | null>(null)
    const projectId = ref<string | null>(null)
    const projectsPromise = ref<Promise<void> | null>(null)

    const project = computed(() => {
      updateProjectId()

      if (projectId.value !== null) {
        return projects.value![projectId.value] ?? null
      }

      return null
    })

    const refreshProjects = async (force: boolean = false): Promise<void> => {
      if (projectsPromise.value) {
        updateProjectId()

        return projectsPromise.value
      }

      if (!force && projects.value !== null) {
        updateProjectId()

        return
      }

      return projectsPromise.value = new Promise((resolve, reject) => {
        useApiDollarFetch<PaginatedList<Project>>(
          '/api/projects',
          {
            query: {
              pageNumber: 1,
              pageSize: 1000
            }
          }
        ).then((apiProjects) => {
          projects.value = {}
          apiProjects.items.forEach((p) => {
            projects.value![p.id] = p
          })
          resolve()
        }).catch((e) => {
          reject(e)
        }).finally(() => {
          updateProjectId()

          projectsPromise.value = null
        })
      })
    }

    const updateProjectId = () => {
      if (projects.value === null) {
        return null
      }

      projectId.value = projectId.value ?? Object.keys(projects.value)[0] ?? null
    }

    return {
      projectId,
      project,
      projects,
      refreshProjects
    }
  },
  {
    persist: {
      key: 'cs-project',
      storage: piniaPluginPersistedstate.cookies(),
      pick: ['projectId']
    }
  }
)

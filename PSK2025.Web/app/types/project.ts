export interface Project {
  id: string;
  name: string;
  owner: {
    id: string;
    firstName: string;
    lastName: string;
  }
  startDate: string | null;
  endDate: string | null;
  description: string | null;
  status: number;
}

export interface NestedProject {
  project: Project;
}

export enum ProjectStatus {
  PLANNED,
  ACTIVE,
  COMPLETED,
  ARCHIVED
}

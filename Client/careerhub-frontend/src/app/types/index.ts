export type JobType =
  | "FullTime"
  | "PartTime"
  | "Contract"
  | "Internship";

export interface JobListing {
  id: string;
  title: string;
  company: string;
  location: string;
  jobType: JobType;
  salaryMin: number;
  salaryMax: number;
  postedDate : string;
  isOpen : boolean;
  applicantCount: number;
}
export interface PagedJobsResponse {
  data: JobListing[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

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
  employmentType: JobType;
  salaryMin: number;
  salaryMax: number;
  postedAt : string;
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
export interface ApplicationRequest {
  jobId: string;
  fullName: string;
  email: string;
  phone?: string;
  yearsOfExperience: number;
  coverLetter: string;
  linkedInUrl?: string;
  availableImmediately: boolean;
  noticePeriodWeeks: number;
}
export interface ApplicationResponse {
  id: string;
  jobId: string;
  email: string;
  submittedAt: string;
}


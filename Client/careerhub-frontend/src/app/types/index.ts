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
//posting jobs
export interface CreateJobRequest {
  title: string;
  description: string;
  companyId: string;       // UUID — employer selects/provides their company ID
  location: string;
  closingDate: string;     // ISO 8601 date string, serialised from DateTime on the API
  salaryMin?: number;      // optional — matches decimal? on the DTO
  salaryMax?: number;
  employmentType: JobType;
}
export interface CreateJobResponse {
  id: string;
  title: string;
  description: string;
  company: string;         // resolved company name returned by the API
  location: string;
  postedAt: string;
  employmentType: JobType;
  salaryMin: number;
  salaryMax: number;
  isOpen: boolean;
  applicationCount: number;
}
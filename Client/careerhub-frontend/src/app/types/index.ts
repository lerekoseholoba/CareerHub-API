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
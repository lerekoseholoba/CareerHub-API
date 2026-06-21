import { NextResponse } from "next/server";
import type { JobListing } from "../../types";

const jobs: JobListing[] = [
  {
    id: "a1",
    title: "Frontend Developer",
    company: "Capitec Bank",
    location: "Cape Town",
    jobType: "FullTime",
    salaryMin: 35000,
    salaryMax: 55000,
    postedDate: new Date().toISOString(),
    isOpen: true,
    applicantCount: 12,
  },
  {
    id: "a2",
    title: "Backend Engineer",
    company: "Takealot",
    location: "Remote",
    jobType: "FullTime",
    salaryMin: 45000,
    salaryMax: 70000,
    postedDate: new Date().toISOString(),
    isOpen: true,
    applicantCount: 0,
  },
  {
    id: "a3",
    title: "Data Analyst",
    company: "Discovery",
    location: "Johannesburg",
    jobType: "Contract",
    salaryMin: 30000,
    salaryMax: 50000,
    postedDate: new Date(Date.now() - 10 * 86400000).toISOString(),
    isOpen: true,
    applicantCount: 6,
  },
  {
    id: "a4",
    title: "UX Designer",
    company: "Nedbank",
    location: "Sandton",
    jobType: "PartTime",
    salaryMin: 25000,
    salaryMax: 40000,
    postedDate: new Date(Date.now() - 40 * 86400000).toISOString(),
    isOpen: true,
    applicantCount: 3,
  },
  {
    id: "a5",
    title: "DevOps Engineer",
    company: "Standard Bank",
    location: "Johannesburg",
    jobType: "FullTime",
    salaryMin: 60000,
    salaryMax: 90000,
    postedDate: new Date(Date.now() - 3 * 86400000).toISOString(),
    isOpen: false,
    applicantCount: 18,
  },
  {
    id: "a6",
    title: "Software Intern",
    company: "FNB",
    location: "Remote",
    jobType: "Internship",
    salaryMin: 8000,
    salaryMax: 12000,
    postedDate: new Date(Date.now() - 60 * 86400000).toISOString(),
    isOpen: true,
    applicantCount: 0,
  },
];

// Handles GET /api/jobs
export async function GET() {
  return NextResponse.json(jobs);
}
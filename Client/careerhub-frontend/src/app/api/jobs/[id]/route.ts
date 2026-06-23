import { NextResponse } from "next/server";
import type { JobListing } from "../../../types";

const jobs: JobListing[] = [
  {
    id: "a1",
    title: "Frontend Developer",
    company: "Capitec Bank",
    location: "Cape Town",
    employmentType: "FullTime",
    salaryMin: 35000,
    salaryMax: 55000,
    postedAt: new Date().toISOString(),
    isOpen: true,
    applicantCount: 12,
    description:
      "Build and maintain customer-facing web interfaces for Capitec's digital banking platform. You'll work in a cross-functional team delivering React-based features, collaborating closely with design and backend engineers.",
  },
  {
    id: "a2",
    title: "Backend Engineer",
    company: "Takealot",
    location: "Remote",
    employmentType: "FullTime",
    salaryMin: 45000,
    salaryMax: 70000,
    postedAt: new Date().toISOString(),
    isOpen: true,
    applicantCount: 0,
    description:
      "Design and scale the services powering South Africa's largest e-commerce platform. You'll own microservices in Node.js or Go, work with high-throughput data pipelines, and participate in on-call rotations.",
  },
  {
    id: "a3",
    title: "Data Analyst",
    company: "Discovery",
    location: "Johannesburg",
    employmentType: "Contract",
    salaryMin: 30000,
    salaryMax: 50000,
    postedAt: new Date(Date.now() - 10 * 86400000).toISOString(),
    isOpen: true,
    applicantCount: 6,
    description:
      "Six-month contract analysing health and life insurance claim patterns. You'll use SQL and Python to surface insights for actuarial and product teams, and build Tableau dashboards consumed by senior leadership.",
  },
  {
    id: "a4",
    title: "UX Designer",
    company: "Nedbank",
    location: "Sandton",
    employmentType : "PartTime",
    salaryMin: 25000,
    salaryMax: 40000,
    postedAt: new Date(Date.now() - 40 * 86400000).toISOString(),
    isOpen: true,
    applicantCount: 3,
    description:
      "Part-time role (three days per week) shaping the experience of Nedbank's retail mobile app. Responsibilities include user research, wireframing, prototyping in Figma, and conducting usability testing sessions.",
  },
  {
    id: "a5",
    title: "DevOps Engineer",
    company: "Standard Bank",
    location: "Johannesburg",
    employmentType: "FullTime",
    salaryMin: 60000,
    salaryMax: 90000,
    postedAt: new Date(Date.now() - 3 * 86400000).toISOString(),
    isOpen: false,
    applicantCount: 18,
    description:
      "Own the CI/CD pipelines and Kubernetes infrastructure supporting Standard Bank's core banking services. You'll drive cloud migration initiatives on AWS, enforce security baselines, and mentor junior engineers on platform tooling.",
  },
  {
    id: "a6",
    title: "Software Intern",
    company: "FNB",
    location: "Remote",
    employmentType: "Internship",
    salaryMin: 8000,
    salaryMax: 12000,
    postedAt: new Date(Date.now() - 60 * 86400000).toISOString(),
    isOpen: true,
    applicantCount: 0,
    description:
      "A six-month paid internship for final-year or recent computer science graduates. You'll rotate through FNB's product squads, contribute to real features under senior mentorship, and attend structured learning sessions each week.",
  },
];
//Next.js calls this function when a GET request hits /api/jobs/[id]
export async function GET(
  _request: Request,
  { params }: { params: Promise<{ id: string }> }// object next.js uses to pass route parameters to the handler
) {
  const { id } = await params;  // resolve once, up front
  const job = jobs.find((j) => j.id === id);  // plain sync callback
  if (!job) {
    return NextResponse.json(
      {
        title: "Not Found",
        detail: `No job with id '${id}' exists.`,
        status: 404,
      },
      { status: 404 }
    );
  }

  return NextResponse.json(job);
}

// All other methods → 405
export async function POST() {
  return NextResponse.json(
    {
      title: "Method Not Allowed",
      detail: "This endpoint only accepts GET requests.",
      status: 405,
    },
    { status: 405, headers: { Allow: "GET" } }
  );
}
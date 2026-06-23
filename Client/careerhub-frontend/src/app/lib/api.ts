import type {
  PagedJobsResponse,
  ApplicationRequest,
  ApplicationResponse,
  CreateJobRequest,
  CreateJobResponse,
} from "../types";

const BASE_URL = process.env.NEXT_PUBLIC_API_URL;

export async function fetchJobs(): Promise<PagedJobsResponse> {
  if (!BASE_URL) throw new Error("NEXT_PUBLIC_API_URL is not defined");

  const res = await fetch(`${BASE_URL}/api/v1/Jobs`);

  if (!res.ok) {
    throw new Error(`Failed to fetch jobs. HTTP status: ${res.status}`);
  }

  return (await res.json()) as PagedJobsResponse;
}

export async function submitApplication(
  application: ApplicationRequest
): Promise<ApplicationResponse> {
  const res = await fetch("/api/applications", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(application),
  });

  if (!res.ok) {
    let message = "Failed to submit application.";
    try {
      const problem = await res.json();
      if (problem.detail) message = problem.detail;
    } catch { /* ignore invalid JSON */ }
    throw new Error(message);
  }

  return (await res.json()) as ApplicationResponse;
}

export async function createJob(
  job: CreateJobRequest
): Promise<CreateJobResponse> {
  const res = await fetch("/api/jobs", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(job),
  });

  if (!res.ok) {
    let message = "Failed to post job.";
    try {
      const problem = await res.json();
      if (problem.detail) message = problem.detail;
    } catch { /* ignore invalid JSON */ }
    throw new Error(message);
  }

  return (await res.json()) as CreateJobResponse;
}
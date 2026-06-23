import type {
  PagedJobsResponse,
  ApplicationRequest,
  ApplicationResponse,
} from "../types";

const BASE_URL = process.env.NEXT_PUBLIC_API_URL;

export async function fetchJobs(): Promise<PagedJobsResponse> {
  if (!BASE_URL) {
    throw new Error("NEXT_PUBLIC_API_URL is not defined");
  }

  const url = `${BASE_URL}/api/v1/Jobs`;

  const res = await fetch(url);

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
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(application),
  });

  if (!res.ok) {
    let message = "Failed to submit application.";

    try {
      const problem = await res.json();

      if (problem.detail) {
        message = problem.detail;
      }
    } catch {
      // Ignore invalid JSON responses
    }

    throw new Error(message);
  }

  return (await res.json()) as ApplicationResponse;
}
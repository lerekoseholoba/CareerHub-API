import type { JobListing } from "../types";
const BASE_URL = process.env.NEXT_PUBLIC_API_URL;

export async function fetchJobs(): Promise<JobListing[]> {
  if (!BASE_URL) {
    throw new Error("NEXT_PUBLIC_API_URL is not defined");
  }

  const url = `${BASE_URL}/api/jobs`;

  const res = await fetch(url);

  if (!res.ok) {
    throw new Error(`Failed to fetch jobs. HTTP status: ${res.status}`);
  }

  const data = (await res.json()) as JobListing[];

  return data;
}
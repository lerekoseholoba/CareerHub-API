import type { JobListing } from "../../types";
import JobsTable from "./JobsTable";

interface ApplicationStat {
  jobId: string;
  applicationCount: number;
}

async function getJobs(): Promise<JobListing[]> {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/v1/Jobs`,
    {
      next: { tags: ["jobs"] },
    }
  );

  if (!res.ok) {
    throw new Error(`Failed to fetch jobs. HTTP status: ${res.status}`);
  }

  const json = (await res.json()) as { data: JobListing[] };
  return json.data;
}

async function getApplicationStats(): Promise<ApplicationStat[]> {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/v1/Applications/stats`,
    {
      cache: "no-store",
    }
  );

  if (!res.ok) {
    throw new Error(
      `Failed to fetch application statistics. HTTP status: ${res.status}`
    );
  }

  return (await res.json()) as ApplicationStat[];
}
export default async function ListingsPage() {
  // Fetch both requests in parallel
  const [jobs, stats] = await Promise.all([
    getJobs(),
    getApplicationStats(),
  ]);

  // Join the data together
  const jobsWithApplications = jobs.map((job) => ({
    ...job,
    applicationCount:
      stats.find((stat) => stat.jobId === job.id)?.applicationCount ?? 0,
  }));

  return <JobsTable jobs={jobsWithApplications} />;
}
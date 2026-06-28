import type { JobListing } from "../types/index";
import JobLinkCard from "../components/JobLinkCard";
import JobFilters from "../components/JobFilters";

export const dynamic = "force-dynamic";

async function getJobs(): Promise<JobListing[]> {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/v1/Jobs`,
    { next: { tags: ["jobs"] } }
  );

  if (!res.ok) {
    throw new Error(`Failed to fetch jobs. HTTP status: ${res.status}`);
  }

  const json = await res.json() as { data: JobListing[] };
  return json.data;
}

interface SearchParams {
  q?: string;
  location?: string;
  status?: string;
}

export default async function JobsPage({
  searchParams,
}: {
  searchParams: Promise<SearchParams>;
}) {
  const { q = "", location = "", status = "all" } = await searchParams;

  const allJobs = await getJobs();

  // Filter in JS after cache retrieval — API does not support query params
  const jobs = allJobs.filter((job) => {
    const keyword = q.toLowerCase();
    const loc     = location.toLowerCase();

    const matchesKeyword =
      keyword === "" ||
      job.title.toLowerCase().includes(keyword) ||
      job.description.toLowerCase().includes(keyword);

    const matchesLocation =
      loc === "" ||
      job.location.toLowerCase().includes(loc);

    const matchesStatus =
      status === "all" ||
      (status === "open" && job.isOpen);

    return matchesKeyword && matchesLocation && matchesStatus;
  });

  return (
    <main className="min-h-screen bg-gray-50 p-8 text-gray-900 dark:bg-gray-950 dark:text-gray-100">
      <div className="mx-auto max-w-4xl space-y-6">
        <h1 className="text-3xl font-bold">Job Listings</h1>

        <JobFilters />

        {jobs.length === 0 ? (
          <div className="rounded-lg border border-gray-200 bg-white p-12 text-center dark:border-gray-700 dark:bg-gray-900">
            <p className="text-sm font-medium text-gray-500 dark:text-gray-400">
              No jobs match your filters.
            </p>
          </div>
        ) : (
          <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
            {jobs.map((job) => (
              <JobLinkCard key={job.id} job={job} />
            ))}
          </div>
        )}
      </div>
    </main>
  );
}
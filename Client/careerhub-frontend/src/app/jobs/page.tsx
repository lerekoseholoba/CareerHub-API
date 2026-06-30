import type { JobListing } from "../types/index";
import JobLinkCard from "../components/JobLinkCard";
import JobFilters from "../components/JobFilters";
import ClearFiltersButton from "../components/ClearFiltersButton";

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

  // ── Determine which empty state applies ──────────────────────────────
  // The DB-empty case is determined from allJobs (the unfiltered set),
  // not jobs (the filtered set) — this is the only correct signal, since
  // jobs.length === 0 alone is ambiguous between "DB is empty" and
  // "filters matched nothing." allJobs.length === 0 can only be true if
  // the DB genuinely has no listings, because allJobs is fetched before
  // any filter is applied.
  const isDatabaseEmpty = allJobs.length === 0;
  const filtersAreActive = q !== "" || location !== "" || status !== "all";
  const noResultsFromFilters = !isDatabaseEmpty && jobs.length === 0 && filtersAreActive;

  return (
    <main className="min-h-screen bg-gray-50 p-8 text-gray-900 dark:bg-gray-950 dark:text-gray-100">
      <div className="mx-auto max-w-4xl space-y-6">
        <h1 className="text-3xl font-bold">Job Listings</h1>

        <JobFilters />

        {isDatabaseEmpty ? (
          // State 1: no jobs in the DB at all — no action available
          <div className="rounded-lg border border-gray-200 bg-white p-12 text-center dark:border-gray-700 dark:bg-gray-900">
            <p className="text-sm font-medium text-gray-500 dark:text-gray-400">
              No jobs are currently listed.
            </p>
          </div>
        ) : noResultsFromFilters ? (
          // State 2: filters eliminated all results — offer a way out
          <div className="rounded-lg border border-gray-200 bg-white p-12 text-center dark:border-gray-700 dark:bg-gray-900">
            <p className="text-sm font-medium text-gray-500 dark:text-gray-400">
              No jobs match your search.
            </p>
            <p className="mt-1 text-xs text-gray-400 dark:text-gray-500">
              {[
                q && `keyword "${q}"`,
                location && `location "${location}"`,
                status !== "all" && `status "${status}"`,
              ]
                .filter(Boolean)
                .join(", ")}
            </p>
            <ClearFiltersButton />
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
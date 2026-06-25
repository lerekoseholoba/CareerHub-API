import { Suspense } from "react";

/**
 * Server-side fetch (runs only on server)
 */
async function getApplicationStats(): Promise<
  { jobId: string; applicationCount: number }[]
> {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/v1/applications/stats`,
    {
      cache: "no-store",
    }
  );

  if (!res.ok) {
    throw new Error(`Failed to fetch application stats: ${res.status}`);
  }

  return res.json();
}

/**
 * Inline skeleton fallback for Suspense
 */
export function ApplicationsSummarySkeleton() {
  return (
    <div className="rounded-lg border bg-white p-6 dark:border-gray-700 dark:bg-gray-900">
      <div className="h-4 w-40 animate-pulse rounded bg-gray-300 dark:bg-gray-700" />
      <div className="mt-3 h-8 w-20 animate-pulse rounded bg-gray-300 dark:bg-gray-700" />
    </div>
  );
}

/**
 * Server Component (NO "use client")
 */
async function ApplicationsSummaryInner() {
  const stats = await getApplicationStats();

  const totalApplications = stats.reduce(
    (sum, item) => sum + item.applicationCount,
    0
  );

  return (
    <div className="rounded-lg border bg-white p-6 dark:border-gray-700 dark:bg-gray-900">
      <p className="text-sm text-gray-500 dark:text-gray-400">
        Total Applications
      </p>

      <p className="mt-2 text-3xl font-bold text-gray-900 dark:text-gray-100">
        {totalApplications}
      </p>
    </div>
  );
}

/**
 * Exported component with Suspense boundary
 */
export default function ApplicationsSummary() {
  return (
    <Suspense fallback={<ApplicationsSummarySkeleton />}>
      <ApplicationsSummaryInner />
    </Suspense>
  );
}
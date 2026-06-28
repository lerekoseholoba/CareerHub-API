import { Suspense } from "react";
import ApplicationsSummary, {
  ApplicationsSummarySkeleton,
} from "../../components/ApplicationsSummary";
import { ListingsTableSkeleton } from "../../components/ListingsTable";
import DashboardToolbar from "../../components/DashboardToolbar";
import ListingsView from "../../components/ListingsView";
import type { JobListing } from "../../types/index";

async function getJobs(): Promise<JobListing[]> {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/v1/Jobs`,
    { cache: "no-store" }
  );
  if (!res.ok) throw new Error(`Failed to fetch jobs. HTTP status: ${res.status}`);
  const json = await res.json();
  return json.data;
}

export default async function ListingsPage() {
  const jobs = await getJobs();

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-gray-100">
            Dashboard Listings
          </h1>
          <p className="text-sm text-gray-500 dark:text-gray-400">
            Manage jobs and track applications in real time
          </p>
        </div>
        <DashboardToolbar />
      </div>

      <Suspense fallback={<ApplicationsSummarySkeleton />}>
        <ApplicationsSummary />
      </Suspense>

      <Suspense fallback={<ListingsTableSkeleton />}>
        <ListingsView jobs={jobs} />
      </Suspense>
    </div>
  );
}
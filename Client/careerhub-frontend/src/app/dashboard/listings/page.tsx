import { Suspense } from "react";
import ApplicationsSummary from "../../components/ApplicationsSummary";
import ListingsTable from "../../components/ListingsTable";
import {
  ApplicationsSummarySkeleton,
} from "../../components/ApplicationsSummary";
import {
  ListingsTableSkeleton,
} from "../../components/ListingsTable";

export default async function ListingsPage() {
  return (
    <div className="space-y-6">
      {/* Page heading renders immediately */}
      <div>
        <h1 className="text-2xl font-bold text-gray-900 dark:text-gray-100">
          Dashboard Listings
        </h1>
        <p className="text-sm text-gray-500 dark:text-gray-400">
          Manage jobs and track applications in real time
        </p>
      </div>

      {/* FIRST STREAM: Applications summary */}
      <Suspense fallback={<ApplicationsSummarySkeleton />}>
        <ApplicationsSummary />
      </Suspense>

      {/* SECOND STREAM: Listings table */}
      <Suspense fallback={<ListingsTableSkeleton />}>
        <ListingsTable />
      </Suspense>
    </div>
  );
}
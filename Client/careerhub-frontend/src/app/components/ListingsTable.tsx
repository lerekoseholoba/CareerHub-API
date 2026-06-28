import Link from "next/link";
import CloseJobButton from "./CloseJobButton";
import type { JobListing } from "../app/types/index";

interface Props {
  jobs: JobListing[];
  view: "table" | "grid";
}

export default function ListingsTable({ jobs, view }: Props) {
  if (jobs.length === 0) {
    return (
      <div className="rounded-lg border border-gray-200 bg-white p-12 text-center dark:border-gray-700 dark:bg-gray-900">
        <p className="text-sm font-medium text-gray-500 dark:text-gray-400">
          No listings yet. Post a job to get started.
        </p>
      </div>
    );
  }

  if (view === "grid") {
    return (
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {jobs.map((job) => (
          <div
            key={job.id}
            className="rounded-lg border border-gray-200 bg-white p-5 shadow-sm dark:border-gray-700 dark:bg-gray-900 space-y-3"
          >
            <div className="flex items-start justify-between gap-2">
              <h3 className="font-semibold text-gray-900 dark:text-gray-100 leading-tight">
                {job.title}
              </h3>
              <span className={
                job.isOpen
                  ? "shrink-0 rounded-full bg-green-100 px-2 py-0.5 text-xs font-medium text-green-700 dark:bg-green-900 dark:text-green-300"
                  : "shrink-0 rounded-full bg-red-100 px-2 py-0.5 text-xs font-medium text-red-700 dark:bg-red-900 dark:text-red-300"
              }>
                {job.isOpen ? "Open" : "Closed"}
              </span>
            </div>

            <p className="text-sm text-gray-600 dark:text-gray-400">
              {job.company} · {job.location}
            </p>

            <p className="text-xs text-gray-500 dark:text-gray-500">
              {job.applicationCount ?? 0} application{job.applicationCount !== 1 ? "s" : ""}
            </p>

            <div className="flex items-center gap-3 pt-1">
              <Link
                href={`/jobs/${job.id}`}
                className="text-sm text-blue-600 hover:underline dark:text-blue-400"
              >
                View
              </Link>
              <CloseJobButton
                jobId={job.id}
                currentStatus={job.isOpen ? "Open" : "Closed"}
              />
            </div>
          </div>
        ))}
      </div>
    );
  }

  // Default: table view
  return (
    <div className="overflow-hidden rounded-lg border border-gray-200 dark:border-gray-700">
      <table className="w-full text-sm">
        <thead className="bg-gray-50 text-left text-xs font-semibold uppercase tracking-wider text-gray-500 dark:bg-gray-800 dark:text-gray-400">
          <tr>
            <th className="px-4 py-3">Title</th>
            <th className="px-4 py-3">Company</th>
            <th className="px-4 py-3">Location</th>
            <th className="px-4 py-3">Status</th>
            <th className="px-4 py-3">Applications</th>
            <th className="px-4 py-3">View</th>
            <th className="px-4 py-3">Action</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-200 bg-white dark:divide-gray-700 dark:bg-gray-900">
          {jobs.map((job) => (
            <tr key={job.id} className="hover:bg-gray-50 dark:hover:bg-gray-800">
              <td className="px-4 py-3 font-medium text-gray-900 dark:text-gray-100">
                {job.title}
              </td>
              <td className="px-4 py-3 text-gray-600 dark:text-gray-400">
                {job.company}
              </td>
              <td className="px-4 py-3 text-gray-600 dark:text-gray-400">
                {job.location}
              </td>
              <td className="px-4 py-3">
                <span className={
                  job.isOpen
                    ? "rounded-full bg-green-100 px-2 py-0.5 text-xs font-medium text-green-700 dark:bg-green-900 dark:text-green-300"
                    : "rounded-full bg-red-100 px-2 py-0.5 text-xs font-medium text-red-700 dark:bg-red-900 dark:text-red-300"
                }>
                  {job.isOpen ? "Open" : "Closed"}
                </span>
              </td>
              <td className="px-4 py-3 text-gray-600 dark:text-gray-400">
                {job.applicationCount ?? 0}
              </td>
              <td className="px-4 py-3">
                <Link
                  href={`/jobs/${job.id}`}
                  className="text-blue-600 hover:underline dark:text-blue-400"
                >
                  View
                </Link>
              </td>
              <td className="px-4 py-3">
                <CloseJobButton
                  jobId={job.id}
                  currentStatus={job.isOpen ? "Open" : "Closed"}
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export function ListingsTableSkeleton() {
  return (
    <div className="overflow-hidden rounded-lg border border-gray-200 dark:border-gray-700">
      <div className="p-4 space-y-3 animate-pulse">
        {Array.from({ length: 5 }).map((_, i) => (
          <div key={i} className="h-10 w-full rounded bg-gray-200 dark:bg-gray-700" />
        ))}
      </div>
    </div>
  );
}
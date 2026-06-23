import Link from "next/link";
import type { JobListing } from "../../types/index";

async function getJobs(): Promise<JobListing[]> {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/v1/Jobs`,
    { cache: "no-store" }
  );

  if (!res.ok) {
    throw new Error(`Failed to fetch jobs. HTTP status: ${res.status}`);
  }

  const json = await res.json() as { data: JobListing[] };
  return json.data;
}

export default async function ListingsPage() {
  const jobs = await getJobs();

  return (
    <div className="space-y-4">
      <div>
        <h1 className="text-2xl font-bold text-gray-900 dark:text-gray-100">
          All Listings
        </h1>
        <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
          {jobs.length} {jobs.length === 1 ? "listing" : "listings"}
        </p>
      </div>

      {jobs.length === 0 ? (
        <div className="rounded-lg border border-gray-200 bg-white p-12 text-center dark:border-gray-700 dark:bg-gray-900">
          <p className="text-sm font-medium text-gray-500 dark:text-gray-400">
            No listings yet. Post a job to get started.
          </p>
        </div>
      ) : (
        <div className="overflow-hidden rounded-lg border border-gray-200 dark:border-gray-700">
          <table className="w-full text-sm">
            <thead className="bg-gray-50 text-left text-xs font-semibold uppercase tracking-wider text-gray-500 dark:bg-gray-800 dark:text-gray-400">
              <tr>
                <th className="px-4 py-3">Title</th>
                <th className="px-4 py-3">Company</th>
                <th className="px-4 py-3">Location</th>
                <th className="px-4 py-3">Status</th>
                <th className="px-4 py-3"></th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200 bg-white dark:divide-gray-700 dark:bg-gray-900">
              {jobs.map((job) => (
                <tr
                  key={job.id}
                  className="hover:bg-gray-50 dark:hover:bg-gray-800"
                >
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
                    <span
                      className={
                        job.isOpen
                          ? "rounded-full bg-green-100 px-2 py-0.5 text-xs font-medium text-green-700 dark:bg-green-900 dark:text-green-300"
                          : "rounded-full bg-red-100 px-2 py-0.5 text-xs font-medium text-red-700 dark:bg-red-900 dark:text-red-300"
                      }
                    >
                      {job.isOpen ? "Open" : "Closed"}
                    </span>
                  </td>
                  <td className="px-4 py-3 text-right">
                    <Link
                      href={`/jobs/${job.id}`}
                      className="text-blue-600 hover:underline dark:text-blue-400"
                    >
                      View
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
import { notFound } from "next/navigation";
import Link from "next/link";
import type { JobListing } from "../../types/index";
import ApplicationWizard from "../../components/ApplicationWizard";
import { auth } from "@/auth";

async function getJob(id: string): Promise<JobListing> {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_API_URL}/api/v1/Jobs/${id}`,
    { next: { tags: ["jobs"] } }
  );

  if (res.status === 404) notFound();

  if (!res.ok) {
    throw new Error(`Failed to fetch job. HTTP status: ${res.status}`);
  }

  return res.json() as Promise<JobListing>;
}

export default async function JobDetailPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  const [job, session] = await Promise.all([getJob(id), auth()]);
  const role = session?.user?.role;

  return (
    <main className="min-h-screen bg-gray-50 p-8 text-gray-900 dark:bg-gray-950 dark:text-gray-100">
      <div className="mx-auto max-w-2xl space-y-6">

        {/* Back link */}
        <Link
          href="/jobs"
          className="inline-flex items-center gap-1 text-sm text-blue-600 hover:underline dark:text-blue-400"
        >
          ← Back to jobs
        </Link>

        {/* Job details */}
        <div className="rounded-lg border border-gray-200 bg-white p-6 shadow-sm dark:border-gray-700 dark:bg-gray-900">
          <div className="mb-4 flex items-start justify-between gap-4">
            <div>
              <h1 className="text-2xl font-bold text-gray-900 dark:text-gray-100">
                {job.title}
              </h1>
              <p className="mt-1 text-sm text-gray-600 dark:text-gray-400">
                {job.company} · {job.location}
              </p>
            </div>

            <span className={
              job.isOpen
                ? "shrink-0 rounded-full bg-green-100 px-3 py-1 text-xs font-medium text-green-700 dark:bg-green-900 dark:text-green-300"
                : "shrink-0 rounded-full bg-red-100 px-3 py-1 text-xs font-medium text-red-700 dark:bg-red-900 dark:text-red-300"
            }>
              {job.isOpen ? "Open" : "Closed"}
            </span>
          </div>

          <p className="whitespace-pre-line text-sm leading-relaxed text-gray-700 dark:text-gray-300">
            {job.description}
          </p>
        </div>

        {/* Application section */}
        {job.isOpen ? (
          <>
            {role === "employer" && (
              <div className="rounded-lg border border-gray-200 bg-white p-6 text-center shadow-sm dark:border-gray-700 dark:bg-gray-900">
                <p className="text-sm font-medium text-gray-600 dark:text-gray-400">
                  Employers cannot apply for jobs.
                </p>
              </div>
            )}

            {/*
              Signed-out and candidate cases both render the wizard now.
              The wizard itself gates step 1 → step 2 based on isSignedIn/role,
              per the assignment ("must not advance past step 1... inline message,
              do not redirect"). We no longer branch the form out of the tree
              for signed-out users.
            */}
            {role !== "employer" && (
              <ApplicationWizard
                jobId={job.id}
                jobTitle={job.title}
                isSignedIn={!!session}
                role={role}
              />
            )}
          </>
        ) : (
          <div className="rounded-lg border border-gray-200 bg-white p-6 text-center shadow-sm dark:border-gray-700 dark:bg-gray-900">
            <p className="text-sm font-medium text-gray-600 dark:text-gray-400">
              This position is no longer accepting applications.
            </p>
          </div>
        )}

      </div>
    </main>
  );
}
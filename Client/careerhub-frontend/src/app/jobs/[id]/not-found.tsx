import Link from "next/link";

export default function NotFound() {
  return (
    <main className="min-h-screen bg-gray-50 p-8 text-gray-900 dark:bg-gray-950 dark:text-gray-100">
      <div className="mx-auto max-w-2xl space-y-4">
        <h1 className="text-2xl font-bold">Job Not Found</h1>
        <p className="text-sm text-gray-600 dark:text-gray-400">
          The job you're looking for doesn't exist or may have been removed.
        </p>
        <Link
          href="/jobs"
          className="inline-flex items-center gap-1 text-sm text-blue-600 hover:underline dark:text-blue-400"
        >
          ← Back to job listings
        </Link>
      </div>
    </main>
  );
}
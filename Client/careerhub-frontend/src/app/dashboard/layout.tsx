import Link from "next/link";

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="flex min-h-screen">
      {/* Sidebar */}
      <aside className="w-64 shrink-0 border-r border-gray-200 bg-white px-6 py-8 dark:border-gray-700 dark:bg-gray-900">
        <p className="mb-6 text-xs font-semibold uppercase tracking-widest text-gray-400 dark:text-gray-500">
          Employer Dashboard
        </p>

        <nav className="space-y-1">
          <Link
            href="/dashboard"
            className="block rounded-md px-3 py-2 text-sm font-medium text-gray-700 hover:bg-gray-100 hover:text-gray-900 dark:text-gray-300 dark:hover:bg-gray-800 dark:hover:text-gray-100"
          >
            All Listings
          </Link>

          <Link
            href="/jobs"
            className="block rounded-md px-3 py-2 text-sm font-medium text-gray-700 hover:bg-gray-100 hover:text-gray-900 dark:text-gray-300 dark:hover:bg-gray-800 dark:hover:text-gray-100"
          >
            View as Candidate
          </Link>
        </nav>
      </aside>

      {/* Content area */}
      <main className="flex-1 px-8 py-8">
        {children}
      </main>
    </div>
  );
}
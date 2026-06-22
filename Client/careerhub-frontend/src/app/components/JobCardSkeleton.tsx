export function JobCardSkeleton() {
  return (
    <div
      className="
        animate-pulse rounded-lg border p-4
        bg-white border-gray-200
        dark:bg-gray-900 dark:border-gray-700
      "
    >
      {/* HEADER: title + badge */}
      <div className="mb-3 flex items-start justify-between gap-2">
        <div className="h-5 w-2/3 rounded bg-gray-300 dark:bg-gray-700" />
        <div className="h-5 w-20 rounded bg-gray-300 dark:bg-gray-700" />
      </div>

      {/* COMPANY + LOCATION */}
      <div className="mb-3 h-4 w-1/2 rounded bg-gray-300 dark:bg-gray-700" />

      {/* SALARY */}
      <div className="mb-3 h-4 w-1/3 rounded bg-gray-300 dark:bg-gray-700" />

      {/* FOOTER */}
      <div className="flex items-center gap-3">
        <div className="h-4 w-20 rounded bg-gray-300 dark:bg-gray-700" />
        <div className="h-4 w-16 rounded bg-gray-300 dark:bg-gray-700" />
        <div className="h-4 w-24 rounded bg-gray-300 dark:bg-gray-700" />
      </div>
    </div>
  );
}

export function JobListSkeleton() {
  return (
    <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
      {Array.from({ length: 6 }).map((_, index) => (
        <JobCardSkeleton key={index} />
      ))}
    </div>
  );
}
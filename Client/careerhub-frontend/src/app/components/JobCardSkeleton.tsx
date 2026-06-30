// Mirrors JobLinkCard's exact layout: p-4 padding, rounded-lg border, a
// left column with three stacked text lines (title / company / location)
// and a right column with two pill-shaped badges. Dimensions are matched
// line-by-line against JobLinkCard rather than approximated, so swapping
// a skeleton for a real card causes no layout shift.
export default function JobCardSkeleton() {
  return (
    <div
      aria-hidden="true"
      className="rounded-lg border border-gray-200 bg-white p-4 shadow-sm dark:border-gray-700 dark:bg-gray-900"
    >
      <div className="flex items-start justify-between gap-4">
        {/* Left column — mirrors title (h2, text-sm), company (text-sm), location (text-xs) */}
        <div className="space-y-2">
          {/* title line — text-sm font-semibold height ≈ 14px line, slightly wider */}
          <div className="h-3.5 w-36 animate-pulse rounded bg-gray-200 dark:bg-gray-700" />
          {/* company line — text-sm, shorter */}
          <div className="h-3.5 w-24 animate-pulse rounded bg-gray-200 dark:bg-gray-700" />
          {/* location line — text-xs, shortest */}
          <div className="h-3 w-20 animate-pulse rounded bg-gray-200 dark:bg-gray-700" />
        </div>

        {/* Right column — mirrors the two JobStatusBadge pills */}
        <div className="flex shrink-0 flex-col items-end gap-2">
          <div className="h-5 w-16 animate-pulse rounded-full bg-gray-200 dark:bg-gray-700" />
          <div className="h-5 w-12 animate-pulse rounded-full bg-gray-200 dark:bg-gray-700" />
        </div>
      </div>
    </div>
  );
}
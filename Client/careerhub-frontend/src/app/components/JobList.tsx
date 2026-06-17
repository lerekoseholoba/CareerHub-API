import type { JobListing } from "../types";
import JobCard from "./JobCard";

interface JobsGridProps {
  jobs: JobListing[];
  selectedId: string | null;
  onSelect: (id: string) => void;
}

export default function JobsGrid({
  jobs,
  selectedId,
  onSelect,
}: JobsGridProps) {
  return (
    <div className="space-y-4">
      {/* Result count */}
      <div className="text-sm text-gray-600 dark:text-gray-400">
        Showing {jobs.length} job{jobs.length !== 1 ? "s" : ""}
      </div>

      {/* Empty state */}
      {jobs.length === 0 ? (
        <div className="rounded-lg border border-dashed border-gray-300 dark:border-gray-700 p-8 text-center">
          <h3 className="text-lg font-semibold text-gray-800 dark:text-gray-100">
            No CareerHub listings found
          </h3>

          <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
            There are currently no active job postings matching your criteria.
            Please check back later or adjust your filters.
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
          {jobs.map((job) => (
            <JobCard
              key={job.id}
              job={job}
              isSelected={selectedId === job.id}
              onSelect={onSelect}
            />
          ))}
        </div>
      )}
    </div>
  );
}
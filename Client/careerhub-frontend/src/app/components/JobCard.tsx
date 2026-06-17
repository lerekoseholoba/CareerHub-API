import { cn } from "../lib/utils";
import type { JobListing } from "../types";
import JobStatusBadge from "./JobStatusBadge";

interface JobCardProps {
  job: JobListing;
  isSelected: boolean;
  onSelect: (id: string) => void;
}

function formatSalary(min: number, max: number): string {
  const formatter = new Intl.NumberFormat("en-ZA");
  return `R${formatter.format(min)} – R${formatter.format(max)} pm`;
}

function getRelativeDate(postedDate: string): string {
  const posted = new Date(postedDate);
  const now = new Date();

  const diffMs = now.getTime() - posted.getTime();
  const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));

  if (diffDays <= 0) return "Today";
  if (diffDays === 1) return "1 day ago";
  if (diffDays < 30) return `${diffDays} days ago`;

  const months = Math.floor(diffDays / 30);

  if (months === 1) return "1 month ago";
  if (months < 12) return `${months} months ago`;

  const years = Math.floor(months / 12);

  return years === 1 ? "1 year ago" : `${years} years ago`;
}

export default function JobCard({
  job,
  isSelected,
  onSelect,
}: JobCardProps) {
  return (
    <div
      onClick={() => onSelect(job.id)}
      className={cn(
        "cursor-pointer rounded-lg border p-4 transition-all",
        "bg-white border-gray-200 hover:border-gray-300 hover:shadow-sm",
        "dark:bg-gray-900 dark:border-gray-700 dark:hover:border-gray-600",
        isSelected &&
          "border-blue-600 bg-blue-50 shadow-md dark:border-blue-400 dark:bg-blue-950 dark:shadow-lg",
        !job.isOpen && "opacity-70"
      )}
    >
      <div className="mb-3 flex items-start justify-between gap-2">
        <h2 className="text-lg font-semibold text-gray-900 dark:text-gray-100">
          {job.title}
        </h2>

        <JobStatusBadge employmentType={job.jobType} />
      </div>

      <p className="mb-3 text-sm text-gray-600 dark:text-gray-400">
        {job.company} · {job.location}
      </p>

      <p className="mb-3 font-medium text-gray-900 dark:text-gray-100">
        {formatSalary(job.salaryMin, job.salaryMax)}
      </p>

      <div className="flex flex-wrap items-center gap-3 text-sm text-gray-500 dark:text-gray-400">
        <span>{getRelativeDate(job.postedDate)}</span>

        <JobStatusBadge isActive={job.isOpen} />

        {job.applicantCount > 0 && (
          <span>
            {job.applicantCount} applicant
            {job.applicantCount !== 1 ? "s" : ""}
          </span>
        )}
      </div>
    </div>
  );
}
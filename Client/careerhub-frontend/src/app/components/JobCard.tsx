import type { JobListing, JobType } from "../types";

interface JobCardProps {
  job: JobListing;
  isSelected: boolean;
  onSelect: (id: string) => void;
}

const badgeStyles: Record<JobType, string> = {
  FullTime: "bg-green-100 text-green-800",
  PartTime: "bg-blue-100 text-blue-800",
  Contract: "bg-orange-100 text-orange-800",
  Internship: "bg-purple-100 text-purple-800",
};

function formatSalary(min: number, max: number): string {
  const formatter = new Intl.NumberFormat("en-ZA");

  return `R${formatter.format(min)} – R${formatter.format(max)} pm`;
}

function getRelativeDate(postedDate: string): string {
  const posted = new Date(postedDate);
  const now = new Date();

  const diffMs = now.getTime() - posted.getTime();
  const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));

  if (diffDays <= 0) {
    return "Today";
  }

  if (diffDays === 1) {
    return "1 day ago";
  }

  if (diffDays < 30) {
    return `${diffDays} days ago`;
  }

  const months = Math.floor(diffDays / 30);

  if (months === 1) {
    return "1 month ago";
  }

  if (months < 12) {
    return `${months} months ago`;
  }

  const years = Math.floor(months / 12);

  if (years === 1) {
    return "1 year ago";
  }

  return `${years} years ago`;
}

export default function JobCard({
  job,
  isSelected,
  onSelect,
}: JobCardProps) {
  return (
    <div
      onClick={() => onSelect(job.id)}
      className={`cursor-pointer rounded-lg border p-4 transition-all ${
        isSelected
          ? "border-blue-600 bg-blue-50 shadow-md"
          : "border-gray-200 bg-white hover:border-gray-300 hover:shadow-sm"
      }`}
    >
      <div className="mb-3 flex items-start justify-between gap-2">
        <h2 className="text-lg font-semibold text-gray-900">
          {job.title}
        </h2>

        <span
          className={`rounded-full px-3 py-1 text-xs font-medium ${
            badgeStyles[job.jobType]
          }`}
        >
          {job.jobType}
        </span>
      </div>

      <p className="mb-3 text-sm text-gray-600">
        {job.company} · {job.location}
      </p>

      <p className="mb-3 font-medium text-gray-900">
        {formatSalary(job.salaryMin, job.salaryMax)}
      </p>

      <div className="flex flex-wrap items-center gap-3 text-sm text-gray-500">
        <span>{getRelativeDate(job.postedDate)}</span>

        {!job.isOpen && (
          <span className="rounded bg-red-100 px-2 py-1 font-medium text-red-700">
            Closed
          </span>
        )}

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
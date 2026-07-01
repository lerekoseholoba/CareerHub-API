import Link from "next/link";
import Image from "next/image";
import type { JobListing } from "../types";
import JobStatusBadge from "./JobStatusBadge";

type Props = {
  job: JobListing;
};

export default function JobLinkCard({ job }: Props) {
  return (
    <Link
      href={`/jobs/${job.id}`}
      className="block rounded-lg border border-gray-200 bg-white p-4 shadow-sm transition-colors hover:border-blue-300 hover:bg-blue-50 dark:border-gray-700 dark:bg-gray-900 dark:hover:border-blue-700 dark:hover:bg-gray-800"
    >
      <div className="flex items-start justify-between gap-4">
        <div className="flex items-start gap-3">
          <Image
            src="/company-logo-placeholder.svg"
            alt={`${job.company} logo`}
            width={40}
            height={40}
            className="rounded-md shrink-0"
          />

          <div className="space-y-1">
            <h2 className="text-sm font-semibold text-gray-900 dark:text-gray-100">
              {job.title}
            </h2>
            <p className="text-sm text-gray-600 dark:text-gray-400">
              {job.company}
            </p>
            <p className="text-xs text-gray-500 dark:text-gray-500">
              {job.location}
            </p>
          </div>
        </div>

        <div className="flex shrink-0 flex-col items-end gap-2">
          <JobStatusBadge employmentType={job.employmentType} />
          <JobStatusBadge isActive={job.isOpen} />
        </div>
      </div>
    </Link>
  );
}
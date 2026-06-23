// ─────────────────────────────────────────────────────────────────────────────
// ORIGINAL CLIENT COMPONENT (kept for reference — do not uncomment in production)
// ─────────────────────────────────────────────────────────────────────────────
//
// "use client";
//
// import { useState, useEffect } from "react";
// import { useQuery } from "@tanstack/react-query";
//
// import type { JobListing, PagedJobsResponse } from "./types";
//
// import JobList from "./components/JobList";
// import { JobListSkeleton } from "./components/JobCardSkeleton";
// import { fetchJobs } from "./lib/api";
// import ApplicationForm from "./components/ApplicationForm";
// import PostJobForm from "./components/PostJobForm";
//
// export default function Home() {
//   const [selectedId, setSelectedId] = useState<string | null>(null);
//   const [showPostForm, setShowPostForm] = useState(false);
//
//   const {
//     data,
//     isPending,
//     isError,
//     error,
//     refetch,
//   } = useQuery<PagedJobsResponse, Error>({
//     queryKey: ["jobs"],
//     queryFn: fetchJobs,
//   });
//
//   const jobs = data?.data ?? [];
//   const selectedJob = jobs.find((job) => job.id === selectedId) ?? null;
//
//   const handleSelect = (id: string) => {
//     setSelectedId((prev) => (prev === id ? null : id));
//   };
//
//   useEffect(() => {
//     const storedId = sessionStorage.getItem("selectedJobId");
//     if (storedId) setSelectedId(storedId);
//   }, []);
//
//   useEffect(() => {
//     if (selectedId) {
//       sessionStorage.setItem("selectedJobId", selectedId);
//     } else {
//       sessionStorage.removeItem("selectedJobId");
//     }
//   }, [selectedId]);
//
//   if (isPending) return <JobListSkeleton />;
//
//   if (isError) {
//     return (
//       <main className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-950 p-8">
//         <div className="max-w-md rounded-lg border border-red-300 bg-red-50 p-6 dark:border-red-700 dark:bg-red-950">
//           <h2 className="mb-2 text-lg font-semibold text-red-700 dark:text-red-300">
//             Failed to load jobs
//           </h2>
//           <p className="mb-4 text-sm text-red-600 dark:text-red-400">
//             {error.message}
//           </p>
//           <button
//             onClick={() => refetch()}
//             className="rounded bg-red-600 px-4 py-2 text-white hover:bg-red-700"
//           >
//             Try again
//           </button>
//         </div>
//       </main>
//     );
//   }
//
//   return (
//     <main className="min-h-screen space-y-6 bg-gray-50 p-8 text-gray-900 dark:bg-gray-950 dark:text-gray-100">
//
//       {/* ── Header ─────────────────────────────────────────────────────────── */}
//       <div className="flex items-center justify-between">
//         <h1 className="text-3xl font-bold">CareerHub Job Listings</h1>
//         <button
//           onClick={() => setShowPostForm((prev) => !prev)}
//           className="rounded-md bg-blue-600 px-4 py-2 text-sm font-semibold text-white transition-colors hover:bg-blue-700 active:bg-blue-800 dark:bg-blue-500 dark:hover:bg-blue-600"
//         >
//           {showPostForm ? "Cancel" : "Post a Job"}
//         </button>
//       </div>
//
//       {/* ── Post Job Form (toggle) ──────────────────────────────────────────── */}
//       {showPostForm && <PostJobForm />}
//
//       <p className="text-sm text-gray-500">
//         Showing {data?.totalCount ?? 0} jobs
//       </p>
//
//       {/* ── Selection panel ────────────────────────────────────────────────── */}
//       {selectedJob && (
//         <div className="rounded-lg border border-gray-200 bg-white p-4 shadow-sm dark:border-gray-700 dark:bg-gray-900">
//           <h2 className="text-lg font-semibold">{selectedJob.title}</h2>
//           <p className="text-sm text-gray-600 dark:text-gray-400">
//             {selectedJob.company}
//           </p>
//         </div>
//       )}
//
//       {/* ── Job list ───────────────────────────────────────────────────────── */}
//       <JobList
//         jobs={jobs}
//         selectedId={selectedId}
//         onSelect={handleSelect}
//       />
//
//       {/* ── Application form ───────────────────────────────────────────────── */}
//       {!isPending && !isError && selectedJob !== null && (
//         <ApplicationForm
//           jobId={selectedJob.id}
//           jobTitle={selectedJob.title}
//         />
//       )}
//     </main>
//   );
// }

// ─────────────────────────────────────────────────────────────────────────────
// NEW SERVER COMPONENT LANDING PAGE
// No "use client" — no useState, useEffect, or useQuery.
// This page ships zero JS of its own; navigation is handled by next/link.
// ─────────────────────────────────────────────────────────────────────────────

import Link from "next/link";

export default function Home() {
  return (
    <main className="flex min-h-screen flex-col items-center justify-center bg-gray-50 p-8 text-gray-900 dark:bg-gray-950 dark:text-gray-100">
      <div className="max-w-xl space-y-6 text-center">
        <h1 className="text-4xl font-bold">Welcome to CareerHub</h1>
        <p className="text-base text-gray-600 dark:text-gray-400">
          CareerHub connects candidates with opportunities and gives employers
          the tools to post and manage listings in one place.
        </p>

        <div className="flex items-center justify-center gap-4">
          <Link
            href="/jobs"
            className="rounded-md bg-blue-600 px-5 py-2.5 text-sm font-semibold text-white transition-colors hover:bg-blue-700 active:bg-blue-800 dark:bg-blue-500 dark:hover:bg-blue-600"
          >
            Browse Jobs
          </Link>
          <Link
            href="/dashboard/listings"
            className="rounded-md border border-gray-300 bg-white px-5 py-2.5 text-sm font-semibold text-gray-700 transition-colors hover:bg-gray-100 dark:border-gray-600 dark:bg-gray-800 dark:text-gray-200 dark:hover:bg-gray-700"
          >
            Employer Dashboard
          </Link>
        </div>
      </div>
    </main>
  );
}
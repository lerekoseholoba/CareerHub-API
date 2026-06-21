"use client";

import { useState, useEffect } from "react";
import { useQuery } from "@tanstack/react-query";

import type { JobListing } from "./types";
import JobList from "./components/JobList";
import { fetchJobs } from "./lib/api";

export default function Home() {
  const [selectedId, setSelectedId] = useState<string | null>(null);

  const {
    data: jobs = [],
    isLoading,
  } = useQuery<JobListing[]>({
    queryKey: ["jobs"],

    // TEMPORARY for testing the skeleton
    //queryFn: () => new Promise(() => {}),

    // Restore this before submitting:
     queryFn: fetchJobs,
  });

  const selectedJob =
    jobs.find((job) => job.id === selectedId) || null;

  const handleSelect = (id: string) => {
    setSelectedId((prev) => (prev === id ? null : id));
  };

  /* Restore selection on mount */
  useEffect(() => {
    const storedId = sessionStorage.getItem("selectedJobId");

    if (!storedId) return;

    const exists = jobs.some((job) => job.id === storedId);

    if (exists) {
      setSelectedId(storedId);
    }
  }, [jobs]);

  /* Sync selection */
  useEffect(() => {
    if (selectedId) {
      sessionStorage.setItem("selectedJobId", selectedId);
    } else {
      sessionStorage.removeItem("selectedJobId");
    }
  }, [selectedId]);

  return (
    <main className="min-h-screen space-y-6 bg-gray-50 p-8 text-gray-900 dark:bg-gray-950 dark:text-gray-100">
      <h1 className="mb-6 text-3xl font-bold">
        ConferenceHub
      </h1>

      {selectedJob && (
        <div
          className="
            rounded-lg border border-gray-200 bg-white p-4 shadow-sm
            dark:border-gray-700 dark:bg-gray-900
          "
        >
          <h2 className="text-lg font-semibold">
            {selectedJob.title}
          </h2>

          <p className="text-sm text-gray-600 dark:text-gray-400">
            {selectedJob.company}
          </p>
        </div>
      )}

      <JobList
        jobs={jobs}
        selectedId={selectedId}
        onSelect={handleSelect}
        isLoading={isLoading}
      />
    </main>
  );
}
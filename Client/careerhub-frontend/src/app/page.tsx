"use client";

import { useState, useEffect } from "react";
import type { JobListing } from "./types";
import JobList from "./components/JobList";

export default function Home() {
  const [selectedId, setSelectedId] = useState<string | null>(null);

  const jobs: JobListing[] = [
    {
      id: "a1",
      title: "Frontend Developer",
      company: "Capitec Bank",
      location: "Cape Town",
      jobType: "FullTime",
      salaryMin: 35000,
      salaryMax: 55000,
      postedDate: new Date().toISOString(),
      isOpen: true,
      applicantCount: 12,
    },
    {
      id: "a2",
      title: "Backend Engineer",
      company: "Takealot",
      location: "Remote",
      jobType: "FullTime",
      salaryMin: 45000,
      salaryMax: 70000,
      postedDate: new Date().toISOString(),
      isOpen: true,
      applicantCount: 0,
    },
    {
      id: "a3",
      title: "Data Analyst",
      company: "Discovery",
      location: "Johannesburg",
      jobType: "Contract",
      salaryMin: 30000,
      salaryMax: 50000,
      postedDate: new Date(Date.now() - 10 * 86400000).toISOString(),
      isOpen: true,
      applicantCount: 6,
    },
    {
      id: "a4",
      title: "UX Designer",
      company: "Nedbank",
      location: "Sandton",
      jobType: "PartTime",
      salaryMin: 25000,
      salaryMax: 40000,
      postedDate: new Date(Date.now() - 40 * 86400000).toISOString(),
      isOpen: true,
      applicantCount: 3,
    },
    {
      id: "a5",
      title: "DevOps Engineer",
      company: "Standard Bank",
      location: "Johannesburg",
      jobType: "FullTime",
      salaryMin: 60000,
      salaryMax: 90000,
      postedDate: new Date(Date.now() - 3 * 86400000).toISOString(),
      isOpen: false,
      applicantCount: 18,
    },
    {
      id: "a6",
      title: "Software Intern",
      company: "FNB",
      location: "Remote",
      jobType: "Internship",
      salaryMin: 8000,
      salaryMax: 12000,
      postedDate: new Date(Date.now() - 60 * 86400000).toISOString(),
      isOpen: true,
      applicantCount: 0,
    },
  ];

  const selectedJob =
    jobs.find((job) => job.id === selectedId) || null;

  const handleSelect = (id: string) => {
    setSelectedId((prev) => (prev === id ? null : id));
  };

  /*
    EFFECT 1: RESTORE SELECTION ON MOUNT

    Runs ONLY once when the component first loads.

    Dependency array: []
    Reason:
    - We only want to read sessionStorage once on mount.
    - If we included dependencies, we could overwrite user selection changes.
    - This ensures we restore previous session state safely.

    Behaviour:
    - Reads "selectedJobId" from sessionStorage
    - Checks if the ID still exists in current jobs list
    - Restores selection ONLY if valid
    - Ignores stale IDs silently (as required)
  */
  useEffect(() => {
    const storedId = sessionStorage.getItem("selectedJobId");

    if (!storedId) return;

    const exists = jobs.some((job) => job.id === storedId);

    if (exists) {
      setSelectedId(storedId);
    }
  }, []);

  /*
    EFFECT 2: SYNC SELECTION TO SESSION STORAGE

    Runs every time selectedId changes.

    Dependency array: [selectedId]
    Reason:
    - We only want to sync storage when user changes selection.
    - Including other dependencies (like jobs) would cause unnecessary writes.
    - This keeps sessionStorage aligned with current UI state.

    Behaviour:
    - If a job is selected → save ID to sessionStorage
    - If selection is cleared → remove key entirely
  */
  useEffect(() => {
    if (selectedId) {
      sessionStorage.setItem("selectedJobId", selectedId);
    } else {
      sessionStorage.removeItem("selectedJobId");
    }
  }, [selectedId]);

  return (
    <main className="p-8 space-y-6">
      {/* Header (unchanged) */}
      <h1 className="text-3xl font-bold mb-6">
        ConferenceHub
      </h1>

      {/* Summary panel (must fully unmount when null) */}
      {selectedJob && (
        <div className="rounded-lg border p-4 bg-white shadow-sm">
          <h2 className="text-lg font-semibold">
            {selectedJob.title}
          </h2>
          <p className="text-sm text-gray-600">
            {selectedJob.company}
          </p>
        </div>
      )}

      {/* Job list */}
      <JobList
        jobs={jobs}
        selectedId={selectedId}
        onSelect={handleSelect}
      />
    </main>
  );
}
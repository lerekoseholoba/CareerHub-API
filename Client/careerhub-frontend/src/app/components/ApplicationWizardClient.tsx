"use client";

import dynamic from "next/dynamic";

const ApplicationWizard = dynamic(
  () =>
    import("./ApplicationWizard").then((mod) => ({
      default: mod.ApplicationWizard,
    })),
  {
    ssr: false,
    loading: () => (
      <div className="h-96 w-full animate-pulse rounded-lg border border-gray-200 bg-gray-100 dark:border-gray-700 dark:bg-gray-800" />
    ),
  }
);

export default ApplicationWizard;
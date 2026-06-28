"use client";

import { useDashboardStore } from "@/stores/dashboardStore";

export default function DashboardToolbar() {
  // One useStore call per value — not destructuring
  const view             = useDashboardStore((s) => s.view);
  const setView          = useDashboardStore((s) => s.setView);
  const showClosedJobs   = useDashboardStore((s) => s.showClosedJobs);
  const toggleShowClosed = useDashboardStore((s) => s.toggleShowClosedJobs);

  return (
    <div className="flex items-center gap-4">

      {/* View toggle */}
      <div className="flex rounded-lg border border-gray-300 dark:border-gray-600 overflow-hidden text-sm">
        {(["table", "grid"] as const).map((v) => (
          <button
            key={v}
            type="button"
            onClick={() => setView(v)}
            className={`px-4 py-2 capitalize transition-colors ${
              view === v
                ? "bg-blue-600 text-white"
                : "bg-transparent text-gray-600 hover:bg-gray-100 dark:text-gray-400 dark:hover:bg-gray-800"
            }`}
          >
            {v === "table" ? "Table" : "Grid"}
          </button>
        ))}
      </div>

      {/* Show closed jobs checkbox */}
      <label className="flex items-center gap-2 text-sm text-gray-600 dark:text-gray-400 cursor-pointer select-none">
        <input
          type="checkbox"
          checked={showClosedJobs}
          onChange={toggleShowClosed}
          className="rounded border-gray-300 dark:border-gray-600"
        />
        Show closed jobs
      </label>

    </div>
  );
}
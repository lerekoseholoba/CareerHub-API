"use client";

import { useQueryStates, parseAsString } from "nuqs";
import { useState, useEffect } from "react";

export default function JobFilters() {
  const [filters, setFilters] = useQueryStates({
    q:        parseAsString.withDefault(""),
    location: parseAsString.withDefault(""),
    status:   parseAsString.withDefault("all"),
  });

  // Local state for debounced inputs
  const [keywordInput, setKeywordInput]   = useState(filters.q);
  const [locationInput, setLocationInput] = useState(filters.location);

  // Debounce keyword — 300ms
  useEffect(() => {
    const timer = setTimeout(() => {
      setFilters({ q: keywordInput });
    }, 300);
    return () => clearTimeout(timer);
  }, [keywordInput]);

  // Debounce location — 300ms
  useEffect(() => {
    const timer = setTimeout(() => {
      setFilters({ location: locationInput });
    }, 300);
    return () => clearTimeout(timer);
  }, [locationInput]);

  return (
    <div className="flex flex-col gap-4 rounded-lg border border-gray-200 bg-white p-4 shadow-sm dark:border-gray-700 dark:bg-gray-900 sm:flex-row sm:items-end">

      {/* Keyword */}
      <div className="flex flex-col gap-1 flex-1">
        <label className="text-xs font-medium text-gray-600 dark:text-gray-400">
          Keyword
        </label>
        <input
          type="text"
          value={keywordInput}
          onChange={(e) => setKeywordInput(e.target.value)}
          placeholder="e.g. engineer"
          className="rounded-lg border border-gray-300 px-3 py-2 text-sm bg-transparent focus:outline-none focus:ring-2 focus:ring-blue-500 dark:border-gray-600"
        />
      </div>

      {/* Location */}
      <div className="flex flex-col gap-1 flex-1">
        <label className="text-xs font-medium text-gray-600 dark:text-gray-400">
          Location
        </label>
        <input
          type="text"
          value={locationInput}
          onChange={(e) => setLocationInput(e.target.value)}
          placeholder="e.g. Cape Town"
          className="rounded-lg border border-gray-300 px-3 py-2 text-sm bg-transparent focus:outline-none focus:ring-2 focus:ring-blue-500 dark:border-gray-600"
        />
      </div>

      {/* Status */}
      <div className="flex flex-col gap-1">
        <label className="text-xs font-medium text-gray-600 dark:text-gray-400">
          Status
        </label>
        <div className="flex rounded-lg border border-gray-300 dark:border-gray-600 overflow-hidden text-sm">
          {(["all", "open"] as const).map((val) => (
            <button
              key={val}
              type="button"
              onClick={() => setFilters({ status: val })}
              className={`px-4 py-2 capitalize transition-colors ${
                filters.status === val
                  ? "bg-blue-600 text-white"
                  : "bg-transparent text-gray-600 hover:bg-gray-100 dark:text-gray-400 dark:hover:bg-gray-800"
              }`}
            >
              {val}
            </button>
          ))}
        </div>
      </div>

    </div>
  );
}
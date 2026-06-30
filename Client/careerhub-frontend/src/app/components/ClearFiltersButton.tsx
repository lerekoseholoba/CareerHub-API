"use client";

import { useQueryStates, parseAsString } from "nuqs";

export default function ClearFiltersButton() {
  const [, setFilters] = useQueryStates({
    q:        parseAsString.withDefault(""),
    location: parseAsString.withDefault(""),
    status:   parseAsString.withDefault("all"),
  });

  function handleClear() {
    // Setting back to defaults removes the params from the URL via nuqs
    // (withDefault means the default value isn't serialized), which is
    // exactly the "reset" behavior nuqs is meant to provide.
    setFilters({ q: "", location: "", status: "all" });
  }

  return (
    <button
      type="button"
      onClick={handleClear}
      className="mt-4 rounded-md bg-blue-600 px-4 py-2 text-sm font-medium text-white transition-colors hover:bg-blue-700"
    >
      Clear all filters
    </button>
  );
}
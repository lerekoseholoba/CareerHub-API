import JobCardSkeleton from "../components/JobCardSkeleton"

export default function JobsLoading() {
  return (
    <main className="min-h-screen bg-gray-50 p-8 text-gray-900 dark:bg-gray-950 dark:text-gray-100">
      <div className="mx-auto max-w-4xl space-y-6">
        <h1 className="text-3xl font-bold">Job Listings</h1>

        {/*
          The filter bar itself isn't part of the data fetch (it's static
          chrome), but rendering a skeleton-shaped placeholder for it here
          would require duplicating JobFilters' exact layout. Since
          JobFilters has no async dependency, the simplest correct choice is
          to omit it from the loading state entirely — it mounts instantly
          alongside the real page once the Suspense boundary resolves.
          What matters for "no blank page / no spinner" is the job list
          area, which the skeletons cover.
        */}

        {/*
          6 skeleton cards: see README for the full justification. Short
          version — it roughly fills one viewport of a 2-column grid on
          common screen sizes without forcing scroll before any real content
          is visible, and reads clearly as "a list is coming" rather than
          implying a specific result count.
        */}
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          {Array.from({ length: 6 }).map((_, i) => (
            <JobCardSkeleton key={i} />
          ))}
        </div>
      </div>
    </main>
  );
}
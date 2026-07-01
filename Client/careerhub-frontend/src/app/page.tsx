import Link from "next/link";
import Image from "next/image";

export default function Home() {
  return (
    <main className="flex min-h-screen flex-col items-center justify-center bg-gray-50 p-8 text-gray-900 dark:bg-gray-950 dark:text-gray-100">
      <div className="max-w-xl space-y-6 text-center">
        <Image
          src="/hero.svg"
          alt="Illustration of CareerHub connecting candidates and employers"
          width={1200}
          height={400}
          priority
          className="w-full h-auto rounded-lg"
        />

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
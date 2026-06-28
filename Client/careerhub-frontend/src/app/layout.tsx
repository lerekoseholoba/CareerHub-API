import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import Link from "next/link";
import ThemeToggle from "./components/ThemeToggle";
import Providers from "./providers";
import { auth, signOut } from "@/auth";

const geistSans = Geist({ variable: "--font-geist-sans", subsets: ["latin"] });
const geistMono = Geist_Mono({ variable: "--font-geist-mono", subsets: ["latin"] });

export const metadata: Metadata = {
  title: "CareerHub",
  description: "Job listing application",
};

export default async function RootLayout({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  const session = await auth();
  const role = session?.user?.role;

  return (
    <html lang="en" className={`${geistSans.variable} ${geistMono.variable} h-full antialiased`}>
      <body className="min-h-full flex flex-col bg-background text-foreground">
        <header className="flex items-center justify-between px-6 py-4 border-b bg-white text-gray-900 border-gray-200 dark:bg-gray-900 dark:text-gray-100 dark:border-gray-700">
          <Link href="/" className="text-lg font-bold hover:text-blue-600 dark:hover:text-blue-400 transition-colors">
            CareerHub
          </Link>

          <div className="flex items-center gap-6">
            <nav className="flex items-center gap-4">
              {/* Jobs link — candidates and signed-out users only */}
              {role !== "employer" && (
                <Link href="/jobs" className="text-sm font-medium text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100 transition-colors">
                  Jobs
                </Link>
              )}

              {/* Dashboard link — employers only */}
              {role === "employer" && (
                <Link href="/dashboard/listings" className="text-sm font-medium text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100 transition-colors">
                  Dashboard
                </Link>
              )}
            </nav>

            {session ? (
              <div className="flex items-center gap-3">
                <span className="text-sm text-gray-600 dark:text-gray-400">
                  {session.user?.name}
                </span>
                <span className={`text-xs px-2 py-0.5 rounded-full font-medium ${
                  role === "employer"
                    ? "bg-purple-100 dark:bg-purple-900 text-purple-700 dark:text-purple-300"
                    : "bg-blue-100 dark:bg-blue-900 text-blue-700 dark:text-blue-300"
                }`}>
                  {role}
                </span>
                <form action={async () => { "use server"; await signOut({ redirectTo: "/" }); }}>
                  <button type="submit" className="text-sm text-gray-500 hover:text-red-600 dark:hover:text-red-400 transition-colors">
                    Sign out
                  </button>
                </form>
              </div>
            ) : (
              <Link href="/login" className="text-sm font-medium text-blue-600 hover:text-blue-700 dark:text-blue-400 transition-colors">
                Sign in
              </Link>
            )}

            <ThemeToggle />
          </div>
        </header>

        <main className="flex-1">
          <Providers>{children}</Providers>
        </main>
      </body>
    </html>
  );
}
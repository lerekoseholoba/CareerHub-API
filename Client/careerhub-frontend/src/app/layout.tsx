import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import Link from "next/link";
import ThemeToggle from "./components/ThemeToggle";
import Providers from "./providers";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "CareerHub",
  description: "Job listing application",
};

export default function RootLayout({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  return (
    <html
      lang="en"
      className={`${geistSans.variable} ${geistMono.variable} h-full antialiased`}
    >
      <body className="min-h-full flex flex-col bg-background text-foreground">
        <header
          className="
            flex items-center justify-between px-6 py-4 border-b
            bg-white text-gray-900 border-gray-200
            dark:bg-gray-900 dark:text-gray-100 dark:border-gray-700
          "
        >
          <Link
            href="/"
            className="text-lg font-bold hover:text-blue-600 dark:hover:text-blue-400 transition-colors"
          >
            CareerHub
          </Link>

          <div className="flex items-center gap-6">
            <nav className="flex items-center gap-4">
              <Link
                href="/jobs"
                className="text-sm font-medium text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100 transition-colors"
              >
                Jobs
              </Link>
              <Link
                href="/dashboard"
                className="text-sm font-medium text-gray-600 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-100 transition-colors"
              >
                Dashboard
              </Link>
            </nav>

            <ThemeToggle />
          </div>
        </header>

        <main className="flex-1">
          <Providers>
            {children}
          </Providers>
        </main>
      </body>
    </html>
  );
}
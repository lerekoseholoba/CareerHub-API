import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import ThemeToggle from "./components/ThemeToggle";
import Providers from "./providers"; //  ADD THIS

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
        {/* HEADER WITH THEME TOGGLE */}
        <header
          className="
            flex items-center justify-between px-6 py-4 border-b
            bg-white text-gray-900 border-gray-200
            dark:bg-gray-900 dark:text-gray-100 dark:border-gray-700
          "
        >
          <h1 className="text-lg font-bold">CareerHub</h1>

          <ThemeToggle />
        </header>

        {/* PAGE CONTENT */}
        <main className="flex-1">
          {/*  React Query + providers boundary */}
          <Providers>
            {children}
          </Providers>
        </main>
      </body>
    </html>
  );
}
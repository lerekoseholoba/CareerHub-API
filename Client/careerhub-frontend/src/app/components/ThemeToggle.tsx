"use client";

import { useEffect, useState } from "react";

type Theme = "light" | "dark";

export default function ThemeToggle() {
  const [theme, setTheme] = useState<Theme>("light");

  // 1. On mount: load saved preference OR system preference
  useEffect(() => {
    const stored = localStorage.getItem("theme") as Theme | null;

    if (stored) {
      setTheme(stored);
      document.documentElement.classList.toggle("dark", stored === "dark");
      return;
    }

    const systemPrefersDark = window.matchMedia(
      "(prefers-color-scheme: dark)"
    ).matches;

    const initialTheme = systemPrefersDark ? "dark" : "light";

    setTheme(initialTheme);
    document.documentElement.classList.toggle(
      "dark",
      initialTheme === "dark"
    );
  }, []);

  // 2. Toggle theme
  const toggleTheme = () => {
    const newTheme: Theme = theme === "dark" ? "light" : "dark";

    setTheme(newTheme);

    document.documentElement.classList.toggle("dark", newTheme === "dark");

    localStorage.setItem("theme", newTheme);
  };

  return (
    <button
      onClick={toggleTheme}
      aria-label={`Switch to ${theme === "dark" ? "light" : "dark"} mode`}
      className="rounded-md border px-3 py-1 text-sm transition-colors
                 border-gray-300 text-gray-800 bg-white
                 dark:border-gray-600 dark:text-gray-100 dark:bg-gray-900"
    >
      {theme === "dark" ? "Dark Mode" : "Light Mode"}
    </button>
  );
}
import { defineConfig } from "vitest/config";
import react from "@vitejs/plugin-react";
import path from "path";

export default defineConfig({
  plugins: [react()],
  test: {
    environment: "jsdom",
    globals: true,
    setupFiles: ["./src/test/setup.ts"],
    include: ["src/**/*.test.{ts,tsx}"],
    env: {
      NEXT_PUBLIC_API_URL: "http://localhost:3000",
    },
  },
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
    // Forces Node's module resolution condition for packages with
    // conditional exports (like msw), overriding jsdom's default
    // "browser" condition — without this, msw/node/package.json's
    // "browser": null entry silently breaks the import.
    conditions: ["node"],
  },
});
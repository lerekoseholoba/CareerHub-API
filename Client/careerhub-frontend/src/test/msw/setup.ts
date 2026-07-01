import "@testing-library/jest-dom";
import { server } from "./server";
import { beforeAll, afterEach, afterAll } from "vitest";

server.events.on("request:start", ({ request }) => {
  console.log("[MSW] intercepted:", request.method, request.url);
});
server.events.on("request:unhandled", ({ request }) => {
  console.log("[MSW] unhandled (no matching handler):", request.method, request.url);
});

beforeAll(() => server.listen({ onUnhandledRequest: "warn" }));
afterEach(() => server.resetHandlers());
afterAll(() => server.close());
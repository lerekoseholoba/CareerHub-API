import { http, HttpResponse } from "msw";

export const handlers = [
  // A relative path (no protocol/origin) matches requests to that path
  // regardless of which origin the app actually requested — this is the
  // documented MSW v2 behavior, and avoids any env-var read-timing issues
  // since there's no origin string to resolve at module-import time.
  http.post("/api/applications", async () => {
    return HttpResponse.json(
      {
        id: "app-001",
        jobId: "job-1",
        email: "jane@example.com",
        submittedAt: new Date().toISOString(),
      },
      { status: 201 }
    );
  }),

  http.get("/api/v1/Jobs", () => {
    return HttpResponse.json({
      data: [],
      page: 1,
      pageSize: 10,
      totalCount: 0,
      totalPages: 0,
      hasNextPage: false,
      hasPreviousPage: false,
    });
  }),

  http.patch("/api/jobs/:jobId", ({ params }) => {
    return HttpResponse.json({
      id: params.jobId,
      title: "Senior Backend Engineer",
      status: "Closed",
    });
  }),
];
import { NextResponse } from "next/server";
import { jobs } from "../../jobs/data";

const applicationCounts: Record<string, number> = {
  a1: 12,
  a2: 4,
  a3: 6,
  a4: 3,
  a5: 18,
  a6: 1,
};

export async function GET() {
  const stats = jobs.map((job) => ({
    jobId: job.id,
    applicationCount: applicationCounts[job.id] ?? 0,
  }));

  return NextResponse.json(stats);
}

export async function POST() {
  return NextResponse.json(
    {
      title: "Method Not Allowed",
      detail: "This endpoint only accepts GET requests.",
      status: 405,
    },
    { status: 405, headers: { Allow: "GET" } }
  );
}
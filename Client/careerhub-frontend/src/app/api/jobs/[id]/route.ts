import { NextResponse } from "next/server";
import { jobs } from "../data";

export async function GET(
  _request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  const job = jobs.find((j) => j.id === id);

  if (!job) {
    return NextResponse.json(
      {
        title: "Not Found",
        detail: `No job with id '${id}' exists.`,
        status: 404,
      },
      { status: 404 }
    );
  }

  return NextResponse.json(job);
}

export async function PATCH(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;

  const jobIndex = jobs.findIndex((j) => j.id === id);

  if (jobIndex === -1) {
    return NextResponse.json(
      {
        title: "Not Found",
        detail: `No job with id '${id}' exists.`,
        status: 404,
      },
      { status: 404 }
    );
  }

  const body = await request.json() as { status?: string };

  if (body.status === undefined || body.status === null) {
    return NextResponse.json(
      {
        title: "Bad Request",
        detail: "Request body must include a 'status' field.",
        status: 400,
      },
      { status: 400 }
    );
  }

  // Mutate the module-level array — persists for the duration of the server process
  jobs[jobIndex] = {
    ...jobs[jobIndex],
    isOpen: body.status === "open",
  };

  return NextResponse.json(jobs[jobIndex]);
}

export async function POST() {
  return NextResponse.json(
    {
      title: "Method Not Allowed",
      detail: "This endpoint only accepts GET and PATCH requests.",
      status: 405,
    },
    { status: 405, headers: { Allow: "GET, PATCH" } }
  );
}
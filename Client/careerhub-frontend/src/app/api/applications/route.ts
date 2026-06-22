import { NextResponse } from "next/server";

export const runtime = "nodejs"; // ensures crypto works properly

// GET must return 405
export async function GET() {
  return NextResponse.json(
    {
      title: "Method Not Allowed",
      detail: "GET is not supported on this endpoint",
      status: 405,
    },
    { status: 405 }
  );
}

// POST handler
export async function POST(req: Request) {
  try {
    const body = await req.json();

    const { jobId, email } = body;

    // simulate network latency (REQUIRED)
    await new Promise<void>((resolve) =>
      setTimeout(resolve, 800)
    );

    // validation: missing fields → 400 Problem Details
    if (!jobId || !email) {
      return NextResponse.json(
        {
          title: "Bad Request",
          detail: "jobId or email is missing",
          status: 400,
        },
        { status: 400 }
      );
    }

    // success response
    return NextResponse.json(
      {
        id: crypto.randomUUID(),
        jobId,
        email,
        submittedAt: new Date().toISOString(),
      },
      { status: 201 }
    );
  } catch (error) {
    // fallback error handler
    return NextResponse.json(
      {
        title: "Invalid Request",
        detail: "Request body is not valid JSON",
        status: 400,
      },
      { status: 400 }
    );
  }
}
"use server";

import { revalidateTag } from "next/cache";

/**
 * Discriminated union return type for action state
 */
export type CloseJobState =
  | { status: "success"; jobTitle: string }
  | { status: "error"; message: string }
  | null;

/**
 * Server Action: Close a job listing
 */
export async function closeJobListing(
  prevState: CloseJobState,
  formData: FormData
): Promise<CloseJobState> {
  const jobId = formData.get("jobId");

  // Validate input early (no network call)
  if (!jobId || typeof jobId !== "string") {
    return {
      status: "error",
      message: "Job ID is missing",
    };
  }

  const baseUrl = process.env.NEXT_PUBLIC_API_URL;

  if (!baseUrl) {
    return {
      status: "error",
      message: "API URL is not configured",
    };
  }

  try {
    const res = await fetch(
      `${baseUrl}/api/jobs/${jobId}`,
      {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          status: "Closed",
        }),
      }
    );

    // Try to read response body safely
    const data = await res.json().catch(() => null);

    if (!res.ok) {
      return {
        status: "error",
        message: data?.detail || "Failed to close job listing",
      };
    }

    // Ensure we have job title from backend response
    const jobTitle = data?.title ?? "Unknown Job";

    //  This invalidates cached job data across app
    revalidateTag("jobs", "default");

    return {
      status: "success",
      jobTitle,
    };
  } catch (error: any) {
    return {
      status: "error",
      message: error?.message || "Unexpected server error",
    };
  }
}
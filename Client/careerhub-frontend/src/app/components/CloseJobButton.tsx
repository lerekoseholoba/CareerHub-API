"use client";

import { useActionState } from "react";
import { useTransition } from "react";
import { closeJobListing } from "../actions/closeJob";

type Props = {
  jobId: string;
  currentStatus: string;
};

type ActionState =
  | { status: "success"; jobTitle: string }
  | { status: "error"; message: string }
  | null;

export default function CloseJobButton({ jobId, currentStatus }: Props) {
  // Don’t render anything for already closed jobs
  if (currentStatus === "Closed") return null;

  const [state, formAction, isPending] = useActionState<
    ActionState,
    FormData
  >(closeJobListing, null);

  return (
    <div className="space-y-2">
      <form action={formAction}>
        <input type="hidden" name="jobId" value={jobId} />

        <button
          type="submit"
          disabled={isPending}
          className={`rounded px-3 py-1 text-sm font-medium text-white ${
            isPending
              ? "bg-gray-400 cursor-not-allowed"
              : "bg-red-600 hover:bg-red-700"
          }`}
        >
          {isPending ? "Closing…" : "Close Job"}
        </button>
      </form>

      {/* Success state */}
      {state?.status === "success" && (
        <p className="text-green-600 text-sm font-medium">
          ✔ {state.jobTitle} closed successfully
        </p>
      )}

      {/* Error state */}
      {state?.status === "error" && (
        <p className="text-red-600 text-sm">
          {state.message}
        </p>
      )}
    </div>
  );
}
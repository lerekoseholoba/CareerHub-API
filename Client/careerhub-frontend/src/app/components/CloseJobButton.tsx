// app/components/CloseJobButton.tsx
"use client";

import { useActionState, useEffect } from "react";
import { toast } from "sonner";
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
  if (currentStatus === "Closed") return null;

  const [state, formAction, isPending] = useActionState<ActionState, FormData>(
    closeJobListing,
    null
  );

  // Fire toast once whenever the server action settles.
  // The effect re-runs only when `state` reference changes (i.e. after each action).
  useEffect(() => {
    if (state?.status === "success") {
      toast.success(`"${state.jobTitle}" closed successfully`);
    } else if (state?.status === "error") {
      toast.error(state.message);
    }
  }, [state]);

  return (
    // Removed the wrapping <div className="space-y-2"> — no inline banners remain
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
    // ← Both inline <p> banners removed; toasts handle feedback now
  );
}
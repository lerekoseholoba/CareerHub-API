"use client";

import { useEffect, useState, useTransition } from "react";
import { toast } from "sonner";
import { closeJobListing, type CloseJobState } from "../actions/closeJob";
import {
  AlertDialog,
  AlertDialogContent,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogCancel,
  AlertDialogAction,
} from "./ui/alert-dialog"; // adjust path to match your shadcn install location

type Props = {
  jobId: string;
  currentStatus: string;
};

export default function CloseJobButton({ jobId, currentStatus }: Props) {
  // Don't render anything for already closed jobs
  if (currentStatus === "Closed") return null;

  const [open, setOpen] = useState(false);
  const [isPending, startTransition] = useTransition();
  const [state, setState] = useState<CloseJobState>(null);

  // Fire toast once whenever the action settles. Mirrors the previous
  // useActionState + useEffect pattern, just sourced from local state now
  // since useTransition doesn't give us an ActionState directly.
  useEffect(() => {
    if (state?.status === "success") {
      toast.success(`"${state.jobTitle}" closed successfully`);
    } else if (state?.status === "error") {
      toast.error(state.message);
    }
  }, [state]);

  function handleConfirm() {
    const formData = new FormData();
    formData.set("jobId", jobId);

    startTransition(async () => {
      const result = await closeJobListing(state, formData);
      setState(result);
      if (result?.status === "success") {
        setOpen(false);
      }
      // On error, the dialog stays open so the user can retry or cancel —
      // closing it would hide the toast's context and force them to
      // re-open the dialog to try again.
    });
  }

  return (
    <AlertDialog open={open} onOpenChange={setOpen}>
      <button
        type="button"
        onClick={() => setOpen(true)}
        disabled={isPending}
        className={`rounded px-3 py-1 text-sm font-medium text-white ${
          isPending ? "bg-gray-400 cursor-not-allowed" : "bg-red-600 hover:bg-red-700"
        }`}
      >
        {isPending ? "Closing…" : "Close Job"}
      </button>

      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Close this listing?</AlertDialogTitle>
          <AlertDialogDescription>
            This listing will be marked as closed and removed from the public
            jobs board. This cannot be undone.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          {/*
            This shadcn install's generated AlertDialogCancel/AlertDialogAction
            types make `variant` and `size` required props (no defaults),
            so they must be passed explicitly even though visually you'd
            normally rely on the component's built-in defaults.
          */}
          <AlertDialogCancel variant="outline" size="default" disabled={isPending}>
            Keep listing
          </AlertDialogCancel>
          {/*
            AlertDialogAction renders in a Radix portal, outside any <form>
            element — type="submit" would do nothing here even if this button
            were inside a <form>. Instead of submitting a form, onClick calls
            handleConfirm(), which wraps the Server Action call in
            startTransition() and manages the resulting state locally. This
            is the "Server Action + useTransition" approach: the action
            itself (closeJobListing) is untouched: only the place it's
            invoked from changed, from a <form action={...}> binding to a
            direct async call.
          */}
          <AlertDialogAction
            variant="default"
            size="default"
            onClick={handleConfirm}
            disabled={isPending}
            className="bg-red-600 text-white hover:bg-red-700 focus:ring-red-500"
          >
            {isPending ? "Closing…" : "Close listing"}
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  );
}
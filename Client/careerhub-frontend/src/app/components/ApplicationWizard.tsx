"use client";

import { useEffect, useRef, useState } from "react";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import Link from "next/link";
import { cn } from "../lib/utils";
import { submitApplication } from "../lib/api";
import type { ApplicationRequest } from "../types";
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

// ─────────────────────────────────────────────────────────────────────────
// Schema — covers exactly the fields the wizard collects per spec.
// (yearsOfExperience / availableImmediately / noticePeriodWeeks are NOT
// part of this wizard's spec — see README note on the API reconciliation.)
// ─────────────────────────────────────────────────────────────────────────
const HEARD_ABOUT_OPTIONS = [
  "LinkedIn",
  "Company website",
  "Job board",
  "Referral",
  "Other",
] as const;

const wizardSchema = z
  .object({
    // Step 1
    fullName: z.string().min(2, "Must be at least 2 characters").max(100),
    email: z.string().email("Enter a valid email address"),
    phone: z
      .union([
        z.string().regex(/^\+?[\d\s\-()]{8,15}$/, "Enter a valid phone number"),
        z.literal(""),
      ])
      .optional(),

    // Step 2
    coverLetter: z
      .union([z.string().max(2000, "Must be 2000 characters or fewer"), z.literal("")])
      .optional(),
    linkedInUrl: z.union([z.string().url("Enter a valid URL"), z.literal("")]).optional(),
    // Native <select> always emits a string, never undefined — an unselected
    // option emits "" (the default <option value="">), so the empty string
    // must be allowed explicitly alongside the enum members.
    heardAbout: z
      .union([z.enum(HEARD_ABOUT_OPTIONS), z.literal("")])
      .optional(),
  })
  .refine(
    (data) => {
      if (!data.linkedInUrl) return true;
      return (
        data.linkedInUrl.startsWith("https://linkedin.com/") ||
        data.linkedInUrl.startsWith("https://www.linkedin.com/")
      );
    },
    {
      path: ["linkedInUrl"],
      message: "LinkedIn URL must start with https://linkedin.com/ or https://www.linkedin.com/",
    }
  );

type WizardFormData = z.infer<typeof wizardSchema>;

const DEFAULT_VALUES: WizardFormData = {
  fullName: "",
  email: "",
  phone: "",
  coverLetter: "",
  linkedInUrl: "",
  heardAbout: "",
};

// Field groups per step — used with trigger() for step-scoped validation
const STEP_FIELDS: Record<number, (keyof WizardFormData)[]> = {
  1: ["fullName", "email", "phone"],
  2: ["coverLetter", "linkedInUrl", "heardAbout"],
  3: [], // Review step — no fields to validate, only renders summary + submit
};

const STEP_LABELS = ["Your Details", "Your Application", "Review & Submit"];

type Props = {
  jobId: string;
  jobTitle: string;
  isSignedIn: boolean;
  role?: string;
};

export default function ApplicationWizard({ jobId, jobTitle, isSignedIn, role }: Props) {
  const queryClient = useQueryClient();
  const [step, setStep] = useState(1);
  const [draftRestored, setDraftRestored] = useState(false);
  const [hasDraft, setHasDraft] = useState(false);
  const [discardDialogOpen, setDiscardDialogOpen] = useState(false);
  const storageKey = `careerhub-application-${jobId}`;

  // Guards against the localStorage save effect firing before the mount-time
  // restore check has run (which would otherwise immediately overwrite a
  // saved draft with the form's blank default values).
  const hasHydrated = useRef(false);

  const form = useForm<WizardFormData>({
    resolver: zodResolver(wizardSchema),
    defaultValues: DEFAULT_VALUES,
    mode: "onTouched",
  });

  const {
    register,
    handleSubmit,
    trigger,
    watch,
    reset,
    getValues,
    formState: { errors, isSubmitting },
  } = form;

  // ── On mount: check localStorage for a saved draft ───────────────────────
  useEffect(() => {
    try {
      const saved = localStorage.getItem(storageKey);
      if (saved) {
        const parsed = JSON.parse(saved) as WizardFormData;
        reset(parsed);
        setDraftRestored(true);
        setHasDraft(true);
      }
    } catch {
      // Corrupt or inaccessible localStorage — fail silently, start fresh
    } finally {
      hasHydrated.current = true;
    }
    // Only run once per mount / jobId change
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [storageKey]);

  // ── Auto-save on every field change ───────────────────────────────────────
  // form.watch() called with no callback and used directly in render
  // (`const values = watch()`) returns a live snapshot and forces a re-render
  // on every change — that's the usual mistake that makes it seem to run on
  // every render rather than only on changes. The correct pattern for side
  // effects is to call watch(callback) once inside useEffect; it returns a
  // Subscription object with unsubscribe(), which must be cleaned up.
  useEffect(() => {
    const subscription = watch((values) => {
      if (!hasHydrated.current) return; // don't save until after restore check
      try {
        localStorage.setItem(storageKey, JSON.stringify(values));
        setHasDraft(true); // reactive: "Discard draft" button appears as soon as anything is saved
      } catch {
        // localStorage may be unavailable (private browsing, quota) — ignore
      }
    });
    return () => subscription.unsubscribe();
  }, [watch, storageKey]);

  // ── Auto-save on every step change too (covers advancing without typing) ──
  useEffect(() => {
    if (!hasHydrated.current) return;
    try {
      localStorage.setItem(storageKey, JSON.stringify(getValues()));
      setHasDraft(true);
    } catch {
      // ignore
    }
  }, [step, storageKey, getValues]);

  const mutation = useMutation({
    mutationFn: submitApplication,
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["jobs"] });
      clearDraft();
      toast.success(`Application for "${jobTitle}" submitted!`, {
        description: "We'll be in touch soon.",
      });
      reset(DEFAULT_VALUES);
      setStep(1);
    },
    onError: (error: Error) => {
      toast.error("Submission failed", { description: error.message });
    },
  });

  const isCandidateSignedIn = isSignedIn && role === "candidate";

  // ── Step navigation ────────────────────────────────────────────────────
  async function goNext() {
    // Gate: don't allow advancing past step 1 unless signed in as candidate
    if (step === 1 && !isCandidateSignedIn) {
      return; // inline message is rendered below; no navigation happens
    }

    const fieldsToValidate = STEP_FIELDS[step];
    const valid = fieldsToValidate.length === 0 ? true : await trigger(fieldsToValidate);
    if (valid) {
      setStep((s) => Math.min(s + 1, 3));
    }
  }

  function goBack() {
    // Intentionally does NOT call trigger() — see README note on why.
    setStep((s) => Math.max(s - 1, 1));
  }

  // ── Draft helpers ──────────────────────────────────────────────────────
  function clearDraft() {
    try {
      localStorage.removeItem(storageKey);
    } catch {
      // ignore
    }
    setHasDraft(false);
    setDraftRestored(false);
  }

  function handleDiscardDraft() {
    // Pure client-side state manipulation — no Server Action, no mutation.
    clearDraft();
    reset(DEFAULT_VALUES);
    setStep(1);
    setDiscardDialogOpen(false);
    toast.success("Draft discarded");
  }

  // ── Reconcile wizard data → full ApplicationRequest ─────────────────────
  // The wizard (per assignment spec) doesn't collect yearsOfExperience,
  // availableImmediately, or noticePeriodWeeks, but ApplicationRequest
  // requires them. Defaults below are placeholders; see README.
  // heardAbout is intentionally NOT sent — ApplicationRequest has no field
  // for it; it exists purely as wizard UI data.
  const onSubmit = async (data: WizardFormData) => {
    const payload: ApplicationRequest = {
      jobId,
      fullName: data.fullName,
      email: data.email,
      phone: data.phone || undefined,
      coverLetter: data.coverLetter || "",
      linkedInUrl: data.linkedInUrl || undefined,
      yearsOfExperience: 0,
      availableImmediately: true,
      noticePeriodWeeks: 0,
    };
    await mutation.mutateAsync(payload);
  };

  const isBusy = isSubmitting || mutation.isPending;

  // ── Styles (reused from ApplicationForm) ──────────────────────────────
  const inputBase =
    "w-full rounded-md border px-3 py-2 text-sm bg-white text-gray-900 " +
    "placeholder-gray-400 outline-none transition-colors " +
    "focus:ring-2 focus:ring-blue-500 focus:border-blue-500 " +
    "dark:bg-gray-800 dark:text-gray-100 dark:placeholder-gray-500";
  const inputValid = "border-gray-300 dark:border-gray-600";
  const inputError =
    "border-red-400 dark:border-red-500 focus:ring-red-400 dark:focus:ring-red-500";
  const labelBase = "block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1";
  const fieldErrorMsg = "mt-1 text-xs text-red-600 dark:text-red-400";

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      noValidate
      className="space-y-6 rounded-lg border border-gray-200 bg-white p-6 shadow-sm dark:border-gray-700 dark:bg-gray-900"
    >
      <div>
        <div className="flex items-start justify-between gap-4">
          <h2 className="text-lg font-semibold text-gray-900 dark:text-gray-100">
            Apply for <span className="text-blue-600 dark:text-blue-400">{jobTitle}</span>
          </h2>

          {/* Discard Draft — only rendered when a draft actually exists */}
          {hasDraft && (
            <AlertDialog open={discardDialogOpen} onOpenChange={setDiscardDialogOpen}>
              <button
                type="button"
                onClick={() => setDiscardDialogOpen(true)}
                className="shrink-0 text-xs font-medium text-gray-500 hover:text-red-600 dark:text-gray-400 dark:hover:text-red-400 transition-colors"
              >
                Discard draft
              </button>

              <AlertDialogContent>
                <AlertDialogHeader>
                  <AlertDialogTitle>Discard your draft?</AlertDialogTitle>
                  <AlertDialogDescription>
                    Your saved application progress will be permanently
                    deleted. This cannot be undone.
                  </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                  {/*
                    This shadcn install's generated AlertDialogCancel/
                    AlertDialogAction types make `variant` and `size`
                    required props (no defaults), so they must be passed
                    explicitly.
                  */}
                  <AlertDialogCancel variant="outline" size="default">
                    Keep draft
                  </AlertDialogCancel>
                  {/*
                    Same portal constraint as CloseJobButton: AlertDialogAction
                    renders outside this <form>, so type="submit" would do
                    nothing. Unlike CloseJobButton, this isn't a Server Action
                    at all — handleDiscardDraft() is pure client-side state
                    manipulation (localStorage.removeItem + form.reset), so
                    onClick is the natural and only choice here.
                  */}
                  <AlertDialogAction
                    variant="default"
                    size="default"
                    onClick={handleDiscardDraft}
                    className="bg-red-600 text-white hover:bg-red-700 focus:ring-red-500"
                  >
                    Discard draft
                  </AlertDialogAction>
                </AlertDialogFooter>
              </AlertDialogContent>
            </AlertDialog>
          )}
        </div>

        {/* Step indicator */}
        <div className="mt-3 flex items-center gap-2">
          {STEP_LABELS.map((label, i) => {
            const n = i + 1;
            return (
              <div key={label} className="flex items-center gap-2">
                <div
                  className={cn(
                    "flex h-6 w-6 items-center justify-center rounded-full text-xs font-semibold",
                    n === step
                      ? "bg-blue-600 text-white"
                      : n < step
                      ? "bg-blue-100 text-blue-700 dark:bg-blue-900 dark:text-blue-300"
                      : "bg-gray-100 text-gray-400 dark:bg-gray-800 dark:text-gray-500"
                  )}
                >
                  {n}
                </div>
                <span
                  className={cn(
                    "text-xs",
                    n === step
                      ? "font-medium text-gray-900 dark:text-gray-100"
                      : "text-gray-500 dark:text-gray-400"
                  )}
                >
                  {label}
                </span>
                {n < 3 && <div className="h-px w-4 bg-gray-200 dark:bg-gray-700" />}
              </div>
            );
          })}
        </div>
      </div>

      {/* Draft restored banner — dismissible, not silent */}
      {draftRestored && (
        <div
          role="status"
          className="flex items-start justify-between gap-3 rounded-md border border-blue-200 bg-blue-50 px-4 py-3 text-sm text-blue-800 dark:border-blue-800 dark:bg-blue-950 dark:text-blue-300"
        >
          <span>You have a saved draft for this application. Restored automatically.</span>
          <button
            type="button"
            onClick={() => setDraftRestored(false)}
            aria-label="Dismiss draft restored notice"
            className="shrink-0 text-blue-600 hover:text-blue-800 dark:text-blue-400 dark:hover:text-blue-200"
          >
            ✕
          </button>
        </div>
      )}

      {/* ── Step 1: Name & contact ────────────────────────────────────── */}
      {step === 1 && (
        <div className="space-y-5">
          <div>
            <label htmlFor="fullName" className={labelBase}>
              Full name <span className="text-red-500">*</span>
            </label>
            <input
              id="fullName"
              type="text"
              placeholder="Jane Smith"
              aria-invalid={!!errors.fullName}
              {...register("fullName")}
              className={cn(inputBase, errors.fullName ? inputError : inputValid)}
            />
            {errors.fullName && <p className={fieldErrorMsg}>{errors.fullName.message}</p>}
          </div>

          <div>
            <label htmlFor="email" className={labelBase}>
              Email <span className="text-red-500">*</span>
            </label>
            <input
              id="email"
              type="email"
              placeholder="jane@example.com"
              aria-invalid={!!errors.email}
              {...register("email")}
              className={cn(inputBase, errors.email ? inputError : inputValid)}
            />
            {errors.email && <p className={fieldErrorMsg}>{errors.email.message}</p>}
          </div>

          <div>
            <label htmlFor="phone" className={labelBase}>
              Phone{" "}
              <span className="text-gray-400 dark:text-gray-500 font-normal">(optional)</span>
            </label>
            <input
              id="phone"
              type="tel"
              placeholder="+1 555 000 0000"
              aria-invalid={!!errors.phone}
              {...register("phone")}
              className={cn(inputBase, errors.phone ? inputError : inputValid)}
            />
            {errors.phone && <p className={fieldErrorMsg}>{errors.phone.message}</p>}
          </div>

          {/* Auth gate — inline, no redirect */}
          {!isCandidateSignedIn && (
            <p
              role="alert"
              className="rounded-md border border-amber-300 bg-amber-50 px-4 py-3 text-sm text-amber-800 dark:border-amber-700 dark:bg-amber-950 dark:text-amber-300"
            >
              You need to be signed in as a candidate to apply.{" "}
              <Link href="/login" className="font-medium underline hover:no-underline">
                Sign in here.
              </Link>
            </p>
          )}
        </div>
      )}

      {/* ── Step 2: Application details ──────────────────────────────── */}
      {step === 2 && (
        <div className="space-y-5">
          <div>
            <label htmlFor="coverLetter" className={labelBase}>
              Cover letter{" "}
              <span className="text-gray-400 dark:text-gray-500 font-normal">(optional)</span>
            </label>
            <textarea
              id="coverLetter"
              rows={6}
              placeholder="Tell us why you're a great fit…"
              aria-invalid={!!errors.coverLetter}
              {...register("coverLetter")}
              className={cn(inputBase, "resize-y", errors.coverLetter ? inputError : inputValid)}
            />
            {errors.coverLetter && (
              <p className={fieldErrorMsg}>{errors.coverLetter.message}</p>
            )}
          </div>

          <div>
            <label htmlFor="linkedInUrl" className={labelBase}>
              LinkedIn profile URL{" "}
              <span className="text-gray-400 dark:text-gray-500 font-normal">(optional)</span>
            </label>
            <input
              id="linkedInUrl"
              type="url"
              placeholder="https://linkedin.com/in/your-profile"
              aria-invalid={!!errors.linkedInUrl}
              {...register("linkedInUrl")}
              className={cn(inputBase, errors.linkedInUrl ? inputError : inputValid)}
            />
            {errors.linkedInUrl && (
              <p className={fieldErrorMsg}>{errors.linkedInUrl.message}</p>
            )}
          </div>

          <div>
            <label htmlFor="heardAbout" className={labelBase}>
              How did you hear about this role?{" "}
              <span className="text-gray-400 dark:text-gray-500 font-normal">(optional)</span>
            </label>
            <select
              id="heardAbout"
              aria-invalid={!!errors.heardAbout}
              {...register("heardAbout")}
              className={cn(inputBase, errors.heardAbout ? inputError : inputValid)}
            >
              <option value="">Select an option…</option>
              {HEARD_ABOUT_OPTIONS.map((opt) => (
                <option key={opt} value={opt}>
                  {opt}
                </option>
              ))}
            </select>
            {errors.heardAbout && (
              <p className={fieldErrorMsg}>{errors.heardAbout.message}</p>
            )}
          </div>
        </div>
      )}

      {/* ── Step 3: Review & submit ──────────────────────────────────── */}
      {step === 3 && <ReviewStep values={getValues()} />}

      {/* ── Navigation ────────────────────────────────────────────────── */}
      <div className="flex items-center justify-between pt-2">
        <button
          type="button"
          onClick={goBack}
          disabled={step === 1}
          className={cn(
            "rounded-md px-4 py-2 text-sm font-medium transition-colors",
            step === 1
              ? "invisible"
              : "border border-gray-300 text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:text-gray-300 dark:hover:bg-gray-800"
          )}
        >
          Back
        </button>

        {step < 3 ? (
          <button
            type="button"
            onClick={goNext}
            disabled={step === 1 && !isCandidateSignedIn}
            className={cn(
              "rounded-md px-4 py-2.5 text-sm font-semibold transition-colors",
              step === 1 && !isCandidateSignedIn
                ? "cursor-not-allowed bg-gray-300 text-gray-500 dark:bg-gray-700 dark:text-gray-500"
                : "bg-blue-600 text-white hover:bg-blue-700 active:bg-blue-800 dark:bg-blue-500 dark:hover:bg-blue-600"
            )}
          >
            Next
          </button>
        ) : (
          <button
            type="submit"
            disabled={isBusy}
            className={cn(
              "rounded-md px-4 py-2.5 text-sm font-semibold transition-colors",
              isBusy
                ? "cursor-not-allowed bg-gray-300 text-gray-500 dark:bg-gray-700 dark:text-gray-500"
                : "bg-blue-600 text-white hover:bg-blue-700 active:bg-blue-800 dark:bg-blue-500 dark:hover:bg-blue-600"
            )}
          >
            {isBusy ? "Submitting…" : "Submit Application"}
          </button>
        )}
      </div>
    </form>
  );
}

// ─────────────────────────────────────────────────────────────────────────
// Review step — read-only summary, including empty optional fields
// ─────────────────────────────────────────────────────────────────────────
function ReviewStep({ values }: { values: WizardFormData }) {
  const rows: { label: string; value?: string }[] = [
    { label: "Full name", value: values.fullName },
    { label: "Email", value: values.email },
    { label: "Phone", value: values.phone },
    { label: "Cover letter", value: values.coverLetter },
    { label: "LinkedIn URL", value: values.linkedInUrl },
    { label: "How did you hear about this role?", value: values.heardAbout },
  ];

  return (
    <div className="space-y-3">
      <p className="text-sm text-gray-600 dark:text-gray-400">
        Review your application before submitting.
      </p>
      <dl className="divide-y divide-gray-100 rounded-md border border-gray-200 dark:divide-gray-800 dark:border-gray-700">
        {rows.map(({ label, value }) => (
          <div key={label} className="grid grid-cols-3 gap-4 px-4 py-3">
            <dt className="text-sm font-medium text-gray-500 dark:text-gray-400">{label}</dt>
            <dd className="col-span-2 text-sm text-gray-900 dark:text-gray-100 whitespace-pre-line">
              {value && value.trim() !== "" ? (
                value
              ) : (
                <span className="italic text-gray-400 dark:text-gray-500">Not provided</span>
              )}
            </dd>
          </div>
        ))}
      </dl>
    </div>
  );
}
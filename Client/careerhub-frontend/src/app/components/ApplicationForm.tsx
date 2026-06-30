// app/components/ApplicationForm.tsx
"use client";

import { z } from "zod";
import { useForm, SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { cn } from "../lib/utils";
import { submitApplication } from "../lib/api";

export const applicationSchema = z
  .object({
    fullName: z.string().min(2).max(100),
    email: z.string().email(),
    phone: z
      .union([
        z.string().regex(/^\+?[\d\s\-()]{8,15}$/),
        z.literal(""),
        z.undefined(),
      ])
      .transform((v) => (v === "" ? undefined : v))
      .optional(),
    yearsOfExperience: z
      .string()
      .transform((v) => Number(v))
      .refine((v) => Number.isInteger(v), "Must be an integer")
      .refine((v) => v >= 0 && v <= 50, "Must be between 0 and 50"),
    coverLetter: z.string().min(50).max(2000),
    linkedInUrl: z
      .union([
        z.string().url().refine((v) => v.includes("linkedin.com")),
        z.literal(""),
        z.undefined(),
      ])
      .transform((v) => (v === "" ? undefined : v))
      .optional(),
    availableImmediately: z.boolean(),
    noticePeriodWeeks: z
      .string()
      .transform((v) => Number(v))
      .refine((v) => Number.isInteger(v), "Must be an integer")
      .refine((v) => v >= 0, "Must be 0 or more"),
  })
  .refine(
    (data) => {
      if (!data.availableImmediately) {
        return data.noticePeriodWeeks > 0;
      }
      return true;
    },
    {
      path: ["noticePeriodWeeks"],
      message: "Must be > 0 if not available immediately",
    }
  );

export type ApplicationFormInput = z.input<typeof applicationSchema>;
export type ApplicationFormOutput = z.output<typeof applicationSchema>;
export type ApplicationFormData = ApplicationFormOutput;

type Props = {
  jobId: string;
  jobTitle: string;
};

export default function ApplicationForm({ jobId, jobTitle }: Props) {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    reset,
    watch,
    formState: { errors, isSubmitting },
  } = useForm<ApplicationFormInput, any, ApplicationFormOutput>({
    resolver: zodResolver(applicationSchema),
    defaultValues: {
      availableImmediately: true,
      yearsOfExperience: "0",
      noticePeriodWeeks: "0",
    },
  });

  const mutation = useMutation({
    mutationFn: submitApplication,
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["jobs"] });
      reset();
      // Success is an API response to a completed action → toast
      toast.success(`Application for "${jobTitle}" submitted!`, {
        description: "We'll be in touch soon.",
      });
    },
    onError: (error: Error) => {
      // API failure is a response to a completed action → toast
      toast.error("Submission failed", {
        description: error.message,
      });
    },
  });

  const isBusy = isSubmitting || mutation.isPending;
  const availableImmediately = watch("availableImmediately");

  const onSubmit: SubmitHandler<ApplicationFormOutput> = async (data) => {
    await mutation.mutateAsync({ ...data, jobId });
  };

  // ── Styles ────────────────────────────────────────────────────────────────
  const inputBase =
    "w-full rounded-md border px-3 py-2 text-sm bg-white text-gray-900 " +
    "placeholder-gray-400 outline-none transition-colors " +
    "focus:ring-2 focus:ring-blue-500 focus:border-blue-500 " +
    "dark:bg-gray-800 dark:text-gray-100 dark:placeholder-gray-500";

  const inputValid = "border-gray-300 dark:border-gray-600";
  const inputError =
    "border-red-400 dark:border-red-500 focus:ring-red-400 dark:focus:ring-red-500";
  const labelBase =
    "block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1";
  const fieldErrorMsg = "mt-1 text-xs text-red-600 dark:text-red-400";

  return (
    <form
      onSubmit={handleSubmit(onSubmit)}
      noValidate
      className="space-y-5 rounded-lg border border-gray-200 bg-white p-6 shadow-sm dark:border-gray-700 dark:bg-gray-900"
    >
      <h2 className="text-lg font-semibold text-gray-900 dark:text-gray-100">
        Apply for <span className="text-blue-600 dark:text-blue-400">{jobTitle}</span>
      </h2>

      {/*
        ← The mutation.isError inline banner is REMOVED.
           API errors now fire toast.error() in onError above.
           Field-level validation errors remain inline below (correct per spec).
      */}

      {/* Full Name */}
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
        {errors.fullName && (
          <p className={fieldErrorMsg}>{errors.fullName.message}</p>
        )}
      </div>

      {/* Email */}
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
        {errors.email && (
          <p className={fieldErrorMsg}>{errors.email.message}</p>
        )}
      </div>

      {/* Phone */}
      <div>
        <label htmlFor="phone" className={labelBase}>
          Phone{" "}
          <span className="text-gray-400 dark:text-gray-500 font-normal">
            (optional)
          </span>
        </label>
        <input
          id="phone"
          type="tel"
          placeholder="+1 555 000 0000"
          aria-invalid={!!errors.phone}
          {...register("phone")}
          className={cn(inputBase, errors.phone ? inputError : inputValid)}
        />
        {errors.phone && (
          <p className={fieldErrorMsg}>{errors.phone.message}</p>
        )}
      </div>

      {/* Years of Experience */}
      <div>
        <label htmlFor="yearsOfExperience" className={labelBase}>
          Years of experience <span className="text-red-500">*</span>
        </label>
        <input
          id="yearsOfExperience"
          type="number"
          min={0}
          max={50}
          aria-invalid={!!errors.yearsOfExperience}
          {...register("yearsOfExperience")}
          className={cn(
            inputBase,
            errors.yearsOfExperience ? inputError : inputValid
          )}
        />
        {errors.yearsOfExperience && (
          <p className={fieldErrorMsg}>{errors.yearsOfExperience.message}</p>
        )}
      </div>

      {/* Cover Letter */}
      <div>
        <label htmlFor="coverLetter" className={labelBase}>
          Cover letter <span className="text-red-500">*</span>
        </label>
        <textarea
          id="coverLetter"
          rows={6}
          placeholder="Tell us why you're a great fit (50–2000 characters)…"
          aria-invalid={!!errors.coverLetter}
          {...register("coverLetter")}
          className={cn(
            inputBase,
            "resize-y",
            errors.coverLetter ? inputError : inputValid
          )}
        />
        {errors.coverLetter && (
          <p className={fieldErrorMsg}>{errors.coverLetter.message}</p>
        )}
      </div>

      {/* LinkedIn URL */}
      <div>
        <label htmlFor="linkedInUrl" className={labelBase}>
          LinkedIn URL{" "}
          <span className="text-gray-400 dark:text-gray-500 font-normal">
            (optional)
          </span>
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

      {/* Available Immediately */}
      <div className="flex items-center gap-3">
        <input
          id="availableImmediately"
          type="checkbox"
          aria-invalid={!!errors.availableImmediately}
          {...register("availableImmediately")}
          className="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500 dark:border-gray-600 dark:bg-gray-800"
        />
        <label htmlFor="availableImmediately" className={labelBase + " mb-0"}>
          Available immediately
        </label>
      </div>

      {/* Notice Period */}
      <div>
        <label htmlFor="noticePeriodWeeks" className={labelBase}>
          Notice period (weeks) <span className="text-red-500">*</span>
        </label>
        <input
          id="noticePeriodWeeks"
          type="number"
          min={0}
          disabled={availableImmediately}
          aria-invalid={!!errors.noticePeriodWeeks}
          {...register("noticePeriodWeeks")}
          className={cn(
            inputBase,
            errors.noticePeriodWeeks ? inputError : inputValid,
            availableImmediately && "cursor-not-allowed opacity-40 dark:opacity-30"
          )}
        />
        {errors.noticePeriodWeeks && (
          <p className={fieldErrorMsg}>{errors.noticePeriodWeeks.message}</p>
        )}
      </div>

      {/* Submit */}
      <button
        type="submit"
        disabled={isBusy}
        className={cn(
          "w-full rounded-md px-4 py-2.5 text-sm font-semibold transition-colors",
          isBusy
            ? "cursor-not-allowed bg-gray-300 text-gray-500 dark:bg-gray-700 dark:text-gray-500"
            : "bg-blue-600 text-white hover:bg-blue-700 active:bg-blue-800 dark:bg-blue-500 dark:hover:bg-blue-600"
        )}
      >
        {isBusy ? "Submitting…" : "Submit Application"}
      </button>
    </form>
  );
}
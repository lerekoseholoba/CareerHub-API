"use client";

import { z } from "zod";
import { useForm, SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { cn } from "../lib/utils";
import { createJob } from "../lib/api";
import type { JobType } from "../types";

const JOB_TYPES: { value: JobType; label: string }[] = [
  { value: "FullTime",   label: "Full-time" },
  { value: "PartTime",   label: "Part-time" },
  { value: "Contract",   label: "Contract" },
  { value: "Internship", label: "Internship" },
];

// ISO date string — must be a future date
const isoFutureDate = z
  .string()
  .min(1, "Required")
  .refine((v) => !isNaN(Date.parse(v)), "Must be a valid date")
  .refine((v) => new Date(v) > new Date(), "Closing date must be in the future");

// Optional salary field: empty string → undefined, otherwise a non-negative number
const optionalSalary = z
  .string()
  .transform((v) => (v === "" ? undefined : Number(v)))
  .refine(
    (v) => v === undefined || (Number.isFinite(v) && v >= 0),
    "Must be a positive number"
  )
  .optional();

export const postJobSchema = z
  .object({
    title:          z.string().min(2).max(100),
    description:    z.string().min(50).max(5000),
    companyId:      z.string().uuid("Must be a valid company ID"),
    location:       z.string().min(2).max(100),
    closingDate:    isoFutureDate,
    salaryMin:      optionalSalary,
    salaryMax:      optionalSalary,
    employmentType: z.enum(["FullTime", "PartTime", "Contract", "Internship"]),
  })
  .refine(
    (data) => {
      // Only validate the range when both values are provided
      if (data.salaryMin !== undefined && data.salaryMax !== undefined) {
        return data.salaryMax >= data.salaryMin;
      }
      return true;
    },
    {
      path: ["salaryMax"],
      message: "Max salary must be ≥ min salary",
    }
  );

export type PostJobInput  = z.input<typeof postJobSchema>;
export type PostJobOutput = z.output<typeof postJobSchema>;

export default function PostJobForm() {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<PostJobInput, any, PostJobOutput>({
    resolver: zodResolver(postJobSchema),
    defaultValues: {
      employmentType: "FullTime",
      salaryMin: "",
      salaryMax: "",
      closingDate: "",
    },
  });

  const mutation = useMutation({
    mutationFn: createJob,
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["jobs"] });
      reset();
    },
  });

  const isBusy = isSubmitting || mutation.isPending;

  const onSubmit: SubmitHandler<PostJobOutput> = async (data) => {
    await mutation.mutateAsync(data);
  };

  // ── Shared style tokens ───────────────────────────────────────────────────
  const inputBase =
    "w-full rounded-md border px-3 py-2 text-sm bg-white text-gray-900 " +
    "placeholder-gray-400 outline-none transition-colors " +
    "focus:ring-2 focus:ring-blue-500 focus:border-blue-500 " +
    "dark:bg-gray-800 dark:text-gray-100 dark:placeholder-gray-500";
  const inputValid   = "border-gray-300 dark:border-gray-600";
  const inputError   = "border-red-400 dark:border-red-500 focus:ring-red-400 dark:focus:ring-red-500";
  const labelBase    = "block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1";
  const fieldErrorMsg = "mt-1 text-xs text-red-600 dark:text-red-400";
  const optionalHint = "text-gray-400 dark:text-gray-500 font-normal";

  // ── Success state ─────────────────────────────────────────────────────────
  if (mutation.isSuccess) {
    return (
      <div className="rounded-lg border border-green-200 bg-green-50 p-6 dark:border-green-800 dark:bg-green-950">
        <div className="mb-1 flex items-center gap-2">
          <svg
            className="h-5 w-5 text-green-600 dark:text-green-400"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            strokeWidth={2}
          >
            <path strokeLinecap="round" strokeLinejoin="round" d="M5 13l4 4L19 7" />
          </svg>
          <h2 className="text-base font-semibold text-green-800 dark:text-green-200">
            Job posted
          </h2>
        </div>
        <p className="text-sm text-green-700 dark:text-green-300">
          <span className="font-medium">{mutation.data.title}</span> is now live on
          the board.
        </p>
      </div>
    );
  }

  return (
    /*
     * noValidate suppresses the browser's native HTML5 validation popups
     * (required, type="email", etc.) so they don't clash with the Zod/RHF
     * error UI. All validation runs through zodResolver.
     */
    <form
      onSubmit={handleSubmit(onSubmit)}
      noValidate
      className="space-y-5 rounded-lg border border-gray-200 bg-white p-6 shadow-sm dark:border-gray-700 dark:bg-gray-900"
    >
      <h2 className="text-lg font-semibold text-gray-900 dark:text-gray-100">
        Post a job
      </h2>

      {/* ── Server-level error panel ──────────────────────────────────────── */}
      {mutation.isError && (
        <div
          role="alert"
          className="rounded-md border border-red-300 bg-red-50 px-4 py-3 text-sm text-red-700 dark:border-red-700 dark:bg-red-950 dark:text-red-300"
        >
          <span className="font-medium">Failed to post job: </span>
          {mutation.error?.message}
        </div>
      )}

      {/* Title */}
      <div>
        <label htmlFor="title" className={labelBase}>
          Job title <span className="text-red-500">*</span>
        </label>
        <input
          id="title"
          type="text"
          placeholder="e.g. Senior Frontend Engineer"
          aria-invalid={!!errors.title}
          {...register("title")}
          className={cn(inputBase, errors.title ? inputError : inputValid)}
        />
        {errors.title && <p className={fieldErrorMsg}>{errors.title.message}</p>}
      </div>

      {/* Company ID */}
      <div>
        <label htmlFor="companyId" className={labelBase}>
          Company ID <span className="text-red-500">*</span>
        </label>
        <input
          id="companyId"
          type="text"
          placeholder="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
          aria-invalid={!!errors.companyId}
          {...register("companyId")}
          className={cn(inputBase, errors.companyId ? inputError : inputValid)}
        />
        {errors.companyId && (
          <p className={fieldErrorMsg}>{errors.companyId.message}</p>
        )}
      </div>

      {/* Location */}
      <div>
        <label htmlFor="location" className={labelBase}>
          Location <span className="text-red-500">*</span>
        </label>
        <input
          id="location"
          type="text"
          placeholder="e.g. Remote, New York NY"
          aria-invalid={!!errors.location}
          {...register("location")}
          className={cn(inputBase, errors.location ? inputError : inputValid)}
        />
        {errors.location && (
          <p className={fieldErrorMsg}>{errors.location.message}</p>
        )}
      </div>

      {/* Employment Type */}
      <div>
        <label htmlFor="employmentType" className={labelBase}>
          Employment type <span className="text-red-500">*</span>
        </label>
        <select
          id="employmentType"
          aria-invalid={!!errors.employmentType}
          {...register("employmentType")}
          className={cn(inputBase, errors.employmentType ? inputError : inputValid)}
        >
          {JOB_TYPES.map(({ value, label }) => (
            <option key={value} value={value}>
              {label}
            </option>
          ))}
        </select>
        {errors.employmentType && (
          <p className={fieldErrorMsg}>{errors.employmentType.message}</p>
        )}
      </div>

      {/* Closing Date */}
      <div>
        <label htmlFor="closingDate" className={labelBase}>
          Closing date <span className="text-red-500">*</span>
        </label>
        <input
          id="closingDate"
          type="date"
          aria-invalid={!!errors.closingDate}
          {...register("closingDate")}
          className={cn(inputBase, errors.closingDate ? inputError : inputValid)}
        />
        {errors.closingDate && (
          <p className={fieldErrorMsg}>{errors.closingDate.message}</p>
        )}
      </div>

      {/* Salary Range */}
      <div className="grid grid-cols-2 gap-4">
        <div>
          <label htmlFor="salaryMin" className={labelBase}>
            Min salary ($) <span className={optionalHint}>(optional)</span>
          </label>
          <input
            id="salaryMin"
            type="number"
            min={0}
            placeholder="e.g. 60000"
            aria-invalid={!!errors.salaryMin}
            {...register("salaryMin")}
            className={cn(inputBase, errors.salaryMin ? inputError : inputValid)}
          />
          {errors.salaryMin && (
            <p className={fieldErrorMsg}>{errors.salaryMin.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="salaryMax" className={labelBase}>
            Max salary ($) <span className={optionalHint}>(optional)</span>
          </label>
          <input
            id="salaryMax"
            type="number"
            min={0}
            placeholder="e.g. 90000"
            aria-invalid={!!errors.salaryMax}
            {...register("salaryMax")}
            className={cn(inputBase, errors.salaryMax ? inputError : inputValid)}
          />
          {errors.salaryMax && (
            <p className={fieldErrorMsg}>{errors.salaryMax.message}</p>
          )}
        </div>
      </div>

      {/* Description */}
      <div>
        <label htmlFor="description" className={labelBase}>
          Job description <span className="text-red-500">*</span>
        </label>
        <textarea
          id="description"
          rows={8}
          placeholder="Describe the role, responsibilities, and requirements (50–5000 characters)…"
          aria-invalid={!!errors.description}
          {...register("description")}
          className={cn(
            inputBase,
            "resize-y",
            errors.description ? inputError : inputValid
          )}
        />
        {errors.description && (
          <p className={fieldErrorMsg}>{errors.description.message}</p>
        )}
      </div>

      {/* Submit */}
      <button
        type="submit"
        disabled={isBusy}
        className={cn(
          "w-full rounded-md px-4 py-2.5 text-sm font-semibold transition-colors",
          isBusy
            ? // Visually distinct: background shifts to gray + blocked cursor,
              // not just opacity, so the unavailable state is unambiguous
              "cursor-not-allowed bg-gray-300 text-gray-500 dark:bg-gray-700 dark:text-gray-500"
            : "bg-blue-600 text-white hover:bg-blue-700 active:bg-blue-800 dark:bg-blue-500 dark:hover:bg-blue-600"
        )}
      >
        {isBusy ? "Submitting…" : "Post Job"}
      </button>
    </form>
  );
}
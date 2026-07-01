import React from "react";
import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { http, HttpResponse } from "msw";
import { renderWithProviders } from "./utils";
import { server } from "./msw/server";
import ApplicationWizard from "@/app/components/ApplicationWizard";

const defaultProps = {
  jobId: "1",
  jobTitle: "Software Engineer",
  isSignedIn: true,
  role: "candidate",
};

// Clear localStorage between tests so draft state doesn't leak
beforeEach(() => {
  localStorage.clear();
});

// ─── Step navigation ──────────────────────────────────────────────────────────

describe("Step navigation", () => {
  it("renders the step 1 heading on mount", () => {
    renderWithProviders(<ApplicationWizard {...defaultProps} />);
    expect(
      screen.getByRole("heading", { name: /apply for software engineer/i })
    ).toBeInTheDocument();
    expect(screen.getByLabelText(/full name/i)).toBeInTheDocument();
  });

  it("blocks advancement when required step 1 fields are empty", async () => {
    const user = userEvent.setup();
    renderWithProviders(<ApplicationWizard {...defaultProps} />);

    await user.click(screen.getByRole("button", { name: /next/i }));

    expect(screen.getByText(/must be at least 2 characters/i)).toBeInTheDocument();
    expect(screen.getByText(/enter a valid email address/i)).toBeInTheDocument();
    // Still on step 1
    expect(screen.getByLabelText(/full name/i)).toBeInTheDocument();
  });

  it("advances to step 2 when step 1 required fields are filled", async () => {
    const user = userEvent.setup();
    renderWithProviders(<ApplicationWizard {...defaultProps} />);

    await user.type(screen.getByLabelText(/full name/i), "Jane Smith");
    await user.type(screen.getByLabelText(/email/i), "jane@example.com");
    await user.click(screen.getByRole("button", { name: /next/i }));

    expect(screen.getByLabelText(/cover letter/i)).toBeInTheDocument();
  });

  it("back button preserves step 1 values", async () => {
    const user = userEvent.setup();
    renderWithProviders(<ApplicationWizard {...defaultProps} />);

    await user.type(screen.getByLabelText(/full name/i), "Jane Smith");
    await user.type(screen.getByLabelText(/email/i), "jane@example.com");
    await user.click(screen.getByRole("button", { name: /next/i }));
    await user.click(screen.getByRole("button", { name: /back/i }));

    expect(screen.getByDisplayValue("Jane Smith")).toBeInTheDocument();
    expect(screen.getByDisplayValue("jane@example.com")).toBeInTheDocument();
  });
});

// ─── Auth gate ────────────────────────────────────────────────────────────────

describe("Auth gate", () => {
  it("shows the sign-in message when user is not authenticated", async () => {
    const user = userEvent.setup();
    renderWithProviders(
      <ApplicationWizard {...defaultProps} isSignedIn={false} role={undefined} />,
      { session: null }
    );

    await user.type(screen.getByLabelText(/full name/i), "Jane Smith");
    await user.type(screen.getByLabelText(/email/i), "jane@example.com");
    await user.click(screen.getByRole("button", { name: /next/i }));

    expect(
      screen.getByText(/you need to be signed in as a candidate to apply/i)
    ).toBeInTheDocument();
    // Step 2 content should not be visible
    expect(screen.queryByLabelText(/cover letter/i)).not.toBeInTheDocument();
  });

  it("advances normally when user is authenticated as a candidate", async () => {
    const user = userEvent.setup();
    renderWithProviders(<ApplicationWizard {...defaultProps} />);

    await user.type(screen.getByLabelText(/full name/i), "Jane Smith");
    await user.type(screen.getByLabelText(/email/i), "jane@example.com");
    await user.click(screen.getByRole("button", { name: /next/i }));

    expect(screen.getByLabelText(/cover letter/i)).toBeInTheDocument();
  });
});

// ─── Review step ──────────────────────────────────────────────────────────────

describe("Review step", () => {
  it("shows all entered values and 'Not provided' for empty optional fields", async () => {
    const user = userEvent.setup();
    renderWithProviders(<ApplicationWizard {...defaultProps} />);

    // Step 1 — fill required fields only
    await user.type(screen.getByLabelText(/full name/i), "Jane Smith");
    await user.type(screen.getByLabelText(/email/i), "jane@example.com");
    await user.click(screen.getByRole("button", { name: /next/i }));

    // Step 2 — leave all optional fields empty
    await user.click(screen.getByRole("button", { name: /next/i }));

    // Step 3 — review
    expect(screen.getByText("Jane Smith")).toBeInTheDocument();
    expect(screen.getByText("jane@example.com")).toBeInTheDocument();

    // Optional fields left blank must show "Not provided"
    const notProvided = screen.getAllByText(/not provided/i);
    expect(notProvided.length).toBeGreaterThan(0);
  });
});

// ─── Submit flow (MSW) ─────────────────────────────────────────────────────

// Walks every step, filling only the required fields, and lands on the
// review step (step 3) with the submit button visible. Shared by both
// submit tests so the fill logic lives in exactly one place.
async function fillAllSteps(user: ReturnType<typeof userEvent.setup>) {
  await user.type(screen.getByLabelText(/full name/i), "Jane Smith");
  await user.type(screen.getByLabelText(/email/i), "jane@example.com");
  await user.click(screen.getByRole("button", { name: /next/i }));

  await screen.findByLabelText(/cover letter/i);
  await user.click(screen.getByRole("button", { name: /next/i }));

  await screen.findByText(/review your application before submitting/i);
}

describe("Submit flow", () => {
  it("Test 8: form resets after successful submission", async () => {
    const user = userEvent.setup();
    renderWithProviders(<ApplicationWizard {...defaultProps} />);

    await fillAllSteps(user);
    await user.click(screen.getByRole("button", { name: /submit application/i }));

    // On success the wizard calls reset() + setStep(1), removing the
    // step-3 submit button and re-rendering step 1. Waiting for Full name
    // to reappear covers both "submission settled" and "reset to step 1".
    const fullNameInput = await screen.findByLabelText(/full name/i);
    expect(fullNameInput).toHaveValue("");
  });

  it("Test 9: form retains values when the API returns an error", async () => {
    // Relative path (no protocol/origin) — matches the request regardless
    // of which origin the app resolves the fetch to, mirroring the default
    // handler in msw/handlers.ts.
    server.use(
      http.post("/api/applications", () => {
        return new HttpResponse(null, { status: 500 });
      })
    );

    const user = userEvent.setup();
    renderWithProviders(<ApplicationWizard {...defaultProps} />);

    await fillAllSteps(user);
    await user.click(screen.getByRole("button", { name: /submit application/i }));

    // On error the wizard does not change step, so the submit button
    // returns to its idle label right where we left it (step 3).
    await screen.findByRole("button", { name: /submit application/i });

    // goBack() doesn't validate, so navigating back twice to inspect the
    // step 1 field is safe regardless of current form state.
    await user.click(screen.getByRole("button", { name: /back/i }));
    await user.click(screen.getByRole("button", { name: /back/i }));

    expect(screen.getByDisplayValue("Jane Smith")).toBeInTheDocument();
  });
});
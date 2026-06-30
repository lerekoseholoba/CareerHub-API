import React from "react";
import { screen } from "@testing-library/react";
import { renderWithProviders } from "./utils";
import ApplicationWizard from "@/app/components/ApplicationWizard";

describe("ApplicationWizard", () => {
  it("renders step 1 heading", () => {
    renderWithProviders(
      <ApplicationWizard
        jobId="1"
        jobTitle="Software Engineer"
        isSignedIn={true}
        role="candidate"
      />
    );
    expect(screen.getByRole("heading", { name: /apply for software engineer/i })).toBeInTheDocument();
  });
});
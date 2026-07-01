import { describe, test, expect, vi } from "vitest";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import CloseJobButton from "@/app/components/CloseJobButton";

// closeJobListing() calls revalidateTag(), which requires a real Next.js
// request context. Outside of Next's runtime (i.e. in vitest, calling the
// server action directly) it throws. Mock it so the action's fetch + return
// value logic can be exercised in isolation.
vi.mock("next/cache", () => ({
  revalidateTag: vi.fn(),
}));

describe("CloseJobButton", () => {
  test("Test 10: AlertDialog opens when the button is clicked", async () => {
    const user = userEvent.setup();
    render(<CloseJobButton jobId="job-1" currentStatus="Open" />);

    await user.click(screen.getByRole("button", { name: /close job/i }));

    // Radix portals the dialog into document.body; screen queries the
    // whole document, so no special handling is needed to find it.
    expect(await screen.findByText(/close this listing\?/i)).toBeInTheDocument();
  });

  test("Test 11: the API is called when the user confirms", async () => {
    const user = userEvent.setup();
    render(<CloseJobButton jobId="job-1" currentStatus="Open" />);

    await user.click(screen.getByRole("button", { name: /close job/i }));
    await screen.findByText(/close this listing\?/i);

    await user.click(screen.getByRole("button", { name: /close listing/i }));

    // On success the component calls setOpen(false), so the dialog
    // unmounting is the observable proof the PATCH was intercepted and
    // returned a success response.
    await waitFor(() => {
      expect(screen.queryByText(/close this listing\?/i)).not.toBeInTheDocument();
    });

    // And we're back to the idle trigger button, not stuck in an error state.
    expect(await screen.findByRole("button", { name: /close job/i })).toBeInTheDocument();
  });
});
import { vi } from "vitest";
import React from "react";
import { render, RenderOptions } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { useSession } from "next-auth/react";
import { Session } from "next-auth";

vi.mock("next-auth/react", () => ({
  useSession: vi.fn(),
}));

const defaultSession: Session = {
  user: {
    name: "alice",
    email: "alice@example.com",
    role: "candidate",
  },
  expires: "9999-01-01T00:00:00.000Z",
};

interface RenderWithProvidersOptions extends Omit<RenderOptions, "wrapper"> {
  session?: Session | null;
}

export function renderWithProviders(
  ui: React.ReactElement,
  { session = defaultSession, ...renderOptions }: RenderWithProvidersOptions = {}
) {
  vi.mocked(useSession).mockReturnValue(
    session
      ? { data: session, status: "authenticated", update: vi.fn() }
      : { data: null, status: "unauthenticated", update: vi.fn() }
  );

  const queryClient = new QueryClient({
    defaultOptions: {
      queries: { retry: false },
      mutations: { retry: false },
    },
  });

  function Wrapper({ children }: { children: React.ReactNode }) {
    return (
      <QueryClientProvider client={queryClient}>
        {children}
      </QueryClientProvider>
    );
  }

  return render(ui, { wrapper: Wrapper, ...renderOptions });
}
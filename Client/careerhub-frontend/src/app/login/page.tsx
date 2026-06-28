import { signIn } from "@/auth";
import { AuthError } from "next-auth";
import { redirect } from "next/navigation";

interface Props {
  searchParams: Promise<{ error?: string }>;
}

export default async function LoginPage({ searchParams }: Props) {
  const { error } = await searchParams;

  async function handleLogin(formData: FormData) {
    "use server";

    const username = formData.get("username") as string;
    const password = formData.get("password") as string;

    // Determine redirect destination before signIn so we don't depend on
    // reading the session immediately after — which is unreliable in Server Actions.
    // We replicate the role lookup here; authorize() will still validate fully.
    const USERS = [
      { name: "employer1", password: "password123", role: "employer" },
      { name: "employer2", password: "password123", role: "employer" },
      { name: "alice",     password: "password123", role: "candidate" },
      { name: "bob",       password: "password123", role: "candidate" },
    ];

    const matched = USERS.find(
      (u) => u.name === username && u.password === password
    );

    try {
      await signIn("credentials", {
        username,
        password,
        redirect: false,
      });
    } catch (err) {
      if (err instanceof AuthError) {
        redirect(`/login?error=${err.type}`);
      }
      throw err;
    }

    // signIn succeeded — matched is guaranteed to exist
    redirect(matched?.role === "employer" ? "/dashboard/listings" : "/jobs");
  }

  return (
    <div className="flex items-center justify-center min-h-[60vh]">
      <div className="w-full max-w-sm p-8 rounded-xl border border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-900 shadow-sm">
        <h1 className="text-2xl font-bold mb-6">Sign in</h1>

        {error === "CredentialsSignin" && (
          <p
            role="alert"
            className="mb-4 px-4 py-3 rounded-lg bg-red-50 dark:bg-red-900/20 text-red-700 dark:text-red-400 text-sm border border-red-200 dark:border-red-800"
          >
            Invalid username or password.
          </p>
        )}

        <form action={handleLogin} className="flex flex-col gap-4">
          <label className="flex flex-col gap-1 text-sm font-medium">
            Username
            <input
              name="username"
              type="text"
              required
              className="px-3 py-2 rounded-lg border border-gray-300 dark:border-gray-600 bg-transparent focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </label>

          <label className="flex flex-col gap-1 text-sm font-medium">
            Password
            <input
              name="password"
              type="password"
              required
              className="px-3 py-2 rounded-lg border border-gray-300 dark:border-gray-600 bg-transparent focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </label>

          <button
            type="submit"
            className="mt-2 py-2 rounded-lg bg-blue-600 hover:bg-blue-700 text-white font-medium transition-colors"
          >
            Sign in
          </button>
        </form>
      </div>
    </div>
  );
}
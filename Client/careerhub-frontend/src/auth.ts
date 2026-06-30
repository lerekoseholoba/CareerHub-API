import NextAuth from "next-auth";
import Credentials from "next-auth/providers/credentials";

const USERS = [
  { id: "1", name: "employer1", password: "password123", role: "employer" },
  { id: "2", name: "employer2", password: "password123", role: "employer" },
  { id: "3", name: "alice",     password: "password123", role: "candidate" },
  { id: "4", name: "bob",       password: "password123", role: "candidate" },
];

export const { handlers, auth, signIn, signOut } = NextAuth({
  trustHost: true,
  session: { strategy: "jwt" },
  pages: { signIn: "/login" },
  providers: [
    Credentials({
      credentials: {
        username: { label: "Username", type: "text" },
        password: { label: "Password", type: "password" },
      },
      async authorize(credentials) {
        const { username, password } = credentials as {
          username: string;
          password: string;
        };
        const user = USERS.find((u) => u.name === username);
        if (!user || user.password !== password) return null;
        return { id: user.id, name: user.name, role: user.role };
      },
    }),
  ],
  callbacks: {
    async jwt({ token, user }) {
      if (user) token.role = (user as { role: string }).role;
      return token;
    },
    async session({ session, token }) {
      if (session.user) session.user.role = token.role as string;
      return session;
    },
  },
});
import { auth } from "@/auth";
import { NextResponse } from "next/server";

export default auth((req) => {
  const { pathname } = req.nextUrl;
  const session = req.auth;
  const role = session?.user?.role;
  const isLoggedIn = !!session;

  // /login — redirect already-authenticated users away
  if (pathname === "/login") {
    if (isLoggedIn) {
      const destination = role === "employer" ? "/dashboard/listings" : "/jobs";
      return NextResponse.redirect(new URL(destination, req.nextUrl.origin));
    }
    return NextResponse.next();
  }

  // /dashboard and everything under it — employers only
  if (pathname.startsWith("/dashboard")) {
    if (!isLoggedIn) {
      return NextResponse.redirect(new URL("/login", req.nextUrl.origin));
    }
    if (role !== "employer") {
      return NextResponse.redirect(new URL("/jobs", req.nextUrl.origin));
    }
    return NextResponse.next();
  }

  // everything else — public
  return NextResponse.next();
});

export const config = {
  matcher: [
    "/((?!_next/static|_next/image|favicon.ico|api/auth).*)",
  ],
};
"use client";

import { useDashboardStore } from "@/stores/dashboardStore";
import ListingsTable from "./ListingsTable";
import type { JobListing } from "../types/index";

interface Props {
  jobs: JobListing[];
}

export default function ListingsView({ jobs }: Props) {
  const view           = useDashboardStore((s) => s.view);
  const showClosedJobs = useDashboardStore((s) => s.showClosedJobs);

  const filtered = showClosedJobs ? jobs : jobs.filter((j) => j.isOpen);

  return <ListingsTable jobs={filtered} view={view} />;
}
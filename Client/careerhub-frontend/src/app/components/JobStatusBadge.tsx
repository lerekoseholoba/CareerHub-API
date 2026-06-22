import { Badge } from "./ui/badge";
import { cn } from "../lib/utils";
import type { JobType } from "../types";

interface JobStatusBadgeProps {
  employmentType?: JobType;
  isActive?: boolean;
}

const employmentTypeClasses: Record<JobType, string> = {
  FullTime: "bg-green-100 text-green-800 hover:bg-green-100",
  PartTime: "bg-blue-100 text-blue-800 hover:bg-blue-100",
  Contract: "bg-orange-100 text-orange-800 hover:bg-orange-100",
  Internship: "bg-purple-100 text-purple-800 hover:bg-purple-100",
};

export default function JobStatusBadge({
  employmentType,
  isActive,
}: JobStatusBadgeProps) {
  // Employment type badge
  if (employmentType && employmentTypeClasses[employmentType]) {
  return (
    <Badge className={cn(employmentTypeClasses[employmentType])}>
      {employmentType}
    </Badge>
  );
}

  // Active status badge
 if (isActive === false) {
  return (
    <Badge className="bg-red-100 text-red-700">
      Closed
    </Badge>
  );
}

return (
  <Badge className="bg-green-100 text-green-700">
    Active
  </Badge>
);
}
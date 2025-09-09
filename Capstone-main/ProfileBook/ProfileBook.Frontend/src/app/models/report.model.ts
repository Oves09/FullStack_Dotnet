export interface Report {
  reportId: number;
  reportedUserId: string;
  reportedUserName: string;
  reportedUserFullName: string;
  reportingUserId: string;
  reportingUserName: string;
  reportingUserFullName: string;
  reason: string;
  description?: string;
  timeStamp: Date;
  status: ReportStatus;
  adminNotes?: string;
  reviewedByUserName?: string;
  reviewedAt?: Date;
}

export interface CreateReportRequest {
  reportedUserId: string;
  reason: string;
  description?: string;
}

export interface ReviewReportRequest {
  reportId: number;
  status: ReportStatus;
  adminNotes?: string;
}

export enum ReportStatus {
  Pending = 0,
  UnderReview = 1,
  Resolved = 2,
  Dismissed = 3
}

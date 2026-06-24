export enum CaseStatus {
  Open,
  PendingDocuments,
  Closed,
  Cancelled
}

export interface CaseViewModel {
  caseId: number;

  status: CaseStatus;
  createdAt: string;
  initiatorId: number;
  responderId: number;
  closedAt: string;
}

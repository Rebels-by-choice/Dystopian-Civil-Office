import { CaseStatus } from '../responses/case.viewmodel';

export interface CreateCaseRequest {
  initiatorId: number;
}

export interface UpdateCaseRequest {
  caseId: number;
  newStatus: CaseStatus
  partyId: number;
}

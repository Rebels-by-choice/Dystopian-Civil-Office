export interface StatisticsViewModel {
  id: number;
  method: string;
  path: string;
  statusCode: number;
  executedAt?: string;
}
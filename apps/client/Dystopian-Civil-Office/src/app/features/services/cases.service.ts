import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, shareReplay, tap, throwError } from 'rxjs';

import { apiConfig } from '../../core/config/api-main-url';
import { CreateCaseRequest, UpdateCaseRequest } from '../../shared/api-models/requests/case.model';
import { CaseViewModel } from '../../shared/api-models/responses/case.viewmodel';

@Injectable({
  providedIn: 'root',
})
export class CasesService {
  private http = inject(HttpClient);
  private readonly url = `${apiConfig.baseUrl}/Case`;

  private cases$?: Observable<CaseViewModel[]>;

  public getCases(): Observable<CaseViewModel[]> {
    if (!this.cases$) {
      this.cases$ = this.http.get<CaseViewModel[]>(this.url).pipe(
        map((items) => [...items].reverse()),
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.cases$ = undefined;
          return throwError(() => error);
        }),
      );
    }

    return this.cases$;
  }

  public createCase(request: CreateCaseRequest): Observable<void> {
    return this.http.post<void>(this.url, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public updateCase(caseId: number, request: UpdateCaseRequest): Observable<void> {
    return this.http.put<void>(`${this.url}/${caseId}`, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public deleteCase(caseId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${caseId}`).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public refreshCases(): Observable<CaseViewModel[]> {
    this.cases$ = undefined;
    return this.getCases();
  }

  public clearCache(): void {
    this.cases$ = undefined;
  }
}

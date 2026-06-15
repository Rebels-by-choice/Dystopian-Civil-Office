import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, shareReplay, tap, throwError } from 'rxjs';

import { DeathRecordViewModel } from '../../shared/api-models/responses/deathRecord.viewmodel';
import { apiConfig } from '../../core/config/api-main-url';
import {
  CreateBirthRecordModel,
  UpdateBirthRecordModel,
} from '../../shared/api-models/requests/deathRecord.model';

@Injectable({
  providedIn: 'root',
})
export class DeathRecordsService {
  private http = inject(HttpClient);
  private readonly url = `${apiConfig.baseUrl}/DeathRecord`;

  private deathRecords$?: Observable<DeathRecordViewModel[]>;

  public getDeathRecords(): Observable<DeathRecordViewModel[]> {
    if (!this.deathRecords$) {
      this.deathRecords$ = this.http.get<DeathRecordViewModel[]>(this.url).pipe(
        map((items) => [...items].reverse()),
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.deathRecords$ = undefined;
          return throwError(() => error);
        }),
      );
    }

    return this.deathRecords$;
  }

  public createDeathRecord(request: CreateBirthRecordModel): Observable<void> {
    return this.http.post<void>(this.url, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public updateDeathRecord(
    deathRecordId: number,
    request: UpdateBirthRecordModel,
  ): Observable<void> {
    return this.http.put<void>(`${this.url}/${deathRecordId}`, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public deleteDeathRecord(deathRecordId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${deathRecordId}`).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public refreshDeathRecords(): Observable<DeathRecordViewModel[]> {
    this.deathRecords$ = undefined;
    return this.getDeathRecords();
  }

  public clearCache(): void {
    this.deathRecords$ = undefined;
  }
}

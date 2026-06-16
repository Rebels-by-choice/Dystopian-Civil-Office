import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, shareReplay, tap, throwError } from 'rxjs';

import { BirthRecordViewModel } from '../../shared/api-models/responses/birthRecord.viewmodel';
import { apiConfig } from '../../core/config/api-main-url';
import {
  CreateBirthRecordModel,
  UpdateBirthRecordModel,
} from '../../shared/api-models/requests/birthRecord.model';

@Injectable({
  providedIn: 'root',
})
export class BirthRecordsService {
  private http = inject(HttpClient);
  private readonly url = `${apiConfig.baseUrl}/BirthRecord`;

  private birthRecords$?: Observable<BirthRecordViewModel[]>;

  public getBirthRecords(): Observable<BirthRecordViewModel[]> {
    if (!this.birthRecords$) {
      this.birthRecords$ = this.http.get<BirthRecordViewModel[]>(this.url).pipe(
        map((items) => [...items].reverse()),
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.birthRecords$ = undefined;
          return throwError(() => error);
        }),
      );
    }

    return this.birthRecords$;
  }

  public createBirthRecord(request: CreateBirthRecordModel): Observable<void> {
    return this.http.post<void>(this.url, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public updateBirthRecord(
    birthRecordId: number,
    request: UpdateBirthRecordModel,
  ): Observable<void> {
    return this.http.put<void>(`${this.url}/${birthRecordId}`, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public deleteBirthRecord(birthRecordId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${birthRecordId}`).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public refreshBirthRecords(): Observable<BirthRecordViewModel[]> {
    this.birthRecords$ = undefined;
    return this.getBirthRecords();
  }

  public clearCache(): void {
    this.birthRecords$ = undefined;
  }
}

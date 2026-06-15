import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, shareReplay, tap, throwError } from 'rxjs';

import {
  CreateMarriageModel,
  UpdateMarriageModel,
} from '../../shared/api-models/requests/marriage.model';
import { MarriageViewModel } from '../../shared/api-models/responses/marriage.viewmodel';
import { apiConfig } from '../../core/config/api-main-url';

@Injectable({
  providedIn: 'root',
})
export class MarriagesService {
  private http = inject(HttpClient);
  private readonly url = `${apiConfig.baseUrl}/Marriage`;

  private marriages$?: Observable<MarriageViewModel[]>;

  public getMarriages(): Observable<MarriageViewModel[]> {
    if (!this.marriages$) {
      this.marriages$ = this.http.get<MarriageViewModel[]>(this.url).pipe(
        map((items) => [...items].reverse()),
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.marriages$ = undefined;
          return throwError(() => error);
        }),
      );
    }

    return this.marriages$;
  }

  public createMarriage(request: CreateMarriageModel): Observable<void> {
    return this.http.post<void>(this.url, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public updateMarriage(marriageRecordId: number, request: UpdateMarriageModel): Observable<void> {
    return this.http.put<void>(`${this.url}/${marriageRecordId}`, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public deleteMarriage(marriageRecordId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${marriageRecordId}`).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public refreshMarriages(): Observable<MarriageViewModel[]> {
    this.marriages$ = undefined;
    return this.getMarriages();
  }

  public clearCache(): void {
    this.marriages$ = undefined;
  }
}

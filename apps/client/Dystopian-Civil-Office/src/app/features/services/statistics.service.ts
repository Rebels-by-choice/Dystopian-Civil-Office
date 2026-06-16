import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, shareReplay, throwError } from 'rxjs';

import { apiConfig } from '../../core/config/api-main-url';
import { StatisticsViewModel } from '../../shared/api-models/responses/statistics.viewmodel';

@Injectable({
  providedIn: 'root',
})
export class StatisticsService {
  private readonly http = inject(HttpClient);
  private readonly url = `${apiConfig.baseUrl}/ApiStats`;

  private stats$?: Observable<StatisticsViewModel[]>;

  public getStatistics(): Observable<StatisticsViewModel[]> {
    if (!this.stats$) {
      this.stats$ = this.http.get<StatisticsViewModel[]>(this.url).pipe(
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.stats$ = undefined;
          return throwError(() => error);
        }),
      );
    }

    return this.stats$;
  }
}
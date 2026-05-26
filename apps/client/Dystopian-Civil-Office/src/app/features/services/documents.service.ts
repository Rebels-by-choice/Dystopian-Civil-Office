import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, shareReplay, throwError } from 'rxjs';

import { DocumentViewModel } from '../../shared/viewmodels/document.viewmodel';
import { apiConfig } from '../../core/config/api-main-url';

@Injectable({
  providedIn: 'root',
})
export class DocumentsService {
  private readonly http = inject(HttpClient);
  private readonly url = `${apiConfig.baseUrl}/document`;

  private documents$?: Observable<DocumentViewModel[]>;

  public getDocuments(): Observable<DocumentViewModel[]> {
    if (!this.documents$) {
      this.documents$ = this.http.get<DocumentViewModel[]>(this.url).pipe(
        map((documents) => [...documents].reverse()),
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.documents$ = undefined;
          return throwError(() => error);
        }),
      );
    }

    return this.documents$;
  }

  public refreshDocuments(): Observable<DocumentViewModel[]> {
    this.documents$ = undefined;
    return this.getDocuments();
  }

  public clearCache(): void {
    this.documents$ = undefined;
  }
}

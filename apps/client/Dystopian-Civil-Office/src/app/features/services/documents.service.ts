import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, shareReplay, tap, throwError } from 'rxjs';

import { DocumentViewModel } from '../../shared/api-models/responses/document.viewmodel';
import { apiConfig } from '../../core/config/api-main-url';
import {
  DeleteDocumentModel,
  DocumentModel,
} from '../../shared/api-models/requests/document.model';
import { UpdateDocumentModel } from '../../shared/api-models/requests/document.model';

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

  public createDocument(request: DocumentModel): Observable<void> {
    return this.http.post<void>(this.url, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public updateDocument(documentId: number, request: UpdateDocumentModel): Observable<void> {
    return this.http.put<void>(`${this.url}/${documentId}`, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public deleteDocument(documentId: number) {
    return this.http.delete<void>(`${this.url}/${documentId}`);
  }

  public refreshDocuments(): Observable<DocumentViewModel[]> {
    this.documents$ = undefined;
    return this.getDocuments();
  }

  public clearCache(): void {
    this.documents$ = undefined;
  }
}

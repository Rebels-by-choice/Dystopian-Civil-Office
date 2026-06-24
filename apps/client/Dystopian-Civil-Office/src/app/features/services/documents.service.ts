import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, catchError, map, shareReplay, tap, throwError } from 'rxjs';

import { DocumentViewModel } from '../../shared/api-models/responses/document.viewmodel';
import { apiConfig } from '../../core/config/api-main-url';
import {
  CreateDocumentModel,
  UpdateDocumentModel,
} from '../../shared/api-models/requests/document.model';

import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
class DocumentsService {
  private readonly http = inject(HttpClient);
  private readonly url = `${apiConfig.baseUrl}/document`;
  private readonly paperlessUrl = environment.paperlessApiUrl;
  private readonly paperlessToken = environment.paperlessToken;

  private documents$?: Observable<DocumentViewModel[]>;
  private readonly documentsByCategoryCache = new Map<string, Observable<DocumentViewModel[]>>();

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

  public getDocumentsByCategory(category: string): Observable<DocumentViewModel[]> {
    const normalizedCategory = category.trim();

    const cachedDocuments = this.documentsByCategoryCache.get(normalizedCategory);
    if (cachedDocuments) {
      return cachedDocuments;
    }

    const request$ = this.http
      .get<DocumentViewModel[]>(`${this.url}/categories`, {
        params: new HttpParams().set('category', normalizedCategory),
      })
      .pipe(
        map((documents) => [...documents].reverse()),
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.documentsByCategoryCache.delete(normalizedCategory);
          return throwError(() => error);
        }),
      );

    this.documentsByCategoryCache.set(normalizedCategory, request$);

    return request$;
  }

  public getDocumentPreview(paperlessDocumentId: number): Observable<Blob> {
    // Note: Paperless-ngx typically uses "Token <your_token>" for auth.
    // Change to "Bearer ${token}" if you are using an OAuth/JWT proxy.
    return this.http.get(`${this.paperlessUrl}/api/documents/${paperlessDocumentId}/preview/`, {
      headers: {
        Authorization: `Token ${this.paperlessToken}`,
      },
      responseType: 'blob',
    });
  }

  public createDocument(request: CreateDocumentModel): Observable<void> {
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

  public deleteDocument(documentId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${documentId}`).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public refreshDocuments(): Observable<DocumentViewModel[]> {
    this.documents$ = undefined;
    return this.getDocuments();
  }

  public refreshDocumentsByCategory(category: string): Observable<DocumentViewModel[]> {
    this.documentsByCategoryCache.delete(category.trim());
    return this.getDocumentsByCategory(category);
  }

  public clearCache(): void {
    this.documents$ = undefined;
    this.documentsByCategoryCache.clear();
  }
}

export default DocumentsService;

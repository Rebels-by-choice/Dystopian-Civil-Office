import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, catchError, map, shareReplay, tap, throwError } from 'rxjs';

import { PersonViewModel } from '../../shared/api-models/responses/person.viewmodel';
import { apiConfig } from '../../core/config/api-main-url';
import {
  CreatePersonModel,
  UpdatePersonModel,
} from '../../shared/api-models/requests/person.model';

@Injectable({
  providedIn: 'root',
})
export class PersonsService {
  private readonly http = inject(HttpClient);
  private readonly url = `${apiConfig.baseUrl}/Person`;

  private persons$?: Observable<PersonViewModel[]>;
  private readonly personsByGenderCache = new Map<string, Observable<PersonViewModel[]>>();

  public getPersons(): Observable<PersonViewModel[]> {
    if (!this.persons$) {
      this.persons$ = this.http.get<PersonViewModel[]>(this.url).pipe(
        map((items) => [...items].reverse()),
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.persons$ = undefined;
          return throwError(() => error);
        }),
      );
    }

    return this.persons$;
  }

  public getPersonsByGender(gender: string): Observable<PersonViewModel[]> {
    const normalizedGender = gender.trim();

    const cachedPersons = this.personsByGenderCache.get(normalizedGender);
    if (cachedPersons) {
      return cachedPersons;
    }

    const request$ = this.http
      .get<PersonViewModel[]>(`${this.url}/genders`, {
        params: new HttpParams().set('gender', normalizedGender),
      })
      .pipe(
        map((items) => [...items].reverse()),
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.personsByGenderCache.delete(normalizedGender);
          return throwError(() => error);
        }),
      );

    this.personsByGenderCache.set(normalizedGender, request$);

    return request$;
  }

  public createPerson(request: CreatePersonModel): Observable<void> {
    return this.http.post<void>(this.url, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public updatePerson(personId: number, request: UpdatePersonModel): Observable<void> {
    return this.http.put<void>(`${this.url}/${personId}`, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public deletePerson(personId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${personId}`).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public refreshPersons(): Observable<PersonViewModel[]> {
    this.persons$ = undefined;
    return this.getPersons();
  }

  public refreshPersonsByGender(gender: string): Observable<PersonViewModel[]> {
    this.personsByGenderCache.delete(gender.trim());
    return this.getPersonsByGender(gender);
  }

  public clearCache(): void {
    this.persons$ = undefined;
    this.personsByGenderCache.clear();
  }
}

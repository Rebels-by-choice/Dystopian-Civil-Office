import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, catchError, map, shareReplay, tap, throwError } from 'rxjs';

import { PersonAddressViewModel } from '../../shared/api-models/responses/personAddress.viewmodel';
import { apiConfig } from '../../core/config/api-main-url';
import {
  DeleteAddressModel,
  CreateAddressModel,
  UpdateAddressModel,
} from '../../shared/api-models/requests/address.model';

@Injectable({
  providedIn: 'root',
})
export class AddressesService {
  private http = inject(HttpClient);
  private readonly url = `${apiConfig.baseUrl}/PersonAddress`;

  private addresses$?: Observable<PersonAddressViewModel[]>;

  public getAddresses(): Observable<PersonAddressViewModel[]> {
    if (!this.addresses$) {
      this.addresses$ = this.http.get<PersonAddressViewModel[]>(this.url).pipe(
        map((addresses) => [...addresses].reverse()),
        shareReplay({ bufferSize: 1, refCount: true }),
        catchError((error) => {
          this.addresses$ = undefined;
          return throwError(() => error);
        }),
      );
    }

    return this.addresses$;
  }

  public createAddress(request: CreateAddressModel): Observable<void> {
    return this.http.post<void>(this.url, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public updateAddress(addressId: number, request: UpdateAddressModel): Observable<void> {
    return this.http.put<void>(`${this.url}/${addressId}`, request).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public deleteAddress(addressId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${addressId}`).pipe(
      tap(() => this.clearCache()),
      catchError((error) => throwError(() => error)),
    );
  }

  public refreshAddresses(): Observable<PersonAddressViewModel[]> {
    this.addresses$ = undefined;
    return this.getAddresses();
  }

  public clearCache(): void {
    this.addresses$ = undefined;
  }
}

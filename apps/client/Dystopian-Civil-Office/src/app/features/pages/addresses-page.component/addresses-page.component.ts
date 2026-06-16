import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { TableComponent } from '../../../shared/ui/table/table.component';
import { AddressesService } from '../../services/addresses.service';
import { CreateAddressDialogComponent } from '../../dialogs/address-dialogs/create-address-dialog.component/create-address-dialog.component';
import { AddressViewModel } from '../../../shared/api-models/responses/address.viewmodel';
import { UpdateAddressDialogComponent } from '../../dialogs/address-dialogs/update-address-dialog.component/update-address-dialog.component';
import { DeleteAddressDialogComponent } from '../../dialogs/address-dialogs/delete-address-dialog.component/delete-address-dialog.component';

type SortColumn =
  | 'registryNumber'
  | 'city'
  | 'street'
  | 'houseNumber'
  | 'apartmentNumber'
  | 'postalCode'
  | 'country'
  | 'documentName';

type SortDirection = 'asc' | 'desc';

interface SortState {
  column: SortColumn;
  direction: SortDirection;
}

@Component({
  selector: 'app-addresses-page.component',
  standalone: true,
  imports: [ButtonComponent, TableComponent, AsyncPipe],
  templateUrl: 'addresses-page.component.html',
  styles: ``,
})
export class AddressesPageComponent {
  private readonly dialog = inject(MatDialog);
  private readonly addressesService = inject(AddressesService);

  private readonly sortState$ = new BehaviorSubject<SortState>({
    column: 'registryNumber',
    direction: 'desc',
  });

  protected addressesErrorMessage = '';

  private readonly rawAddresses$ = this.addressesService.getAddresses().pipe(
    catchError(() => {
      this.addressesErrorMessage = 'Failed to load addresses.';
      return of([]);
    }),
  );

  protected readonly addresses$ = combineLatest([this.rawAddresses$, this.sortState$]).pipe(
    map(([addresses, sortState]) => this.sortAddresses(addresses, sortState)),
  );

  protected openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateAddressDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
    });

    dialogRef.afterClosed().subscribe((created: boolean) => {
      if (!created) {
        return;
      }

      this.reloadCurrentAddresses();
    });
  }

  protected openUpdateDialog(address: AddressViewModel): void {
    const dialogRef = this.dialog.open(UpdateAddressDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: address,
    });

    dialogRef.afterClosed().subscribe((updated: boolean) => {
      if (!updated) {
        return;
      }

      this.reloadCurrentAddresses();
    });
  }

  protected openDeleteDialog(address: AddressViewModel): void {
    const dialogRef = this.dialog.open(DeleteAddressDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: address,
    });

    dialogRef.afterClosed().subscribe((deleted: boolean) => {
      if (!deleted) {
        return;
      }

      this.reloadCurrentAddresses();
    });
  }

  protected sortBy(column: SortColumn): void {
    const currentSort = this.sortState$.value;

    if (currentSort.column === column) {
      this.sortState$.next({
        column,
        direction: currentSort.direction === 'asc' ? 'desc' : 'asc',
      });
    } else {
      this.sortState$.next({
        column,
        direction: 'asc',
      });
    }
  }

  protected getSortDirection(column: SortColumn): SortDirection {
    const currentSort = this.sortState$.value;
    return currentSort.column === column ? currentSort.direction : 'asc';
  }

  protected getApartmentNumberDisplayValue(address: AddressViewModel): string {
    return address.apartmentNumber?.trim() || 'No apartment number';
  }

  protected getDocumentNameDisplayValue(address: AddressViewModel): string {
    return address.documentName?.trim() || 'No document provided';
  }

  private sortAddresses(addresses: AddressViewModel[], sortState: SortState): AddressViewModel[] {
    return [...addresses].sort((a, b) => {
      const aValue = this.getSortableAddressValue(a, sortState.column);
      const bValue = this.getSortableAddressValue(b, sortState.column);

      if (aValue === bValue) {
        return 0;
      }

      const comparison = aValue.localeCompare(bValue, undefined, {
        numeric: true,
        sensitivity: 'base',
      });

      return sortState.direction === 'asc' ? comparison : -comparison;
    });
  }

  private getSortableAddressValue(address: AddressViewModel, column: SortColumn): string {
    switch (column) {
      case 'registryNumber':
        return this.normalizeSortValue(address.registryNumber);
      case 'city':
        return this.normalizeSortValue(address.city);
      case 'street':
        return this.normalizeSortValue(address.street);
      case 'houseNumber':
        return this.normalizeSortValue(address.houseNumber);
      case 'apartmentNumber':
        return this.normalizeSortValue(
          address.apartmentNumber,
          this.getApartmentNumberDisplayValue(address),
        );
      case 'postalCode':
        return this.normalizeSortValue(address.postalCode);
      case 'country':
        return this.normalizeSortValue(address.country);
      case 'documentName':
        return this.normalizeSortValue(
          address.documentName,
          this.getDocumentNameDisplayValue(address),
        );
      default:
        return '';
    }
  }

  private normalizeSortValue(value: string | null | undefined, fallback = ''): string {
    return (value ?? fallback).toString().trim().toLowerCase();
  }

  private reloadCurrentAddresses(): void {
    this.addressesService.refreshAddresses().subscribe({
      next: () => {
        this.addressesErrorMessage = '';
      },
      error: () => {
        this.addressesErrorMessage = 'Failed to load addresses.';
      },
    });
  }
}

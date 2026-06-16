import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

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

  private readonly rawAddresses$ = this.sortState$.pipe(
    switchMap(() => {
      this.addressesErrorMessage = '';
      return this.addressesService.getAddresses().pipe(
        catchError((error) => {
          this.addressesErrorMessage = 'Failed to load addresses.';
          return of([]);
        }),
      );
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

  private sortAddresses(addresses: AddressViewModel[], sortState: SortState): AddressViewModel[] {
    const sorted = [...addresses].sort((a, b) => {
      let aValue: unknown;
      let bValue: unknown;

      switch (sortState.column) {
        case 'registryNumber':
          aValue = a.registryNumber.toLowerCase();
          bValue = b.registryNumber.toLowerCase();
          break;
        case 'city':
          aValue = a.city.toLowerCase();
          bValue = b.city.toLowerCase();
          break;
        case 'street':
          aValue = a.street.toLowerCase();
          bValue = b.street.toLowerCase();
          break;
        case 'houseNumber':
          aValue = a.houseNumber.toLowerCase();
          bValue = b.houseNumber.toLowerCase();
          break;
        case 'apartmentNumber':
          aValue = a.apartmentNumber.toLowerCase();
          bValue = b.apartmentNumber.toLowerCase();
          break;
        case 'postalCode':
          aValue = a.postalCode.toLowerCase();
          bValue = b.postalCode.toLowerCase();
          break;
        case 'country':
          aValue = a.country.toLowerCase();
          bValue = b.country.toLowerCase();
          break;
        case 'documentName':
          aValue = a.documentName.toLowerCase();
          bValue = b.documentName.toLowerCase();
          break;
      }

      if (aValue === bValue) {
        return 0;
      }

      const isAsc = sortState.direction === 'asc';
      return (aValue ?? 0) < (bValue ?? 0) ? (isAsc ? -1 : 1) : isAsc ? 1 : -1;
    });

    return sorted;
  }

  private reloadCurrentAddresses(): void {
    this.addressesService.refreshAddresses();
    this.sortState$.next(this.sortState$.value);
  }
}

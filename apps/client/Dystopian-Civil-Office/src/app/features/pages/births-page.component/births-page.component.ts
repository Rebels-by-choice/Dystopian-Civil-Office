import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { TableComponent } from '../../../shared/ui/table/table.component';
import { BirthRecordsService } from '../../services/birth-records.service';
import { CreateBirthRecordDialogComponent } from '../../dialogs/birth-record-dialogs/create-birth-record-dialog.component/create-birth-record-dialog.component';
import { BirthRecordViewModel } from '../../../shared/api-models/responses/birthRecord.viewmodel';
import { UpdateBirthRecordDialogComponent } from '../../dialogs/birth-record-dialogs/update-birth-record-dialog.component/update-birth-record-dialog.component';
import { DeleteBirthRecordDialogComponent } from '../../dialogs/birth-record-dialogs/delete-birth-record-dialog.component/delete-birth-record-dialog.component';

type SortColumn =
  | 'registryNumber'
  | 'registryDate'
  | 'bornPersonPesel'
  | 'motherPesel'
  | 'fatherPesel'
  | 'birthdate'
  | 'birthPlace'
  | 'documentName';
type SortDirection = 'asc' | 'desc';

interface SortState {
  column: SortColumn;
  direction: SortDirection;
}

@Component({
  selector: 'app-births-page.component',
  standalone: true,
  imports: [ButtonComponent, TableComponent, AsyncPipe, DatePipe],
  templateUrl: './births-page.component.html',
  styles: ``,
})
export class BirthsPageComponent {
  private readonly dialog = inject(MatDialog);
  private readonly birthRecordsService = inject(BirthRecordsService);

  private readonly sortState$ = new BehaviorSubject<SortState>({
    column: 'registryDate',
    direction: 'desc',
  });

  protected birthsErrorMessage = '';

  private readonly rawBirths$ = this.sortState$.pipe(
    switchMap(() => {
      this.birthsErrorMessage = '';
      return this.birthRecordsService.getBirthRecords().pipe(
        catchError((error) => {
          this.birthsErrorMessage = 'Failed to load birth records.';
          return of([]);
        }),
      );
    }),
  );

  protected readonly births$ = combineLatest([this.rawBirths$, this.sortState$]).pipe(
    map(([births, sortState]) => this.sortBirths(births, sortState)),
  );

  protected openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateBirthRecordDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
    });

    dialogRef.afterClosed().subscribe((created: boolean) => {
      if (!created) {
        return;
      }

      this.reloadCurrentBirths();
    });
  }

  protected openUpdateDialog(birth: BirthRecordViewModel): void {
    const dialogRef = this.dialog.open(UpdateBirthRecordDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: birth,
    });

    dialogRef.afterClosed().subscribe((updated: boolean) => {
      if (!updated) {
        return;
      }

      this.reloadCurrentBirths();
    });
  }

  protected openDeleteDialog(birth: BirthRecordViewModel): void {
    const dialogRef = this.dialog.open(DeleteBirthRecordDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: birth,
    });

    dialogRef.afterClosed().subscribe((deleted: boolean) => {
      if (!deleted) {
        return;
      }

      this.reloadCurrentBirths();
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

  private sortBirths(births: BirthRecordViewModel[], sortState: SortState): BirthRecordViewModel[] {
    const sorted = [...births].sort((a, b) => {
      let aValue: unknown;
      let bValue: unknown;

      switch (sortState.column) {
        case 'registryNumber':
          aValue = a.registryNumber.toLowerCase();
          bValue = b.registryNumber.toLowerCase();
          break;
        case 'registryDate':
          aValue = new Date(a.registryDate).getTime();
          bValue = new Date(b.registryDate).getTime();
          break;
        case 'bornPersonPesel':
          aValue = a.bornPersonPesel.toLowerCase();
          bValue = b.bornPersonPesel.toLowerCase();
          break;
        case 'motherPesel':
          aValue = a.motherPesel.toLowerCase();
          bValue = b.motherPesel.toLowerCase();
          break;
        case 'fatherPesel':
          aValue = a.fatherPesel.toLowerCase();
          bValue = b.fatherPesel.toLowerCase();
          break;
        case 'birthdate':
          aValue = new Date(a.birthDate).getTime();
          bValue = new Date(b.birthDate).getTime();
          break;
        case 'birthPlace':
          aValue = a.birthPlace.toLowerCase();
          bValue = b.birthPlace.toLowerCase();
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

  private reloadCurrentBirths(): void {
    this.birthRecordsService.refreshBirthRecords();
    this.sortState$.next(this.sortState$.value);
  }
}

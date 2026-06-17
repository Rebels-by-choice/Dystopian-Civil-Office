import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

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

  private readonly rawBirths$ = this.birthRecordsService.getBirthRecords().pipe(
    catchError(() => {
      this.birthsErrorMessage = 'Failed to load birth records.';
      return of([]);
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

  protected getMotherPeselDisplayValue(birth: BirthRecordViewModel): string {
    return birth.motherPesel?.trim() || 'No mother PESEL';
  }

  protected getFatherPeselDisplayValue(birth: BirthRecordViewModel): string {
    return birth.fatherPesel?.trim() || 'No father PESEL';
  }

  protected getDocumentNameDisplayValue(birth: BirthRecordViewModel): string {
    return birth.documentName?.trim() || 'No document provided';
  }

  private sortBirths(births: BirthRecordViewModel[], sortState: SortState): BirthRecordViewModel[] {
    return [...births].sort((a, b) => {
      const aValue = this.getSortableBirthValue(a, sortState.column);
      const bValue = this.getSortableBirthValue(b, sortState.column);

      if (aValue === bValue) {
        return 0;
      }

      const comparison =
        typeof aValue === 'number' && typeof bValue === 'number'
          ? aValue - bValue
          : aValue.toString().localeCompare(bValue.toString(), undefined, {
              numeric: true,
              sensitivity: 'base',
            });

      return sortState.direction === 'asc' ? comparison : -comparison;
    });
  }

  private getSortableBirthValue(birth: BirthRecordViewModel, column: SortColumn): string | number {
    switch (column) {
      case 'registryNumber':
        return this.normalizeSortValue(birth.registryNumber);

      case 'registryDate':
        return birth.registryDate
          ? new Date(birth.registryDate).getTime()
          : Number.MIN_SAFE_INTEGER;

      case 'bornPersonPesel':
        return this.normalizeRequiredSortValue(birth.bornPersonPesel);

      case 'motherPesel':
        return this.normalizeSortValue(birth.motherPesel, this.getMotherPeselDisplayValue(birth));

      case 'fatherPesel':
        return this.normalizeSortValue(birth.fatherPesel, this.getFatherPeselDisplayValue(birth));

      case 'birthdate':
        return birth.birthDate ? new Date(birth.birthDate).getTime() : Number.MIN_SAFE_INTEGER;

      case 'birthPlace':
        return this.normalizeSortValue(birth.birthPlace);

      case 'documentName':
        return this.normalizeSortValue(birth.documentName, this.getDocumentNameDisplayValue(birth));

      default:
        return '';
    }
  }

  private normalizeRequiredSortValue(value: string): string {
    return value.trim().toLowerCase();
  }

  private normalizeSortValue(value: string | null | undefined, fallback = ''): string {
    return (value ?? fallback).toString().trim().toLowerCase();
  }

  private reloadCurrentBirths(): void {
    this.birthRecordsService.refreshBirthRecords().subscribe({
      next: () => {
        this.birthsErrorMessage = '';
      },
      error: () => {
        this.birthsErrorMessage = 'Failed to load birth records.';
      },
    });
  }
}

import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { TableComponent } from '../../../shared/ui/table/table.component';
import { DeathRecordsService } from '../../services/death-records.service';
import { CreateDeathRecordDialogComponent } from '../../dialogs/death-record-dialogs/create-death-record-dialog.component/create-death-record-dialog.component';
import { DeathRecordViewModel } from '../../../shared/api-models/responses/deathRecord.viewmodel';
import { UpdateDeathRecordDialogComponent } from '../../dialogs/death-record-dialogs/update-death-record-dialog.component/update-death-record-dialog.component';
import { DeleteDeathRecordDialogComponent } from '../../dialogs/death-record-dialogs/delete-death-record-dialog.component/delete-death-record-dialog.component';

type SortColumn =
  | 'registryNumber'
  | 'registryDate'
  | 'personPesel'
  | 'deathDate'
  | 'deathPlace'
  | 'causeOfDeath'
  | 'documentName';

type SortDirection = 'asc' | 'desc';

interface SortState {
  column: SortColumn;
  direction: SortDirection;
}

@Component({
  selector: 'app-deaths-page.component',
  standalone: true,
  imports: [ButtonComponent, TableComponent, AsyncPipe, DatePipe],
  templateUrl: './deaths-page.component.html',
  styles: ``,
})
export class DeathsPageComponent {
  private readonly dialog = inject(MatDialog);
  private readonly deathRecordsService = inject(DeathRecordsService);

  private readonly sortState$ = new BehaviorSubject<SortState>({
    column: 'registryDate',
    direction: 'desc',
  });

  protected deathsErrorMessage = '';

  private readonly rawDeaths$ = this.deathRecordsService.getDeathRecords().pipe(
    catchError(() => {
      this.deathsErrorMessage = 'Failed to load death records.';
      return of([]);
    }),
  );

  protected readonly deaths$ = combineLatest([this.rawDeaths$, this.sortState$]).pipe(
    map(([deaths, sortState]) => this.sortDeaths(deaths, sortState)),
  );

  protected openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateDeathRecordDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
    });

    dialogRef.afterClosed().subscribe((created: boolean) => {
      if (!created) {
        return;
      }

      this.reloadCurrentDeaths();
    });
  }

  protected openUpdateDialog(death: DeathRecordViewModel): void {
    const dialogRef = this.dialog.open(UpdateDeathRecordDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: death,
    });

    dialogRef.afterClosed().subscribe((updated: boolean) => {
      if (!updated) {
        return;
      }

      this.reloadCurrentDeaths();
    });
  }

  protected openDeleteDialog(death: DeathRecordViewModel): void {
    const dialogRef = this.dialog.open(DeleteDeathRecordDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: death,
    });

    dialogRef.afterClosed().subscribe((deleted: boolean) => {
      if (!deleted) {
        return;
      }

      this.reloadCurrentDeaths();
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

  protected getDocumentNameDisplayValue(death: DeathRecordViewModel): string {
    return death.documentName?.trim() || 'No document provided';
  }

  private sortDeaths(deaths: DeathRecordViewModel[], sortState: SortState): DeathRecordViewModel[] {
    return [...deaths].sort((a, b) => {
      const aValue = this.getSortableDeathValue(a, sortState.column);
      const bValue = this.getSortableDeathValue(b, sortState.column);

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

  private getSortableDeathValue(death: DeathRecordViewModel, column: SortColumn): string | number {
    switch (column) {
      case 'registryNumber':
        return this.normalizeRequiredSortValue(death.registryNumber);

      case 'registryDate':
        return death.registryDate
          ? new Date(death.registryDate).getTime()
          : Number.MIN_SAFE_INTEGER;

      case 'personPesel':
        return this.normalizeRequiredSortValue(death.personPesel);

      case 'deathDate':
        return death.deathDate ? new Date(death.deathDate).getTime() : Number.MIN_SAFE_INTEGER;

      case 'deathPlace':
        return this.normalizeRequiredSortValue(death.deathPlace);

      case 'causeOfDeath':
        return this.normalizeRequiredSortValue(death.causeOfDeath);

      case 'documentName':
        return this.normalizeSortValue(death.documentName, this.getDocumentNameDisplayValue(death));

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

  private reloadCurrentDeaths(): void {
    this.deathRecordsService.refreshDeathRecords().subscribe({
      next: () => {
        this.deathsErrorMessage = '';
      },
      error: () => {
        this.deathsErrorMessage = 'Failed to load death records.';
      },
    });
  }
}

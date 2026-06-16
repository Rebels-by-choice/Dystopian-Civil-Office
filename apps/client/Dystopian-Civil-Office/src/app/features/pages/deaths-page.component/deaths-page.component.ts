import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

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

  private readonly rawDeaths$ = this.sortState$.pipe(
    switchMap(() => {
      this.deathsErrorMessage = '';
      return this.deathRecordsService.getDeathRecords().pipe(
        catchError((error) => {
          this.deathsErrorMessage = 'Failed to load death records.';
          return of([]);
        }),
      );
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

  private sortDeaths(deaths: DeathRecordViewModel[], sortState: SortState): DeathRecordViewModel[] {
    const sorted = [...deaths].sort((a, b) => {
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
        case 'personPesel':
          aValue = a.personPesel.toLowerCase();
          bValue = b.personPesel.toLowerCase();
          break;
        case 'deathDate':
          aValue = new Date(a.deathDate).getTime();
          bValue = new Date(b.deathDate).getTime();
          break;
        case 'deathPlace':
          aValue = a.deathPlace.toLowerCase();
          bValue = b.deathPlace.toLowerCase();
          break;
        case 'causeOfDeath':
          aValue = a.causeOfDeath.toLowerCase();
          bValue = b.causeOfDeath.toLowerCase();
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

  private reloadCurrentDeaths(): void {
    this.deathRecordsService.refreshDeathRecords();
    this.sortState$.next(this.sortState$.value);
  }
}

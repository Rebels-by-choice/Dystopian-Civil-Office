import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { TableComponent } from '../../../shared/ui/table/table.component';
import { MarriagesService } from '../../services/marriages.service';
import { CreateMarriageDialogComponent } from '../../dialogs/marriage-dialogs/create-marriage-dialog.component/create-marriage-dialog.component';
import { MarriageViewModel } from '../../../shared/api-models/responses/marriage.viewmodel';
import { UpdateMarriageDialogComponent } from '../../dialogs/marriage-dialogs/update-marriage-dialog.component/update-marriage-dialog.component';
import { DeleteMarriageDialogComponent } from '../../dialogs/marriage-dialogs/delete-marriage-dialog.component/delete-marriage-dialog.component';

type SortColumn =
  | 'registryNumber'
  | 'registryDate'
  | 'spouse1Pesel'
  | 'spouse2Pesel'
  | 'marriageDate'
  | 'marriagePlace'
  | 'documentName';
type SortDirection = 'asc' | 'desc';

interface SortState {
  column: SortColumn;
  direction: SortDirection;
}

@Component({
  selector: 'app-marriages-page.component',
  standalone: true,
  imports: [ButtonComponent, TableComponent, AsyncPipe, DatePipe],
  templateUrl: './marriages-page.component.html',
  styles: ``,
})
export class MarriagesPageComponent {
  private readonly dialog = inject(MatDialog);
  private readonly marriagesService = inject(MarriagesService);

  private readonly sortState$ = new BehaviorSubject<SortState>({
    column: 'registryDate',
    direction: 'desc',
  });

  protected marriagesErrorMessage = '';

  private readonly rawMarriages$ = this.sortState$.pipe(
    switchMap(() => {
      this.marriagesErrorMessage = '';
      return this.marriagesService.getMarriages().pipe(
        catchError((error) => {
          this.marriagesErrorMessage = 'Failed to load marriages.';
          return of([]);
        }),
      );
    }),
  );

  protected readonly marriages$ = combineLatest([this.rawMarriages$, this.sortState$]).pipe(
    map(([marriages, sortState]) => this.sortMarriages(marriages, sortState)),
  );

  protected openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateMarriageDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
    });

    dialogRef.afterClosed().subscribe((created: boolean) => {
      if (!created) {
        return;
      }

      this.reloadCurrentMarriages();
    });
  }

  protected openUpdateDialog(marriage: MarriageViewModel): void {
    const dialogRef = this.dialog.open(UpdateMarriageDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: marriage,
    });

    dialogRef.afterClosed().subscribe((updated: boolean) => {
      if (!updated) {
        return;
      }

      this.reloadCurrentMarriages();
    });
  }

  protected openDeleteDialog(marriage: MarriageViewModel): void {
    const dialogRef = this.dialog.open(DeleteMarriageDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: marriage,
    });

    dialogRef.afterClosed().subscribe((deleted: boolean) => {
      if (!deleted) {
        return;
      }

      this.reloadCurrentMarriages();
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

  private sortMarriages(marriages: MarriageViewModel[], sortState: SortState): MarriageViewModel[] {
    const sorted = [...marriages].sort((a, b) => {
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
        case 'spouse1Pesel':
          aValue = a.spouse1Pesel.toLowerCase();
          bValue = b.spouse1Pesel.toLowerCase();
          break;
        case 'spouse2Pesel':
          aValue = a.spouse2Pesel.toLowerCase();
          bValue = b.spouse2Pesel.toLowerCase();
          break;
        case 'marriageDate':
          aValue = new Date(a.marriageDate).getTime();
          bValue = new Date(b.marriageDate).getTime();
          break;
        case 'marriagePlace':
          aValue = a.marriagePlace.toLowerCase();
          bValue = b.marriagePlace.toLowerCase();
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

  private reloadCurrentMarriages(): void {
    this.marriagesService.refreshMarriages();
    this.sortState$.next(this.sortState$.value);
  }
}

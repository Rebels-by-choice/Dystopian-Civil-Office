import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

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

  private readonly rawMarriages$ = this.marriagesService.getMarriages().pipe(
    catchError(() => {
      this.marriagesErrorMessage = 'Failed to load marriages.';
      return of([]);
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

  protected getSpouse1PeselDisplayValue(marriage: MarriageViewModel): string {
    return marriage.spouse1Pesel?.trim() || 'No spouse 1 PESEL';
  }

  protected getSpouse2PeselDisplayValue(marriage: MarriageViewModel): string {
    return marriage.spouse2Pesel?.trim() || 'No spouse 2 PESEL';
  }

  protected getDocumentNameDisplayValue(marriage: MarriageViewModel): string {
    return marriage.documentName?.trim() || 'No document provided';
  }

  private sortMarriages(marriages: MarriageViewModel[], sortState: SortState): MarriageViewModel[] {
    return [...marriages].sort((a, b) => {
      const aValue = this.getSortableMarriageValue(a, sortState.column);
      const bValue = this.getSortableMarriageValue(b, sortState.column);

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

  private getSortableMarriageValue(
    marriage: MarriageViewModel,
    column: SortColumn,
  ): string | number {
    switch (column) {
      case 'registryNumber':
        return this.normalizeSortValue(marriage.registryNumber);

      case 'registryDate':
        return marriage.registryDate
          ? new Date(marriage.registryDate).getTime()
          : Number.MIN_SAFE_INTEGER;

      case 'spouse1Pesel':
        return this.normalizeSortValue(
          marriage.spouse1Pesel,
          this.getSpouse1PeselDisplayValue(marriage),
        );

      case 'spouse2Pesel':
        return this.normalizeSortValue(
          marriage.spouse2Pesel,
          this.getSpouse2PeselDisplayValue(marriage),
        );

      case 'marriageDate':
        return marriage.marriageDate
          ? new Date(marriage.marriageDate).getTime()
          : Number.MIN_SAFE_INTEGER;

      case 'marriagePlace':
        return this.normalizeSortValue(marriage.marriagePlace);

      case 'documentName':
        return this.normalizeSortValue(
          marriage.documentName,
          this.getDocumentNameDisplayValue(marriage),
        );

      default:
        return '';
    }
  }

  private normalizeSortValue(value: string | null | undefined, fallback = ''): string {
    return (value ?? fallback).toString().trim().toLowerCase();
  }

  private reloadCurrentMarriages(): void {
    this.marriagesService.refreshMarriages().subscribe({
      next: () => {
        this.marriagesErrorMessage = '';
      },
      error: () => {
        this.marriagesErrorMessage = 'Failed to load marriages.';
      },
    });
  }
}

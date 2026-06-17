import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { TableComponent } from '../../../shared/ui/table/table.component';
import { PersonsService } from '../../services/persons.service';
import { CreatePersonDialogComponent } from '../../dialogs/person-dialogs/create-person-dialog.component/create-person-dialog.component';
import { PersonViewModel } from '../../../shared/api-models/responses/person.viewmodel';
import { UpdatePersonDialogComponent } from '../../dialogs/person-dialogs/update-person-dialog.component/update-person-dialog.component';
import { DeletePersonDialogComponent } from '../../dialogs/person-dialogs/delete-person-dialog.component/delete-person-dialog.component';

type SortColumn =
  | 'personPesel'
  | 'firstName'
  | 'middleName'
  | 'lastName'
  | 'gender'
  | 'birthDate'
  | 'birthPlace'
  | 'addressRegistryNumber'
  | 'documentName';

type SortDirection = 'asc' | 'desc';

interface SortState {
  column: SortColumn;
  direction: SortDirection;
}

@Component({
  selector: 'app-persons-page.component',
  standalone: true,
  imports: [ButtonComponent, TableComponent, AsyncPipe, DatePipe, FormsModule],
  templateUrl: './persons-page.component.html',
  styles: ``,
})
export class PersonsPageComponent {
  private readonly dialog = inject(MatDialog);
  private readonly personsService = inject(PersonsService);

  private readonly sortState$ = new BehaviorSubject<SortState>({
    column: 'birthDate',
    direction: 'desc',
  });

  private readonly activeGenderFilter$ = new BehaviorSubject<string | null>(null);
  private readonly refreshTrigger$ = new BehaviorSubject<number>(0);

  protected personsErrorMessage = '';
  protected selectedGender = 'Default';

  protected readonly genderOptions: string[] = ['Default', 'Male', 'Female', 'Other'];

  private readonly rawPersons$ = combineLatest([
    this.activeGenderFilter$,
    this.refreshTrigger$,
  ]).pipe(
    switchMap(([gender]) => {
      this.personsErrorMessage = '';

      const request$ = gender
        ? this.personsService.refreshPersonsByGender(gender)
        : this.personsService.refreshPersons();

      return request$.pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 0) {
            this.personsErrorMessage =
              'Cannot connect to server. Check whether the backend is running.';
          } else if (error.error?.description) {
            this.personsErrorMessage = error.error.description;
          } else if (error.error?.title) {
            this.personsErrorMessage = error.error.title;
          } else {
            this.personsErrorMessage = `Failed to load persons. HTTP Error ${error.status}.`;
          }

          return of([]);
        }),
      );
    }),
  );

  protected readonly persons$ = combineLatest([this.rawPersons$, this.sortState$]).pipe(
    map(([persons, sortState]) => this.sortPersons(persons, sortState)),
  );

  protected openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreatePersonDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
    });

    dialogRef.afterClosed().subscribe((created: boolean) => {
      if (!created) {
        return;
      }

      this.reloadCurrentPersons();
    });
  }

  protected openUpdateDialog(person: PersonViewModel): void {
    const dialogRef = this.dialog.open(UpdatePersonDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: person,
    });

    dialogRef.afterClosed().subscribe((updated: boolean) => {
      if (!updated) {
        return;
      }

      this.reloadCurrentPersons();
    });
  }

  protected openDeleteDialog(person: PersonViewModel): void {
    const dialogRef = this.dialog.open(DeletePersonDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: person,
    });

    dialogRef.afterClosed().subscribe((deleted: boolean) => {
      if (!deleted) {
        return;
      }

      this.reloadCurrentPersons();
    });
  }

  protected applyGenderFilter(): void {
    const normalizedGender = this.selectedGender.trim();

    if (!normalizedGender || normalizedGender === 'Default') {
      this.resetGenderFilterState();
      return;
    }

    this.activeGenderFilter$.next(normalizedGender);
  }

  protected resetGenderFilter(): void {
    this.resetGenderFilterState();
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

  protected getMiddleNameDisplayValue(person: PersonViewModel): string {
    return person.middleName?.trim() || 'No middle name';
  }

  protected getAddressRegistryNumberDisplayValue(person: PersonViewModel): string {
    return person.addressRegistryNumber?.trim() || 'No address provided';
  }

  protected getDocumentNameDisplayValue(person: PersonViewModel): string {
    return person.documentName?.trim() || 'No document provided';
  }

  private sortPersons(persons: PersonViewModel[], sortState: SortState): PersonViewModel[] {
    return [...persons].sort((a, b) => {
      const aValue = this.getSortablePersonValue(a, sortState.column);
      const bValue = this.getSortablePersonValue(b, sortState.column);

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

  private getSortablePersonValue(person: PersonViewModel, column: SortColumn): string | number {
    switch (column) {
      case 'personPesel':
        return this.normalizeRequiredSortValue(person.personPesel);

      case 'firstName':
        return this.normalizeRequiredSortValue(person.firstName);

      case 'middleName':
        return this.normalizeSortValue(person.middleName, this.getMiddleNameDisplayValue(person));

      case 'lastName':
        return this.normalizeRequiredSortValue(person.lastName);

      case 'gender':
        return this.normalizeRequiredSortValue(person.gender);

      case 'birthDate':
        return person.birthDate ? new Date(person.birthDate).getTime() : Number.MIN_SAFE_INTEGER;

      case 'birthPlace':
        return this.normalizeRequiredSortValue(person.birthPlace);

      case 'addressRegistryNumber':
        return this.normalizeSortValue(
          person.addressRegistryNumber,
          this.getAddressRegistryNumberDisplayValue(person),
        );

      case 'documentName':
        return this.normalizeSortValue(
          person.documentName,
          this.getDocumentNameDisplayValue(person),
        );

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

  private resetGenderFilterState(): void {
    this.selectedGender = 'Default';
    this.activeGenderFilter$.next(null);
  }

  private reloadCurrentPersons(): void {
    this.refreshTrigger$.next(this.refreshTrigger$.value + 1);
  }
}

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

  protected personsErrorMessage = '';
  protected selectedGender = 'Default';

  protected readonly genderOptions: string[] = ['Default', 'Male', 'Female', 'Other'];

  private readonly rawPersons$ = this.activeGenderFilter$.pipe(
    switchMap((gender) => {
      this.personsErrorMessage = '';

      const request$ = gender
        ? this.personsService.getPersonsByGender(gender)
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

  private sortPersons(persons: PersonViewModel[], sortState: SortState): PersonViewModel[] {
    const sorted = [...persons].sort((a, b) => {
      let aValue: unknown;
      let bValue: unknown;

      switch (sortState.column) {
        case 'personPesel':
          aValue = a.personPesel.toLowerCase();
          bValue = b.personPesel.toLowerCase();
          break;
        case 'firstName':
          aValue = a.firstName.toLowerCase();
          bValue = b.firstName.toLowerCase();
          break;
        case 'middleName':
          aValue = a.middleName.toLowerCase();
          bValue = b.middleName.toLowerCase();
          break;
        case 'lastName':
          aValue = a.lastName.toLowerCase();
          bValue = b.lastName.toLowerCase();
          break;
        case 'gender':
          aValue = a.gender.toLowerCase();
          bValue = b.gender.toLowerCase();
          break;
        case 'birthDate':
          aValue = new Date(a.birthDate).getTime();
          bValue = new Date(b.birthDate).getTime();
          break;
        case 'birthPlace':
          aValue = a.birthPlace.toLowerCase();
          bValue = b.birthPlace.toLowerCase();
          break;
        case 'addressRegistryNumber':
          aValue = a.addressRegistryNumber.toLowerCase();
          bValue = b.addressRegistryNumber.toLowerCase();
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

  private resetGenderFilterState(): void {
    this.selectedGender = 'Default';
    this.activeGenderFilter$.next(null);
  }

  private reloadCurrentPersons(): void {
    const currentGender = this.activeGenderFilter$.value;

    if (currentGender) {
      this.personsService.refreshPersonsByGender(currentGender);
    } else {
      this.personsService.refreshPersons();
    }

    this.activeGenderFilter$.next(currentGender);
  }
}

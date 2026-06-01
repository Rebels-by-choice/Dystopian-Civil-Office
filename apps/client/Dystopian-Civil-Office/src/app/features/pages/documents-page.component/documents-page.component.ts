import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { DocumentsService } from '../../services/documents.service';
import { CreateDocumentDialogComponent } from '../../dialogues/create-document-dialog.component/create-document-dialog.component';
import { DocumentViewModel } from '../../../shared/api-models/responses/document.viewmodel';

type SortColumn = 'name' | 'category' | 'importDate';
type SortDirection = 'asc' | 'desc';

interface SortState {
  column: SortColumn;
  direction: SortDirection;
}

@Component({
  selector: 'app-documents-page.component',
  standalone: true,
  imports: [ButtonComponent, AsyncPipe, DatePipe],
  templateUrl: './documents-page.component.html',
  styles: ``,
})
export class DocumentsPageComponent {
  private readonly dialog = inject(MatDialog);
  private readonly documentsService = inject(DocumentsService);

  private readonly refreshDocumentsTrigger$ = new BehaviorSubject<void>(undefined);

  private readonly sortState$ = new BehaviorSubject<SortState>({
    column: 'importDate',
    direction: 'desc',
  });

  protected documentsErrorMessage = '';

  private readonly rawDocuments$ = this.refreshDocumentsTrigger$.pipe(
    switchMap(() => {
      this.documentsErrorMessage = '';

      return this.documentsService.refreshDocuments().pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 0) {
            this.documentsErrorMessage =
              'Cannot connect to server. Check whether the backend is running.';
          } else {
            this.documentsErrorMessage = `Failed to load documents. HTTP Error ${error.status}.`;
          }

          return of([]);
        }),
      );
    }),
  );

  protected readonly documents$ = combineLatest([this.rawDocuments$, this.sortState$]).pipe(
    map(([documents, sortState]) => this.sortDocuments(documents, sortState)),
  );

  protected openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateDocumentDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
    });

    dialogRef.afterClosed().subscribe((created: boolean) => {
      if (!created) {
        return;
      }

      setTimeout(() => {
        this.refreshDocumentsTrigger$.next();
      }, 0);
    });
  }

  protected sortBy(column: SortColumn): void {
    const currentSort = this.sortState$.value;

    if (currentSort.column === column) {
      this.sortState$.next({
        column,
        direction: currentSort.direction === 'asc' ? 'desc' : 'asc',
      });

      return;
    }

    this.sortState$.next({
      column,
      direction: 'asc',
    });
  }

  protected getSortDirection(column: SortColumn): SortDirection | null {
    const currentSort = this.sortState$.value;
    return currentSort.column === column ? currentSort.direction : null;
  }

  private sortDocuments(documents: DocumentViewModel[], sortState: SortState): DocumentViewModel[] {
    const sorted = [...documents];

    sorted.sort((a, b) => {
      let comparison = 0;

      switch (sortState.column) {
        case 'name':
          comparison = a.name.localeCompare(b.name);
          break;

        case 'category':
          comparison = a.category.localeCompare(b.category);
          break;

        case 'importDate':
          comparison = new Date(a.importDate).getTime() - new Date(b.importDate).getTime();
          break;
      }

      return sortState.direction === 'asc' ? comparison : -comparison;
    });

    return sorted;
  }
}

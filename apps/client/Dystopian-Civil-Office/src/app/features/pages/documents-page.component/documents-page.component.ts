import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { TableComponent } from '../../../shared/ui/table/table.component';
import DocumentsService from '../../services/documents.service';
import { CreateDocumentDialogComponent } from '../../dialogs/document-dialogs/create-document-dialog.component/create-document-dialog.component';
import { DocumentViewModel } from '../../../shared/api-models/responses/document.viewmodel';
import { UpdateDocumentDialogComponent } from '../../dialogs/document-dialogs/update-document-dialog.component/update-document-dialog.component';
import { DeleteDocumentDialogComponent } from '../../dialogs/document-dialogs/delete-document-dialog.component/delete-document-dialog.component';
type SortColumn = 'name' | 'category' | 'importDate';
type SortDirection = 'asc' | 'desc';

interface SortState {
  column: SortColumn;
  direction: SortDirection;
}

@Component({
  selector: 'app-documents-page.component',
  standalone: true,
  imports: [ButtonComponent, TableComponent, AsyncPipe, DatePipe, FormsModule],
  templateUrl: './documents-page.component.html',
  styles: ``,
})
export class DocumentsPageComponent {
  private readonly dialog = inject(MatDialog);
  private readonly documentService = inject(DocumentsService);

  private readonly sortState$ = new BehaviorSubject<SortState>({
    column: 'importDate',
    direction: 'desc',
  });

  private readonly activeCategoryFilter$ = new BehaviorSubject<string | null>(null);

  protected documentsErrorMessage = '';
  protected selectedCategory = 'Default';

  protected readonly categoryOptions: string[] = [
    'Default',
    'BirthRecord',
    'Person',
    'Address',
    'DeathRecord',
    'MarriageRecord',
  ];

  private readonly rawDocuments$ = this.activeCategoryFilter$.pipe(
    switchMap((category) => {
      this.documentsErrorMessage = '';

      const request$ = category
        ? this.documentService.getDocumentsByCategory(category)
        : this.documentService.refreshDocuments();

      return request$.pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 0) {
            this.documentsErrorMessage =
              'Cannot connect to server. Check whether the backend is running.';
          } else if (error.error?.description) {
            this.documentsErrorMessage = error.error.description;
          } else if (error.error?.title) {
            this.documentsErrorMessage = error.error.title;
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

      this.reloadCurrentDocuments();
    });
  }

  protected openUpdateDialog(document: DocumentViewModel): void {
    const dialogRef = this.dialog.open(UpdateDocumentDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: document,
    });

    dialogRef.afterClosed().subscribe((updated: boolean) => {
      if (!updated) {
        return;
      }

      this.reloadCurrentDocuments();
    });
  }

  protected openDeleteDialog(document: DocumentViewModel): void {
    const dialogRef = this.dialog.open(DeleteDocumentDialogComponent, {
      disableClose: true,
      panelClass: 'document-dialog-panel',
      data: document,
    });

    dialogRef.afterClosed().subscribe((deleted: boolean) => {
      if (!deleted) {
        return;
      }

      this.reloadCurrentDocuments();
    });
  }

  protected applyCategoryFilter(): void {
    const normalizedCategory = this.selectedCategory.trim();

    if (normalizedCategory === 'Default') {
      this.resetCategoryFilterState();
      return;
    }

    this.activeCategoryFilter$.next(normalizedCategory);
  }

  protected resetCategoryFilter(): void {
    this.resetCategoryFilterState();
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

  private sortDocuments(documents: DocumentViewModel[], sortState: SortState): DocumentViewModel[] {
    const sorted = [...documents].sort((a, b) => {
      let aValue: unknown;
      let bValue: unknown;

      switch (sortState.column) {
        case 'name':
          aValue = a.name.toLowerCase();
          bValue = b.name.toLowerCase();
          break;
        case 'category':
          aValue = a.category.toLowerCase();
          bValue = b.category.toLowerCase();
          break;
        case 'importDate':
          aValue = new Date(a.importDate).getTime();
          bValue = new Date(b.importDate).getTime();
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

  private resetCategoryFilterState(): void {
    this.selectedCategory = 'Default';
    this.activeCategoryFilter$.next(null);
  }

  private reloadCurrentDocuments(): void {
    const currentCategory = this.activeCategoryFilter$.value;

    if (currentCategory) {
      this.documentService.refreshDocumentsByCategory(currentCategory);
    } else {
      this.documentService.refreshDocuments();
    }

    this.activeCategoryFilter$.next(currentCategory);
  }

  openDocumentPreview(paperlessDocumentId: number) {
    this.documentService.getDocumentPreview(paperlessDocumentId).subscribe({
      next: (blob: Blob) => {
        const objectUrl = window.URL.createObjectURL(blob);

        // Open the preview in a new tab
        window.open(objectUrl, '_blank');

        // window.URL.revokeObjectURL(objectUrl);
      },
      error: (err) => console.error('Failed to load document preview', err),
    });
  }
}

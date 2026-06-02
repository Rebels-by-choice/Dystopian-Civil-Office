import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { DocumentsService } from '../../services/documents.service';
import { CreateDocumentDialogComponent } from '../../dialogues/create-document-dialog.component/create-document-dialog.component';
import { DocumentViewModel } from '../../../shared/api-models/responses/document.viewmodel';
import { UpdateDocumentDialogComponent } from '../../dialogues/update-document-dialog.component/update-document-dialog.component';
import { DeleteDocumentDialogComponent } from '../../dialogues/delete-document-dialog.component/delete-document-dialog.component';

type SortColumn = 'name' | 'category' | 'importDate';
type SortDirection = 'asc' | 'desc';

interface SortState {
  column: SortColumn;
  direction: SortDirection;
}

@Component({
  selector: 'app-documents-page.component',
  standalone: true,
  imports: [ButtonComponent, AsyncPipe, DatePipe, FormsModule],
  templateUrl: './documents-page.component.html',
  styles: ``,
})
export class DocumentsPageComponent {
  private readonly dialog = inject(MatDialog);
  private readonly documentsService = inject(DocumentsService);

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
        ? this.documentsService.getDocumentsByCategory(category)
        : this.documentsService.refreshDocuments();

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

  private reloadCurrentDocuments(): void {
    const activeCategory = this.activeCategoryFilter$.value;

    if (activeCategory) {
      this.documentsService.refreshDocumentsByCategory(activeCategory);
      this.activeCategoryFilter$.next(activeCategory);
      return;
    }

    this.documentsService.clearCache();
    this.activeCategoryFilter$.next(null);
  }

  private resetCategoryFilterState(): void {
    this.selectedCategory = 'Default';
    this.documentsService.clearCache();
    this.activeCategoryFilter$.next(null);
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

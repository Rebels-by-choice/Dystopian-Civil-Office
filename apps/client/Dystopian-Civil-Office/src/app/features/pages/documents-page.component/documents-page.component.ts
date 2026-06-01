import { Component, inject } from '@angular/core';
import { AsyncPipe, DatePipe } from '@angular/common';

import { ButtonComponent } from '../../../shared/ui/button.component/button.component';
import { DocumentsService } from '../../services/documents.service';
import { MatDialog } from '@angular/material/dialog';
import { CreateDocumentDialogComponent } from '../../dialogues/create-document-dialog.component/create-document-dialog.component';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { catchError, of, switchMap, tap } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

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

  protected documentsErrorMessage = '';

  protected readonly documents$ = this.refreshDocumentsTrigger$.pipe(
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
}

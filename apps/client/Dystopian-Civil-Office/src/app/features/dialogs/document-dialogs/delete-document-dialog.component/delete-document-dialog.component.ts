import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import DocumentsService from '../../../services/documents.service';
import { DocumentViewModel } from '../../../../shared/api-models/responses/document.viewmodel';
import { DocumentFormValidators } from '../../../../shared/validators/document-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';

@Component({
  selector: 'app-delete-document-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, ButtonComponent, DialogShellComponent],
  templateUrl: 'delete-document-dialog.component.html',
})
export class DeleteDocumentDialogComponent {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<DeleteDocumentDialogComponent>);
  private readonly documentsService = inject(DocumentsService);

  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: DocumentViewModel) {}

  protected onCancel(): void {
    if (this.isSubmitting) {
      return;
    }

    this.dialogRef.close(false);
  }

  protected onConfirm(): void {
    this.apiErrorMessage = '';

    if (!Number.isInteger(this.data.documentId)) {
      this.apiErrorMessage = 'Document identifier is missing.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.documentsService.deleteDocument(this.data.documentId).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.cdr.detectChanges();
        this.dialogRef.close(true);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.apiErrorMessage = DocumentFormValidators.getApiErrorMessage(error);
        this.cdr.detectChanges();
      },
    });
  }
}

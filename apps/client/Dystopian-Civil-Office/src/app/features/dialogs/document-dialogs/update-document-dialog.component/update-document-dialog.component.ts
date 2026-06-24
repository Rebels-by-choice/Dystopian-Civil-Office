import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import DocumentsService from '../../../services/documents.service';
import { UpdateDocumentModel } from '../../../../shared/api-models/requests/document.model';
import { DocumentViewModel } from '../../../../shared/api-models/responses/document.viewmodel';
import { DocumentFormValidators } from '../../../../shared/validators/document-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';

@Component({
  selector: 'app-update-document-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    ButtonComponent,
    DialogShellComponent,
  ],
  templateUrl: './update-document-dialog.component.html',
})
export class UpdateDocumentDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<UpdateDocumentDialogComponent>);
  private readonly documentsService = inject(DocumentsService);

  protected form!: FormGroup;
  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: DocumentViewModel) {}

  public ngOnInit(): void {
    this.form = this.fb.group({
      name: [this.data.name, DocumentFormValidators.nameValidators()],
      category: [this.data.category, DocumentFormValidators.categoryValidators()],
    });
  }

  protected get nameError(): string {
    return DocumentFormValidators.getControlErrorMessage(this.form.controls['name'], 'Name');
  }

  protected get categoryError(): string {
    return DocumentFormValidators.getControlErrorMessage(
      this.form.controls['category'],
      'Category',
    );
  }

  protected onCancel(): void {
    if (this.isSubmitting) {
      return;
    }

    this.dialogRef.close(false);
  }

  protected onConfirm(): void {
    this.apiErrorMessage = '';

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.cdr.detectChanges();
      return;
    }

    if (!DocumentFormValidators.hasDocumentChanges(this.form, this.data.name, this.data.category)) {
      this.apiErrorMessage =
        'Provided data is the same as current. Please make changes before submitting.';
      this.cdr.detectChanges();
      return;
    }

    const trimmedName = this.form.controls['name'].value!.trim();
    const trimmedCategory = this.form.controls['category'].value!.trim();

    const request: UpdateDocumentModel = {};

    if (trimmedName !== this.data.name.trim()) {
      request.name = trimmedName;
    }

    if (trimmedCategory !== this.data.category.trim()) {
      request.category = trimmedCategory;
    }

    if (this.data.documentId == null) {
      this.apiErrorMessage = 'Document ID is missing. Cannot update this record.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.documentsService.updateDocument(this.data.documentId, request).subscribe({
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

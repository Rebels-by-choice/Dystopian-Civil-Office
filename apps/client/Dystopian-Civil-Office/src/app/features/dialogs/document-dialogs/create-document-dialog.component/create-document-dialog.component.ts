import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import DocumentsService from '../../../services/documents.service';
import { CreateDocumentModel } from '../../../../shared/api-models/requests/document.model';
import { DocumentFormValidators } from '../../../../shared/validators/document-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';

@Component({
  selector: 'app-create-document-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    ButtonComponent,
    DialogShellComponent,
  ],
  templateUrl: './create-document-dialog.component.html',
})
export class CreateDocumentDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<CreateDocumentDialogComponent>);
  private readonly documentsService = inject(DocumentsService);

  protected readonly form = this.fb.group({
    name: ['', DocumentFormValidators.nameValidators()],
    category: ['', DocumentFormValidators.categoryValidators()],
  });

  protected isSubmitting = false;
  protected apiErrorMessage = '';

  protected get nameError(): string {
    return DocumentFormValidators.getControlErrorMessage(this.form.controls.name, 'Name');
  }

  protected get categoryError(): string {
    return DocumentFormValidators.getControlErrorMessage(this.form.controls.category, 'Category');
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

    const request: CreateDocumentModel = {
      name: this.form.controls.name.value!.trim(),
      category: this.form.controls.category.value!.trim(),
      importDate: new Date().toISOString(),
    };

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.documentsService.createDocument(request).subscribe({
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

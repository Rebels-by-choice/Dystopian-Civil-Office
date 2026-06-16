import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import { BirthRecordsService } from '../../../services/birth-records.service';
import { BirthRecordViewModel } from '../../../../shared/api-models/responses/birthRecord.viewmodel';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-delete-birth-record-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, ButtonComponent, DialogShellComponent],
  templateUrl: './delete-birth-record-dialog.component.html',
})
export class DeleteBirthRecordDialogComponent {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<DeleteBirthRecordDialogComponent>);
  private readonly birthRecordsService = inject(BirthRecordsService);

  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: BirthRecordViewModel) {}

  protected onCancel(): void {
    if (this.isSubmitting) {
      return;
    }

    this.dialogRef.close(false);
  }

  protected onConfirm(): void {
    this.apiErrorMessage = '';

    if (!Number.isInteger(this.data.birthRecordId)) {
      this.apiErrorMessage = 'Birth record identifier is missing.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.birthRecordsService.deleteBirthRecord(this.data.birthRecordId).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.cdr.detectChanges();
        this.dialogRef.close(true);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.apiErrorMessage = GlobalFormValidators.getApiErrorMessage(error);
        this.cdr.detectChanges();
      },
    });
  }
}

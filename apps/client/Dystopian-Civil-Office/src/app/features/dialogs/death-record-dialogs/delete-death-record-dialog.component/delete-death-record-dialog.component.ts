import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import { DeathRecordsService } from '../../../services/death-records.service';
import { DeathRecordViewModel } from '../../../../shared/api-models/responses/deathRecord.viewmodel';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-delete-death-record-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, ButtonComponent, DialogShellComponent],
  templateUrl: './delete-death-record-dialog.component.html',
})
export class DeleteDeathRecordDialogComponent {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<DeleteDeathRecordDialogComponent>);
  private readonly deathRecordsService = inject(DeathRecordsService);

  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: DeathRecordViewModel) {}

  protected onCancel(): void {
    if (this.isSubmitting) {
      return;
    }

    this.dialogRef.close(false);
  }

  protected onConfirm(): void {
    this.apiErrorMessage = '';

    if (!Number.isInteger(this.data.deathRecordId)) {
      this.apiErrorMessage = 'Death record identifier is missing.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.deathRecordsService.deleteDeathRecord(this.data.deathRecordId).subscribe({
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

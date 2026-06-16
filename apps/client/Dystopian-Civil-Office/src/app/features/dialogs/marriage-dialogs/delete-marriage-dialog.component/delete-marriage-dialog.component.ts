import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import { MarriagesService } from '../../../services/marriages.service';
import { MarriageViewModel } from '../../../../shared/api-models/responses/marriage.viewmodel';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-delete-marriage-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, ButtonComponent, DialogShellComponent],
  templateUrl: './delete-marriage-dialog.component.html',
})
export class DeleteMarriageDialogComponent {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<DeleteMarriageDialogComponent>);
  private readonly marriagesService = inject(MarriagesService);

  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: MarriageViewModel) {}

  protected onCancel(): void {
    if (this.isSubmitting) {
      return;
    }

    this.dialogRef.close(false);
  }

  protected onConfirm(): void {
    this.apiErrorMessage = '';

    if (!Number.isInteger(this.data.marriageRecordId)) {
      this.apiErrorMessage = 'Marriage record identifier is missing.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.marriagesService.deleteMarriage(this.data.marriageRecordId).subscribe({
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

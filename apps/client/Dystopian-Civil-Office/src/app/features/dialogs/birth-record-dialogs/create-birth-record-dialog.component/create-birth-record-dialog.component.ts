import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

import { BirthRecordsService } from '../../../services/birth-records.service';
import { CreateBirthRecordModel } from '../../../../shared/api-models/requests/birthRecord.model';
import { BirthRecordsFormValidators } from '../../../../shared/validators/birthRecord-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-create-birth-record-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    ButtonComponent,
    DialogShellComponent,
  ],
  templateUrl: './create-birth-record-dialog.component.html',
})
export class CreateBirthRecordDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<CreateBirthRecordDialogComponent>);
  private readonly birthRecordsService = inject(BirthRecordsService);

  protected readonly form = this.fb.group({
    registryNumber: ['', BirthRecordsFormValidators.registryNumberValidators()],
    personPesel: ['', BirthRecordsFormValidators.personPeselValidators()],
    motherPesel: ['', BirthRecordsFormValidators.relativePeselValidators()],
    fatherPesel: ['', BirthRecordsFormValidators.relativePeselValidators()],
    documentName: ['', BirthRecordsFormValidators.documentNameValidators()],
  });

  protected isSubmitting = false;
  protected apiErrorMessage = '';

  protected get registryNumberError(): string {
    return BirthRecordsFormValidators.getControlErrorMessage(
      this.form.controls['registryNumber'],
      'Registry number',
    );
  }

  protected get personPeselError(): string {
    return BirthRecordsFormValidators.getControlErrorMessage(
      this.form.controls['personPesel'],
      'Person PESEL',
    );
  }

  protected get motherPeselError(): string {
    return BirthRecordsFormValidators.getControlErrorMessage(
      this.form.controls['motherPesel'],
      'Mother PESEL',
    );
  }

  protected get fatherPeselError(): string {
    return BirthRecordsFormValidators.getControlErrorMessage(
      this.form.controls['fatherPesel'],
      'Father PESEL',
    );
  }

  protected get documentNameError(): string {
    return BirthRecordsFormValidators.getControlErrorMessage(
      this.form.controls['documentName'],
      'Document name',
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

    const request: CreateBirthRecordModel = {
      registryNumber: this.form.controls['registryNumber'].value!.trim(),
      personPesel: this.form.controls['personPesel'].value!.trim(),
      motherPesel: this.form.controls['motherPesel'].value?.trim() ?? '',
      fatherPesel: this.form.controls['fatherPesel'].value?.trim() ?? '',
      registryDate: new Date(),
      documentName: this.form.controls['documentName'].value?.trim() ?? '',
    };

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.birthRecordsService.createBirthRecord(request).subscribe({
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

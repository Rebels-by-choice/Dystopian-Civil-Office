import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { DeathRecordsService } from '../../../services/death-records.service';
import { CreateBirthRecordModel } from '../../../../shared/api-models/requests/deathRecord.model';
import { DeathRecordFormValidators } from '../../../../shared/validators/deathRecord-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-create-death-record-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ButtonComponent,
    DialogShellComponent,
  ],
  templateUrl: './create-death-record-dialog.component.html',
})
export class CreateDeathRecordDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<CreateDeathRecordDialogComponent>);
  private readonly deathRecordsService = inject(DeathRecordsService);

  protected readonly form = this.fb.group({
    registryNumber: ['', DeathRecordFormValidators.registryNumberValidators()],
    personPesel: ['', DeathRecordFormValidators.personPeselValidators()],
    deathDate: [null, DeathRecordFormValidators.deathDateValidators()],
    deathPlace: ['', DeathRecordFormValidators.deathPlaceValidators()],
    causeOfDeath: ['', DeathRecordFormValidators.causeOfDeathValidators()],
    documentName: ['', DeathRecordFormValidators.documentNameValidators()],
  });

  protected isSubmitting = false;
  protected apiErrorMessage = '';

  protected get registryNumberError(): string {
    return DeathRecordFormValidators.getControlErrorMessage(
      this.form.controls['registryNumber'],
      'Registry number',
    );
  }

  protected get personPeselError(): string {
    return DeathRecordFormValidators.getControlErrorMessage(
      this.form.controls['personPesel'],
      'Person PESEL',
    );
  }

  protected get deathDateError(): string {
    return DeathRecordFormValidators.getControlErrorMessage(
      this.form.controls['deathDate'],
      'Death date',
    );
  }

  protected get deathPlaceError(): string {
    return DeathRecordFormValidators.getControlErrorMessage(
      this.form.controls['deathPlace'],
      'Death place',
    );
  }

  protected get causeOfDeathError(): string {
    return DeathRecordFormValidators.getControlErrorMessage(
      this.form.controls['causeOfDeath'],
      'Cause of death',
    );
  }

  protected get documentNameError(): string {
    return DeathRecordFormValidators.getControlErrorMessage(
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

    const deathDate = this.form.controls['deathDate'].value;

    if (!deathDate) {
      this.apiErrorMessage = 'Death date is required.';
      this.cdr.detectChanges();
      return;
    }

    const request: CreateBirthRecordModel = {
      RegistryNumber: this.form.controls['registryNumber'].value!.trim(),
      PersonPesel: this.form.controls['personPesel'].value!.trim(),
      DeathDate: deathDate,
      DeathPlace: this.form.controls['deathPlace'].value!.trim(),
      RegistryDate: new Date(),
      CauseOfDeath: this.form.controls['causeOfDeath'].value?.trim() ?? '',
      DocumentName: this.form.controls['documentName'].value?.trim() ?? '',
    };

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.deathRecordsService.createDeathRecord(request).subscribe({
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

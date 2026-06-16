import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { DeathRecordsService } from '../../../services/death-records.service';
import { UpdateBirthRecordModel } from '../../../../shared/api-models/requests/deathRecord.model';
import { DeathRecordViewModel } from '../../../../shared/api-models/responses/deathRecord.viewmodel';
import { DeathRecordFormValidators } from '../../../../shared/validators/deathRecord-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-update-death-record-dialog',
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
  templateUrl: './update-death-record-dialog.component.html',
})
export class UpdateDeathRecordDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<UpdateDeathRecordDialogComponent>);
  private readonly deathRecordsService = inject(DeathRecordsService);

  protected form!: FormGroup;
  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: DeathRecordViewModel) {}

  public ngOnInit(): void {
    this.form = this.fb.group({
      registryNumber: [
        this.data.registryNumber,
        DeathRecordFormValidators.registryNumberValidators(),
      ],
      personPesel: [this.data.personPesel, DeathRecordFormValidators.personPeselValidators()],
      deathDate: [this.data.deathDate, DeathRecordFormValidators.deathDateValidators()],
      deathPlace: [this.data.deathPlace, DeathRecordFormValidators.deathPlaceValidators()],
      causeOfDeath: [this.data.causeOfDeath, DeathRecordFormValidators.causeOfDeathValidators()],
      documentName: [this.data.documentName, DeathRecordFormValidators.documentNameValidators()],
    });
  }

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

    const trimmedRegistryNumber = this.form.controls['registryNumber'].value!.trim();
    const trimmedPersonPesel = this.form.controls['personPesel'].value!.trim();
    const trimmedDeathPlace = this.form.controls['deathPlace'].value!.trim();
    const trimmedCauseOfDeath = this.form.controls['causeOfDeath'].value?.trim() ?? '';
    const trimmedDocumentName = this.form.controls['documentName'].value?.trim() ?? '';

    const request: UpdateBirthRecordModel = {};

    if (trimmedRegistryNumber !== this.data.registryNumber.trim()) {
      request.RegistryNumber = trimmedRegistryNumber;
    }

    if (trimmedPersonPesel !== this.data.personPesel.trim()) {
      request.PersonPesel = trimmedPersonPesel;
    }

    const currentDeathDate = this.form.controls['deathDate'].value;
    const originalDeathDate = this.data.deathDate;

    if (
      currentDeathDate &&
      originalDeathDate &&
      new Date(currentDeathDate).getTime() !== new Date(originalDeathDate).getTime()
    ) {
      request.DeathDate = currentDeathDate;
    }

    if (trimmedDeathPlace !== this.data.deathPlace.trim()) {
      request.DeathPlace = trimmedDeathPlace;
    }

    if (trimmedCauseOfDeath !== (this.data.causeOfDeath?.trim() ?? '')) {
      request.CauseOfDeath = trimmedCauseOfDeath;
    }

    if (trimmedDocumentName !== (this.data.documentName?.trim() ?? '')) {
      request.DocumentName = trimmedDocumentName;
    }

    if (!Number.isInteger(this.data.deathRecordId)) {
      this.apiErrorMessage = 'Death record ID is missing. Cannot update this record.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.deathRecordsService.updateDeathRecord(this.data.deathRecordId, request).subscribe({
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

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

import { BirthRecordsService } from '../../../services/birth-records.service';
import { UpdateBirthRecordModel } from '../../../../shared/api-models/requests/birthRecord.model';
import { BirthRecordViewModel } from '../../../../shared/api-models/responses/birthRecord.viewmodel';
import { BirthRecordsFormValidators } from '../../../../shared/validators/birthRecord-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-update-birth-record-dialog',
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
  templateUrl: './update-birth-record-dialog.component.html',
})
export class UpdateBirthRecordDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<UpdateBirthRecordDialogComponent>);
  private readonly birthRecordsService = inject(BirthRecordsService);

  protected form!: FormGroup;
  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: BirthRecordViewModel) {}

  public ngOnInit(): void {
    this.form = this.fb.group({
      registryNumber: [
        this.data.registryNumber,
        BirthRecordsFormValidators.registryNumberValidators(),
      ],
      personPesel: [this.data.bornPersonPesel, BirthRecordsFormValidators.personPeselValidators()],
      motherPesel: [this.data.motherPesel, BirthRecordsFormValidators.relativePeselValidators()],
      fatherPesel: [this.data.fatherPesel, BirthRecordsFormValidators.relativePeselValidators()],
      documentName: [this.data.documentName, BirthRecordsFormValidators.documentNameValidators()],
    });
  }

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

    if (
      !BirthRecordsFormValidators.hasBirthRecordChanges(
        this.form,
        this.data.registryNumber,
        this.data.bornPersonPesel,
        this.data.motherPesel,
        this.data.fatherPesel,
        this.data.documentName,
      )
    ) {
      this.apiErrorMessage =
        'Provided data is the same as current. Please make changes before submitting.';
      this.cdr.detectChanges();
      return;
    }

    const trimmedRegistryNumber = this.form.controls['registryNumber'].value!.trim();
    const trimmedPersonPesel = this.form.controls['personPesel'].value!.trim();
    const trimmedMotherPesel = this.form.controls['motherPesel'].value?.trim() ?? '';
    const trimmedFatherPesel = this.form.controls['fatherPesel'].value?.trim() ?? '';
    const trimmedDocumentName = this.form.controls['documentName'].value?.trim() ?? '';

    const request: UpdateBirthRecordModel = {};

    if (trimmedRegistryNumber !== this.data.registryNumber.trim()) {
      request.registryNumber = trimmedRegistryNumber;
    }

    if (trimmedPersonPesel !== this.data.bornPersonPesel.trim()) {
      request.personPesel = trimmedPersonPesel;
    }

    if (trimmedMotherPesel !== (this.data.motherPesel ?? '').trim()) {
      request.motherPesel = trimmedMotherPesel;
    }

    if (trimmedFatherPesel !== (this.data.fatherPesel ?? '').trim()) {
      request.fatherPesel = trimmedFatherPesel;
    }

    if (trimmedDocumentName !== (this.data.documentName ?? '').trim()) {
      request.documentName = trimmedDocumentName;
    }

    if (!Number.isInteger(this.data.birthRecordId)) {
      this.apiErrorMessage = 'Birth record ID is missing. Cannot update this record.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.birthRecordsService.updateBirthRecord(this.data.birthRecordId, request).subscribe({
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

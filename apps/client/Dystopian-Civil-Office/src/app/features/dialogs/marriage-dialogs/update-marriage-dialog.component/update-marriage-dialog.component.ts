import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { MarriagesService } from '../../../services/marriages.service';
import { UpdateMarriageModel } from '../../../../shared/api-models/requests/marriage.model';
import { MarriageViewModel } from '../../../../shared/api-models/responses/marriage.viewmodel';
import { MarriageFormValidators } from '../../../../shared/validators/marriage-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-update-marriage-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ButtonComponent,
    DialogShellComponent,
  ],
  templateUrl: './update-marriage-dialog.component.html',
})
export class UpdateMarriageDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<UpdateMarriageDialogComponent>);
  private readonly marriagesService = inject(MarriagesService);

  protected form!: FormGroup;
  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: MarriageViewModel) {}

  public ngOnInit(): void {
    this.form = this.fb.group({
      registryNumber: [this.data.registryNumber, MarriageFormValidators.registryNumberValidators()],
      spouse1Pesel: [this.data.spouse1Pesel, MarriageFormValidators.spouse1PeselValidators()],
      spouse2Pesel: [this.data.spouse2Pesel, MarriageFormValidators.spouse2PeselValidators()],
      marriageDate: [this.data.marriageDate, MarriageFormValidators.marriageDateValidators()],
      marriagePlace: [this.data.marriagePlace, MarriageFormValidators.marriagePlaceValidators()],
      documentName: [this.data.documentName, MarriageFormValidators.documentNameValidators()],
    });
  }

  protected get registryNumberError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['registryNumber'],
      'Registry number',
    );
  }

  protected get spouse1PeselError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['spouse1Pesel'],
      'Spouse 1 PESEL',
    );
  }

  protected get spouse2PeselError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['spouse2Pesel'],
      'Spouse 2 PESEL',
    );
  }

  protected get marriageDateError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['marriageDate'],
      'Marriage date',
    );
  }

  protected get marriagePlaceError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['marriagePlace'],
      'Marriage place',
    );
  }

  protected get documentNameError(): string {
    return MarriageFormValidators.getControlErrorMessage(
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
      !MarriageFormValidators.hasMarriageChanges(
        this.form,
        this.data.registryNumber,
        this.data.spouse1Pesel,
        this.data.spouse2Pesel,
        this.data.marriageDate,
        this.data.marriagePlace,
      )
    ) {
      this.apiErrorMessage =
        'Provided data is the same as current. Please make changes before submitting.';
      this.cdr.detectChanges();
      return;
    }

    const trimmedRegistryNumber = this.form.controls['registryNumber'].value!.trim();
    const trimmedSpouse1Pesel = this.form.controls['spouse1Pesel'].value!.trim();
    const trimmedSpouse2Pesel = this.form.controls['spouse2Pesel'].value!.trim();
    const trimmedMarriagePlace = this.form.controls['marriagePlace'].value!.trim();
    const trimmedDocumentName = this.form.controls['documentName'].value?.trim() ?? '';

    const currentMarriageDate = this.form.controls['marriageDate'].value;
    const originalMarriageDate = this.data.marriageDate ? new Date(this.data.marriageDate) : null;

    const request: UpdateMarriageModel = {};

    if (trimmedRegistryNumber !== this.data.registryNumber.trim()) {
      request.registryNumber = trimmedRegistryNumber;
    }

    if (trimmedSpouse1Pesel !== this.data.spouse1Pesel.trim()) {
      request.spouse1Pesel = trimmedSpouse1Pesel;
    }

    if (trimmedSpouse2Pesel !== this.data.spouse2Pesel.trim()) {
      request.spouse2Pesel = trimmedSpouse2Pesel;
    }

    if (
      currentMarriageDate &&
      ((originalMarriageDate?.getTime?.() ?? null) !== currentMarriageDate.getTime())
    ) {
      request.marriageDate = currentMarriageDate;
    }

    if (trimmedMarriagePlace !== this.data.marriagePlace.trim()) {
      request.marriagePlace = trimmedMarriagePlace;
    }

    if (trimmedDocumentName !== (this.data.documentName?.trim() ?? '')) {
      request.documentName = trimmedDocumentName;
    }

    if (!Number.isInteger(this.data.marriageRecordId)) {
      this.apiErrorMessage = 'Marriage record ID is missing. Cannot update this record.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.marriagesService.updateMarriage(this.data.marriageRecordId, request).subscribe({
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
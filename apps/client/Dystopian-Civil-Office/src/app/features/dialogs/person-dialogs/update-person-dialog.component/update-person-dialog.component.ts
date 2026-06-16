import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { PersonsService } from '../../../services/persons.service';
import { UpdatePersonModel } from '../../../../shared/api-models/requests/person.model';
import { PersonViewModel } from '../../../../shared/api-models/responses/person.viewmodel';
import { PersonFormValidators } from '../../../../shared/validators/person-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-update-person-dialog',
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
  templateUrl: './update-person-dialog.component.html',
})
export class UpdatePersonDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<UpdatePersonDialogComponent>);
  private readonly personsService = inject(PersonsService);

  protected form!: FormGroup;
  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: PersonViewModel) {}

  public ngOnInit(): void {
    this.form = this.fb.group({
      pesel: [this.data.personPesel, PersonFormValidators.peselValidators()],
      firstName: [this.data.firstName, PersonFormValidators.firstNameValidators()],
      middleName: [this.data.middleName, PersonFormValidators.middleNameValidators()],
      lastName: [this.data.lastName, PersonFormValidators.lastNameValidators()],
      gender: [this.data.gender, PersonFormValidators.genderValidators()],
      birthDate: [this.data.birthDate, PersonFormValidators.birthDateValidators()],
      birthPlace: [this.data.birthPlace, PersonFormValidators.birthPlaceValidators()],
      addressRegistryNumber: [
        this.data.addressRegistryNumber,
        PersonFormValidators.addressRegistryNumberValidators(),
      ],
      documentName: [this.data.documentName, PersonFormValidators.documentNameValidators()],
    });
  }

  protected get peselError(): string {
    return PersonFormValidators.getControlErrorMessage(this.form.controls['pesel'], 'PESEL');
  }

  protected get firstNameError(): string {
    return PersonFormValidators.getControlErrorMessage(
      this.form.controls['firstName'],
      'First name',
    );
  }

  protected get middleNameError(): string {
    return PersonFormValidators.getControlErrorMessage(
      this.form.controls['middleName'],
      'Middle name',
    );
  }

  protected get lastNameError(): string {
    return PersonFormValidators.getControlErrorMessage(this.form.controls['lastName'], 'Last name');
  }

  protected get genderError(): string {
    return PersonFormValidators.getControlErrorMessage(this.form.controls['gender'], 'Gender');
  }

  protected get birthDateError(): string {
    return PersonFormValidators.getControlErrorMessage(
      this.form.controls['birthDate'],
      'Birth date',
    );
  }

  protected get birthPlaceError(): string {
    return PersonFormValidators.getControlErrorMessage(
      this.form.controls['birthPlace'],
      'Birth place',
    );
  }

  protected get addressRegistryNumberError(): string {
    return PersonFormValidators.getControlErrorMessage(
      this.form.controls['addressRegistryNumber'],
      'Address registry number',
    );
  }

  protected get documentNameError(): string {
    return PersonFormValidators.getControlErrorMessage(
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
      !PersonFormValidators.hasPersonChanges(
        this.form,
        this.data.personPesel,
        this.data.firstName,
        this.data.middleName,
        this.data.lastName,
        this.data.gender,
        this.data.birthDate,
        this.data.birthPlace,
        this.data.addressRegistryNumber,
        this.data.documentName,
      )
    ) {
      this.apiErrorMessage =
        'Provided data is the same as current. Please make changes before submitting.';
      this.cdr.detectChanges();
      return;
    }

    const trimmedPesel = this.form.controls['pesel'].value!.trim();
    const trimmedFirstName = this.form.controls['firstName'].value!.trim();
    const trimmedMiddleName = this.form.controls['middleName'].value?.trim() ?? '';
    const trimmedLastName = this.form.controls['lastName'].value!.trim();
    const trimmedGender = this.form.controls['gender'].value!.trim();
    const trimmedBirthPlace = this.form.controls['birthPlace'].value!.trim();
    const trimmedAddressRegistryNumber =
      this.form.controls['addressRegistryNumber'].value?.trim() ?? '';
    const trimmedDocumentName = this.form.controls['documentName'].value?.trim() ?? '';

    const request: UpdatePersonModel = {};

    if (trimmedPesel !== this.data.personPesel.trim()) {
      request.pesel = trimmedPesel;
    }

    if (trimmedFirstName !== this.data.firstName.trim()) {
      request.firstName = trimmedFirstName;
    }

    if (trimmedMiddleName !== (this.data.middleName ?? '').trim()) {
      request.middleName = trimmedMiddleName;
    }

    if (trimmedLastName !== this.data.lastName.trim()) {
      request.lastName = trimmedLastName;
    }

    if (trimmedGender !== this.data.gender.trim()) {
      request.gender = trimmedGender;
    }

    if (this.form.controls['birthDate'].value !== this.data.birthDate) {
      request.birthDate = this.form.controls['birthDate'].value;
    }

    if (trimmedBirthPlace !== this.data.birthPlace.trim()) {
      request.birthPlace = trimmedBirthPlace;
    }

    if (trimmedAddressRegistryNumber !== this.data.addressRegistryNumber.trim()) {
      request.addressRegistryNumber = trimmedAddressRegistryNumber;
    }

    if (trimmedDocumentName !== this.data.documentName.trim()) {
      request.documentName = trimmedDocumentName;
    }

    if (!Number.isInteger(this.data.personId)) {
      this.apiErrorMessage = 'Person ID is missing. Cannot update this record.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.personsService.updatePerson(this.data.personId, request).subscribe({
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

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { PersonsService } from '../../../services/persons.service';
import { CreatePersonModel } from '../../../../shared/api-models/requests/person.model';
import { PersonFormValidators } from '../../../../shared/validators/person-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-create-person-dialog',
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
  templateUrl: './create-person-dialog.component.html',
})
export class CreatePersonDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<CreatePersonDialogComponent>);
  private readonly personsService = inject(PersonsService);

  protected readonly form = this.fb.group({
    pesel: ['', PersonFormValidators.peselValidators()],
    firstName: ['', PersonFormValidators.firstNameValidators()],
    middleName: ['', PersonFormValidators.middleNameValidators()],
    lastName: ['', PersonFormValidators.lastNameValidators()],
    gender: ['', PersonFormValidators.genderValidators()],
    birthDate: [null, PersonFormValidators.birthDateValidators()],
    birthPlace: ['', PersonFormValidators.birthPlaceValidators()],
    addressRegistryNumber: ['', PersonFormValidators.addressRegistryNumberValidators()],
    documentName: ['', PersonFormValidators.documentNameValidators()],
  });

  protected isSubmitting = false;
  protected apiErrorMessage = '';

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

    const birthDate = this.form.controls['birthDate'].value;

    if (!birthDate) {
      this.apiErrorMessage = 'Birth date is required.';
      this.cdr.detectChanges();
      return;
    }

    const request: CreatePersonModel = {
      pesel: this.form.controls['pesel'].value!.trim(),
      firstName: this.form.controls['firstName'].value!.trim(),
      middleName: this.form.controls['middleName'].value?.trim() ?? '',
      lastName: this.form.controls['lastName'].value!.trim(),
      gender: this.form.controls['gender'].value!.trim(),
      birthDate,
      birthPlace: this.form.controls['birthPlace'].value!.trim(),
      addressRegistryNumber: this.form.controls['addressRegistryNumber'].value?.trim() ?? '',
      documentName: this.form.controls['documentName'].value?.trim() ?? '',
    };

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.personsService.createPerson(request).subscribe({
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

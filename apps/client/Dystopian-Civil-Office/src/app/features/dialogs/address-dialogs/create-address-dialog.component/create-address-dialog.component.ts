import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

import { AddressesService } from '../../../services/addresses.service';
import { CreateAddressModel } from '../../../../shared/api-models/requests/address.model';
import { AddressFormValidators } from '../../../../shared/validators/address-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-create-address-dialog.component',
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
  templateUrl: './create-address-dialog.component.html',
  styles: ``,
})
export class CreateAddressDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<CreateAddressDialogComponent>);
  private readonly addressesService = inject(AddressesService);

  protected readonly form = this.fb.group({
    registryNumber: ['', AddressFormValidators.registryNumberValidators?.() ?? []],
    documentName: ['', AddressFormValidators.documentNameValidators?.() ?? []],
    city: ['', AddressFormValidators.cityValidators()],
    street: ['', AddressFormValidators.streetValidators()],
    houseNumber: ['', AddressFormValidators.houseNumberValidators()],
    apartmentNumber: ['', AddressFormValidators.apartmentNumberValidators()],
    postalCode: ['', AddressFormValidators.postalCodeValidators()],
    country: ['', AddressFormValidators.countryValidators()],
  });

  protected isSubmitting = false;
  protected apiErrorMessage = '';

  protected get registryNumberError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls.registryNumber,
      'Registry number',
    );
  }

  protected get documentNameError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls.documentName,
      'Document name',
    );
  }

  protected get cityError(): string {
    return GlobalFormValidators.getControlErrorMessage(this.form.controls.city, 'City');
  }

  protected get streetError(): string {
    return GlobalFormValidators.getControlErrorMessage(this.form.controls.street, 'Street');
  }

  protected get houseNumberError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls.houseNumber,
      'House number',
    );
  }

  protected get apartmentNumberError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls.apartmentNumber,
      'Apartment number',
    );
  }

  protected get postalCodeError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls.postalCode,
      'Postal code',
    );
  }

  protected get countryError(): string {
    return GlobalFormValidators.getControlErrorMessage(this.form.controls.country, 'Country');
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

    const request: CreateAddressModel = {
      registryNumber: (this.form.controls.registryNumber.value ?? '').trim(),
      documentName: (this.form.controls.documentName.value ?? '').trim(),
      city: (this.form.controls.city.value ?? '').trim(),
      street: (this.form.controls.street.value ?? '').trim(),
      houseNumber: (this.form.controls.houseNumber.value ?? '').trim(),
      apartmentNumber: (this.form.controls.apartmentNumber.value ?? '').trim(),
      postalCode: (this.form.controls.postalCode.value ?? '').trim(),
      country: (this.form.controls.country.value ?? '').trim(),
    };

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.addressesService.createAddress(request).subscribe({
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

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

import { AddressesService } from '../../../services/addresses.service';
import { UpdateAddressModel } from '../../../../shared/api-models/requests/address.model';
import { AddressViewModel } from '../../../../shared/api-models/responses/address.viewmodel';
import { AddressFormValidators } from '../../../../shared/validators/address-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-update-address-dialog.component',
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
  templateUrl: './update-address-dialog.component.html',
  styles: ``,
})
export class UpdateAddressDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<UpdateAddressDialogComponent>);
  private readonly addressesService = inject(AddressesService);

  protected form!: FormGroup;
  protected isSubmitting = false;
  protected apiErrorMessage = '';

  constructor(@Inject(MAT_DIALOG_DATA) protected readonly data: AddressViewModel) {}

  public ngOnInit(): void {
    this.form = this.fb.group({
      city: [this.data.city, AddressFormValidators.cityValidators()],
      street: [this.data.street, AddressFormValidators.streetValidators()],
      houseNumber: [this.data.houseNumber, AddressFormValidators.houseNumberValidators()],
      apartmentNumber: [
        this.data.apartmentNumber,
        AddressFormValidators.apartmentNumberValidators(),
      ],
      postalCode: [this.data.postalCode, AddressFormValidators.postalCodeValidators()],
      country: [this.data.country, AddressFormValidators.countryValidators()],
    });
  }

  protected get registryNumberError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls['registryNumber'],
      'registryNumber',
    );
  }

  protected get cityError(): string {
    return GlobalFormValidators.getControlErrorMessage(this.form.controls['city'], 'City');
  }

  protected get streetError(): string {
    return GlobalFormValidators.getControlErrorMessage(this.form.controls['street'], 'Street');
  }

  protected get houseNumberError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls['houseNumber'],
      'House number',
    );
  }

  protected get apartmentNumberError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls['apartmentNumber'],
      'Apartment number',
    );
  }

  protected get postalCodeError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls['postalCode'],
      'Postal code',
    );
  }

  protected get countryError(): string {
    return GlobalFormValidators.getControlErrorMessage(this.form.controls['country'], 'Country');
  }

  protected get documentNameError(): string {
    return GlobalFormValidators.getControlErrorMessage(
      this.form.controls['documentName'],
      'documentName',
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
      !AddressFormValidators.hasAddressChanges(
        this.form,
        this.data.registryNumber,
        this.data.city,
        this.data.street,
        this.data.houseNumber,
        this.data.apartmentNumber,
        this.data.postalCode,
        this.data.country,
        this.data.documentName,
      )
    ) {
      this.apiErrorMessage =
        'Provided data is the same as current. Please make changes before submitting.';
      this.cdr.detectChanges();
      return;
    }

    const trimmedRegistryNumber = this.form.controls['registryNumber'].value!.trim();
    const trimmedCity = this.form.controls['city'].value!.trim();
    const trimmedStreet = this.form.controls['street'].value!.trim();
    const trimmedHouseNumber = this.form.controls['houseNumber'].value!.trim();
    const trimmedApartmentNumber = this.form.controls['apartmentNumber'].value!.trim();
    const trimmedPostalCode = this.form.controls['postalCode'].value!.trim();
    const trimmedCountry = this.form.controls['country'].value!.trim();
    const trimmedDocumentName = this.form.controls['documentName'].value!.trim();

    const request: UpdateAddressModel = {};

    if (trimmedRegistryNumber !== this.data.registryNumber.trim()) {
      request.registryNumber = trimmedRegistryNumber;
    }

    if (trimmedCity !== this.data.city.trim()) {
      request.city = trimmedCity;
    }

    if (trimmedStreet !== this.data.street.trim()) {
      request.street = trimmedStreet;
    }

    if (trimmedHouseNumber !== this.data.houseNumber.trim()) {
      request.houseNumber = trimmedHouseNumber;
    }

    if (trimmedApartmentNumber !== this.data.apartmentNumber.trim()) {
      request.apartmentNumber = trimmedApartmentNumber;
    }

    if (trimmedPostalCode !== this.data.postalCode.trim()) {
      request.postalCode = trimmedPostalCode;
    }

    if (trimmedCountry !== this.data.country.trim()) {
      request.country = trimmedCountry;
    }

    if (trimmedDocumentName !== this.data.documentName.trim()) {
      request.documentName = trimmedDocumentName;
    }

    if (this.data.addressId == null) {
      this.apiErrorMessage = 'Address ID is missing. Cannot update this record.';
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.addressesService.updateAddress(this.data.addressId, request).subscribe({
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

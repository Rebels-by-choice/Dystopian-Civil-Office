import { FormGroup, ValidatorFn, Validators } from '@angular/forms';

import { GlobalFormValidators } from './global-form.validators';

export class AddressesFormValidators {
  public static cityValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(100),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static streetValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(100),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static houseNumberValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(20),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static apartmentNumberValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(20),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static postalCodeValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(15),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static countryValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(60),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static getControlErrorMessage = GlobalFormValidators.getControlErrorMessage;
  public static getApiErrorMessage = GlobalFormValidators.getApiErrorMessage;

  public static hasAddressesChanges(
    form: FormGroup,
    originalCity: string,
    originalStreet: string,
    originalHouseNumber: string,
    originalApartmentNumber: string,
    originalPostalCode: string,
    originalCountry: string,
  ): boolean {
    const currentCity = form.get('city')?.value?.trim() ?? '';
    const currentStreet = form.get('street')?.value?.trim() ?? '';
    const currentHouseNumber = form.get('houseNumber')?.value?.trim() ?? '';
    const currentApartmentNumber = form.get('apartmentNumber')?.value?.trim() ?? '';
    const currentPostalCode = form.get('postalCode')?.value?.trim() ?? '';
    const currentCountry = form.get('country')?.value?.trim() ?? '';

    return (
      currentCity !== originalCity.trim() ||
      currentStreet !== originalStreet.trim() ||
      currentHouseNumber !== originalHouseNumber.trim() ||
      currentApartmentNumber !== originalApartmentNumber.trim() ||
      currentPostalCode !== originalPostalCode.trim() ||
      currentCountry !== originalCountry.trim()
    );
  }
}

import {
  FormGroup,
  ValidatorFn,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';

import { GlobalFormValidators } from './global-form.validators';

export class PersonFormValidators {
  public static peselValidators(): ValidatorFn[] {
    return [Validators.required, Validators.minLength(11), Validators.maxLength(11)];
  }

  public static firstNameValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(100),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static middleNameValidators(): ValidatorFn[] {
    return [Validators.maxLength(100)];
  }

  public static lastNameValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(100),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static genderValidators(): ValidatorFn[] {
    return [Validators.required, PersonFormValidators.allowedGender()];
  }

  private static allowedGender(): ValidatorFn {
    const allowed = ['Male', 'Female', 'Other'];
    return (control: AbstractControl): ValidationErrors | null => {
      const v = control.value as string | null | undefined;
      if (v == null || v === '') {
        return null;
      }
      return allowed.includes(v.trim()) ? null : { invalidGender: true };
    };
  }

  public static birthDateValidators(): ValidatorFn[] {
    return [Validators.required];
  }

  public static birthPlaceValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(100),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static addressRegistryNumberValidators(): ValidatorFn[] {
    return [Validators.maxLength(50)];
  }

  public static documentNameValidators(): ValidatorFn[] {
    return [Validators.maxLength(100)];
  }

  public static getControlErrorMessage = GlobalFormValidators.getControlErrorMessage;
  public static getApiErrorMessage = GlobalFormValidators.getApiErrorMessage;

  public static hasPersonChanges(
    form: FormGroup,
    originalPesel: string,
    originalFirstName: string,
    originalMiddleName: string,
    originalLastName: string,
    originalGender: string,
    originalBirthDate: Date | string,
    originalBirthPlace: string,
    originalAddressRegistryNumber: string,
  ): boolean {
    const currentPesel = form.get('pesel')?.value?.trim() ?? '';
    const currentFirstName = form.get('firstName')?.value?.trim() ?? '';
    const currentMiddleName = form.get('middleName')?.value?.trim() ?? '';
    const currentLastName = form.get('lastName')?.value?.trim() ?? '';
    const currentGender = form.get('gender')?.value?.trim() ?? '';
    const currentBirthDate = form.get('birthDate')?.value ?? '';
    const currentBirthPlace = form.get('birthPlace')?.value?.trim() ?? '';
    const currentAddressRegistryNumber = form.get('addressRegistryNumber')?.value?.trim() ?? '';

    return (
      currentPesel !== (originalPesel?.toString() ?? '') ||
      currentFirstName !== (originalFirstName ?? '') ||
      currentMiddleName !== (originalMiddleName ?? '') ||
      currentLastName !== (originalLastName ?? '') ||
      currentGender !== (originalGender ?? '') ||
      (currentBirthDate?.toString() ?? '') !== (originalBirthDate?.toString() ?? '') ||
      currentBirthPlace !== (originalBirthPlace ?? '') ||
      currentAddressRegistryNumber !== (originalAddressRegistryNumber ?? '')
    );
  }
}

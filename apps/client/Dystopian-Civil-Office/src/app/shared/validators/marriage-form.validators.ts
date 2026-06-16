import {
  FormGroup,
  ValidatorFn,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';

import { GlobalFormValidators } from './global-form.validators';

export class MarriageFormValidators {
  public static registryNumberValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(50),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static spouse1PeselValidators(): ValidatorFn[] {
    return [Validators.required, Validators.minLength(11), Validators.maxLength(11)];
  }

  public static spouse2PeselValidators(): ValidatorFn[] {
    return [Validators.required, Validators.minLength(11), Validators.maxLength(11)];
  }

  public static marriageDateValidators(): ValidatorFn[] {
    return [Validators.required];
  }

  public static marriagePlaceValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(100),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static documentNameValidators(): ValidatorFn[] {
    return [Validators.minLength(2), Validators.maxLength(100)];
  }

  public static spousesDifferent(): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const spouse1 = group.get('spouse1Pesel')?.value?.toString()?.trim() ?? '';
      const spouse2 = group.get('spouse2Pesel')?.value?.toString()?.trim() ?? '';

      if (!spouse1 || !spouse2) {
        return null;
      }

      return spouse1 === spouse2 ? { spousesSame: true } : null;
    };
  }

  public static getControlErrorMessage = GlobalFormValidators.getControlErrorMessage;
  public static getApiErrorMessage = GlobalFormValidators.getApiErrorMessage;

  public static hasMarriageChanges(
    form: FormGroup,
    originalRegistryNumber: string,
    originalSpouse1Pesel: string,
    originalSpouse2Pesel: string,
    originalMarriageDate: Date | string,
    originalMarriagePlace: string,
  ): boolean {
    const currentRegistryNumber = form.get('registryNumber')?.value?.trim() ?? '';
    const currentSpouse1 = form.get('spouse1Pesel')?.value?.trim() ?? '';
    const currentSpouse2 = form.get('spouse2Pesel')?.value?.trim() ?? '';
    const currentMarriageDate = form.get('marriageDate')?.value ?? null;
    const currentMarriagePlace = form.get('marriagePlace')?.value?.trim() ?? '';

    const originalMarriageDateValue = originalMarriageDate ? new Date(originalMarriageDate) : null;

    const marriageDateChanged =
      (currentMarriageDate?.getTime?.() ?? null) !==
      (originalMarriageDateValue?.getTime?.() ?? null);

    return (
      currentRegistryNumber !== (originalRegistryNumber ?? '') ||
      currentSpouse1 !== (originalSpouse1Pesel ?? '') ||
      currentSpouse2 !== (originalSpouse2Pesel ?? '') ||
      marriageDateChanged ||
      currentMarriagePlace !== (originalMarriagePlace ?? '')
    );
  }
}

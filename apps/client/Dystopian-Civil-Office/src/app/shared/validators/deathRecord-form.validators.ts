import { FormGroup, ValidatorFn, Validators } from '@angular/forms';

import { GlobalFormValidators } from './global-form.validators';

export class DeathRecordFormValidators {
  public static registryNumberValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(50),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static personPeselValidators(): ValidatorFn[] {
    return [Validators.required, Validators.minLength(11), Validators.maxLength(11)];
  }

  public static deathDateValidators(): ValidatorFn[] {
    return [Validators.required];
  }

  public static deathPlaceValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(100),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static registryDateValidators(): ValidatorFn[] {
    return [Validators.required];
  }

  public static causeOfDeathValidators(): ValidatorFn[] {
    return [Validators.required, Validators.maxLength(200), GlobalFormValidators.notBlank()];
  }

  public static documentNameValidators(): ValidatorFn[] {
    return [Validators.minLength(2), Validators.maxLength(100)];
  }

  public static getControlErrorMessage = GlobalFormValidators.getControlErrorMessage;
  public static getApiErrorMessage = GlobalFormValidators.getApiErrorMessage;

  public static hasDeathRecordChanges(
    form: FormGroup,
    originalRegistryNumber: string,
    originalPersonPesel: string,
    originalDeathDate: Date | string,
    originalDeathPlace: string,
    originalRegistryDate: Date | string,
    originalCauseOfDeath: string,
  ): boolean {
    const currentRegistryNumber = form.get('registryNumber')?.value?.trim() ?? '';
    const currentPersonPesel = form.get('personPesel')?.value?.trim() ?? '';
    const currentDeathDate = form.get('deathDate')?.value ?? '';
    const currentDeathPlace = form.get('deathPlace')?.value?.trim() ?? '';
    const currentRegistryDate = form.get('registryDate')?.value ?? '';
    const currentCauseOfDeath = form.get('causeOfDeath')?.value?.trim() ?? '';

    return (
      currentRegistryNumber !== (originalRegistryNumber ?? '') ||
      currentPersonPesel !== (originalPersonPesel ?? '') ||
      (currentDeathDate?.toString() ?? '') !== (originalDeathDate?.toString() ?? '') ||
      currentDeathPlace !== (originalDeathPlace ?? '') ||
      (currentRegistryDate?.toString() ?? '') !== (originalRegistryDate?.toString() ?? '') ||
      currentCauseOfDeath !== (originalCauseOfDeath ?? '')
    );
  }
}

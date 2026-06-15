import {
  FormGroup,
  ValidatorFn,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';

import { GlobalFormValidators } from './global-form.validators';

export class BirthRecordsFormValidators {
  public static registryNumberValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(50),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static personPeselValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(11),
      Validators.maxLength(11),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static relativePeselValidators(): ValidatorFn[] {
    return [Validators.minLength(11), Validators.maxLength(11)];
  }

  public static documentNameValidators(): ValidatorFn[] {
    return [Validators.maxLength(100)];
  }

  public static getControlErrorMessage = GlobalFormValidators.getControlErrorMessage;
  public static getApiErrorMessage = GlobalFormValidators.getApiErrorMessage;

  public static hasBirthRecordChanges(
    form: FormGroup,
    originalRegistryNumber: string,
    originalPersonPesel: string,
    originalMotherPesel: string,
    originalFatherPesel: string,
    originalDocumentName: string,
  ): boolean {
    const currentRegistryNumber = form.get('registryNumber')?.value?.trim() ?? '';
    const currentPersonPesel = form.get('personPesel')?.value?.trim() ?? '';
    const currentMotherPesel = form.get('motherPesel')?.value?.trim() ?? '';
    const currentFatherPesel = form.get('fatherPesel')?.value?.trim() ?? '';
    const currentDocumentName = form.get('documentName')?.value?.trim() ?? '';

    return (
      currentRegistryNumber !== (originalRegistryNumber ?? '') ||
      currentPersonPesel !== (originalPersonPesel ?? '') ||
      currentMotherPesel !== (originalMotherPesel ?? '') ||
      currentFatherPesel !== (originalFatherPesel ?? '') ||
      currentDocumentName !== (originalDocumentName ?? '')
    );
  }
}

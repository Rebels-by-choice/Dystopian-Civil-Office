import { FormGroup, ValidatorFn, Validators } from '@angular/forms';

import { GlobalFormValidators } from './global-form.validators';

export class DocumentFormValidators {
  public static nameValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(100),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static categoryValidators(): ValidatorFn[] {
    return [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(100),
      GlobalFormValidators.notBlank(),
    ];
  }

  public static getControlErrorMessage = GlobalFormValidators.getControlErrorMessage;
  public static getApiErrorMessage = GlobalFormValidators.getApiErrorMessage;

  public static hasDocumentChanges(
    form: FormGroup,
    originalName: string,
    originalCategory: string,
  ): boolean {
    const currentName = form.get('name')?.value?.trim() ?? '';
    const currentCategory = form.get('category')?.value?.trim() ?? '';

    return currentName !== originalName.trim() || currentCategory !== originalCategory.trim();
  }
}

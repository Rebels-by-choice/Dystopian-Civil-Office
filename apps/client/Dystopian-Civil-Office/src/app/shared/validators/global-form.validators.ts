import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

export class GlobalFormValidators {
  public static notBlank(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value as string | null | undefined;

      if (value == null) {
        return null;
      }

      return value.trim().length === 0 ? { blank: true } : null;
    };
  }

  public static getControlErrorMessage(control: AbstractControl | null, label: string): string {
    if (!control?.errors || !(control.touched || control.dirty)) {
      return '';
    }

    if (control.errors['required']) {
      return `${label} is required.`;
    }

    if (control.errors['blank']) {
      return `${label} cannot contain only spaces.`;
    }

    if (control.errors['minlength']) {
      return `${label} must contain at least ${control.errors['minlength'].requiredLength} characters.`;
    }

    if (control.errors['maxlength']) {
      return `${label} cannot exceed ${control.errors['maxlength'].requiredLength} characters.`;
    }

    return `${label} is invalid.`;
  }

  public static getApiErrorMessage(error: unknown): string {
    if (!(error instanceof HttpErrorResponse)) {
      return 'Unexpected error occurred.';
    }

    const status = error.status;
    const detail = error.error?.detail;
    const title = error.error?.title;

    if (detail) {
      return `${detail}`;
    }

    if (title) {
      return `HTTP Error ${status} - ${title}`;
    }

    return 'Temporary issue with the server';
  }
}

import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { provideNativeDateAdapter } from '@angular/material/core';

import { MarriagesService } from '../../../services/marriages.service';
import { CreateMarriageModel } from '../../../../shared/api-models/requests/marriage.model';
import { MarriageFormValidators } from '../../../../shared/validators/marriage-form.validators';
import { ButtonComponent } from '../../../../shared/ui/button.component/button.component';
import { DialogShellComponent } from '../../../../shared/ui/dialog-shell/dialog-shell.component';
import { GlobalFormValidators } from '../../../../shared/validators/global-form.validators';

@Component({
  selector: 'app-create-marriage-dialog',
  standalone: true,
  templateUrl: './create-marriage-dialog.component.html',
  providers: [provideNativeDateAdapter()],
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
})
export class CreateMarriageDialogComponent {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly dialogRef = inject(MatDialogRef<CreateMarriageDialogComponent>);
  private readonly marriagesService = inject(MarriagesService);

  protected readonly form = this.fb.group({
    registryNumber: ['', MarriageFormValidators.registryNumberValidators()],
    spouse1Pesel: ['', MarriageFormValidators.spouse1PeselValidators()],
    spouse2Pesel: ['', MarriageFormValidators.spouse2PeselValidators()],
    marriageDate: [null, MarriageFormValidators.marriageDateValidators()],
    marriagePlace: ['', MarriageFormValidators.marriagePlaceValidators()],
    documentName: ['', MarriageFormValidators.documentNameValidators()],
  });

  protected isSubmitting = false;
  protected apiErrorMessage = '';

  protected get registryNumberError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['registryNumber'],
      'Registry number',
    );
  }

  protected get spouse1PeselError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['spouse1Pesel'],
      'Spouse 1 PESEL',
    );
  }

  protected get spouse2PeselError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['spouse2Pesel'],
      'Spouse 2 PESEL',
    );
  }

  protected get marriageDateError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['marriageDate'],
      'Marriage date',
    );
  }

  protected get marriagePlaceError(): string {
    return MarriageFormValidators.getControlErrorMessage(
      this.form.controls['marriagePlace'],
      'Marriage place',
    );
  }

  protected get documentNameError(): string {
    return MarriageFormValidators.getControlErrorMessage(
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

    const marriageDate = this.form.controls['marriageDate'].value;

    if (!marriageDate) {
      this.apiErrorMessage = 'Marriage date is required.';
      this.cdr.detectChanges();
      return;
    }

    const request: CreateMarriageModel = {
      registryNumber: this.form.controls['registryNumber'].value!.trim(),
      spouse1Pesel: this.form.controls['spouse1Pesel'].value!.trim(),
      spouse2Pesel: this.form.controls['spouse2Pesel'].value!.trim(),
      marriageDate,
      marriagePlace: this.form.controls['marriagePlace'].value!.trim(),
      registryDate: new Date(),
      documentName: this.form.controls['documentName'].value?.trim() ?? '',
    };

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.marriagesService.createMarriage(request).subscribe({
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

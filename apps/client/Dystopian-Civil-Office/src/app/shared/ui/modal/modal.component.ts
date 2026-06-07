import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Generic modal wrapper component for reusable dialog containers.
 * Handles backdrop, close buttons, and consistent styling.
 */
@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: 'modal.component.html',
})
export class ModalComponent {
  @Input() showCloseButton = true;
  @Input() closeOnBackdropClick = false;
  @Output() close = new EventEmitter<void>();

  protected onClose(): void {
    this.close.emit();
  }

  protected onBackdropClick(): void {
    if (this.closeOnBackdropClick) {
      this.onClose();
    }
  }
}

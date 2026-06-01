import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './button.component.html',
})
export class ButtonComponent {
  @Input() label = '';
  @Input() variant: 'primary' | 'secondary' = 'primary';
  @Input() disabled = false;

  @Output() buttonClick = new EventEmitter<void>();

  protected onClick(): void {
    if (this.disabled) {
      return;
    }

    this.buttonClick.emit();
  }
}

import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  standalone: true,
  templateUrl: './button.component.html',
})
export class ButtonComponent {
  @Output() clicked = new EventEmitter<void>();
}

import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dialog-shell',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dialog-shell.component.html',
})
export class DialogShellComponent {
  @Input() title = '';
  @Input() subtitle = '';
  @Input() errorMessage = '';
  @Input() containerClass =
    'w-[460px] max-w-[92vw] rounded-2xl bg-zinc-900 p-6 text-zinc-100 shadow-2xl';
  @Output() close = new EventEmitter<void>();
}

import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonViewModel } from '../../api-models/responses/person.viewmodel';

@Component({
  selector: 'app-citizen-select',
  imports: [CommonModule],
  templateUrl: './citizen-select.component.html',
})
export class CitizenSelectComponent {
  @Input() users: PersonViewModel[] = [];
  @Input() placeholder: string = 'Select the citizen to login as';

  @Output() selectionChanged = new EventEmitter<string>();

  onSelectionChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selected = selectElement.value;

    this.selectionChanged.emit(selected);
  }

  protected readonly JSON = JSON;
}

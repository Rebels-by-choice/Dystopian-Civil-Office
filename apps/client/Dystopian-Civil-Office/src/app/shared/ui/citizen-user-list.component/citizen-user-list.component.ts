import { Component, inject } from '@angular/core';
import { PersonsService } from '../../../features/services/persons.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { CitizenSelectComponent } from '../citizen-select.component/citizen-select.component';
import { LoginService } from '../../../core/login.service';

@Component({
  selector: 'app-citizen-user-list',
  standalone: true,
  imports: [CitizenSelectComponent],
  template: `
    <div class="flex flex-col items-center">
      <h2 class="text-4xl p-10">"Log in" as:</h2>
      <app-citizen-select
        class="text-xl"
        [users]="users()"
        placeholder="Select a citizen to log in as..."
        (selectionChanged)="handleSelection($event)"
      >
      </app-citizen-select>
    </div>
  `,
})
export class CitizenUserListComponent {
  private personService = inject(PersonsService);
  private loginService = inject(LoginService);

  public users = toSignal(this.personService.getPersons(), { initialValue: [] });

  handleSelection(selectedPerson: string) {
    const [citizenId, isFunctionary] = selectedPerson.split(':');
    console.log(citizenId, isFunctionary);

    this.loginService.setCurrentUser({
      citizenId: parseInt(citizenId),
      isFunctionary: isFunctionary == 'true',
    });
  }
}


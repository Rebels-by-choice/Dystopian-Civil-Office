import { Component, inject, OnInit } from '@angular/core';
import { CasesService } from '../../services/cases.service';
import { AsyncPipe, NgClass } from '@angular/common';
import { LoginService } from '../../../core/login.service';
import { CaseStatus, CaseViewModel } from '../../../shared/api-models/responses/case.viewmodel';
import { combineLatest, Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { PersonsService } from '../../services/persons.service';
import { RouterLink, RouterModule } from '@angular/router';

@Component({
  imports: [AsyncPipe, NgClass, RouterLink, RouterModule],
  templateUrl: './cases-page.component.html',
})
export class CasesPageComponent implements OnInit {
  private casesService = inject(CasesService);
  private personsService = inject(PersonsService);
  private loginService = inject(LoginService);

  cases$!: Observable<any[]>;

  ngOnInit() {
    this.cases$ = this.loginService.getCurrentUser().pipe(
      switchMap((session) => {
        const userCases$ = this.casesService
          .refreshCases()
          .pipe(
            map((cases: any[]) =>
              cases.filter(
                (c) => c.initiatorId === session.citizenId || c.responderId === session.citizenId,
              ),
            ),
          );
        const persons$ = this.personsService.getPersons();

        return combineLatest([userCases$, persons$]).pipe(
          map(([cases, persons]) => {
            return cases.map((c) => {
              const initiator = persons.find((p) => p.personId === c.initiatorId);
              const responder = persons.find((p) => p.personId === c.responderId);

              return {
                ...c,
                statusName: CaseStatus[c.status],
                initiatorName: initiator
                  ? `${initiator.firstName} ${initiator.lastName}`
                  : 'Unknown',
                responderName: responder
                  ? `${responder.firstName} ${responder.lastName}`
                  : 'Unknown',
              };
            });
          }),
        );
      }),
    );
  }

  protected readonly CaseStatus = CaseStatus;
}

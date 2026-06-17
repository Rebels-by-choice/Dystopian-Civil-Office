import { Component, inject, OnInit } from '@angular/core';
import { CasesService } from '../../services/cases.service';
import { PersonsService } from '../../services/persons.service';
import { LoginService } from '../../../core/login.service';
import { combineLatest, first, Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CaseStatus } from '../../../shared/api-models/responses/case.viewmodel';
import { AsyncPipe } from '@angular/common';

@Component({
  templateUrl: './singular-case-page.component.html',
  imports: [AsyncPipe, RouterLink],
})
export class SingularCasePageComponent implements OnInit {
  private casesService = inject(CasesService);
  private personsService = inject(PersonsService);
  private loginService = inject(LoginService);
  private routeParams = inject(ActivatedRoute);

  case$!: Observable<any>;

  ngOnInit() {
    this.case$ = this.loginService.getCurrentUser().pipe(
      switchMap((session) => {
        const caseId = Number(this.routeParams.snapshot.params['caseId']);
        const userCase$ = this.casesService.getCaseById(caseId).pipe(
          map((current_case) => {
            if (
              current_case.initiatorId !== session.citizenId &&
              current_case.responderId !== session.citizenId
            ) {
              throw new Error('Unauthorized');
            }
            return current_case;
          }),
        );

        const persons$ = this.personsService.getPersons();
        return combineLatest([userCase$, persons$]).pipe(
          map(([current_case, persons]) => {
            const initiator = persons.find((p) => p.personId === current_case.initiatorId);
            const responder = persons.find((p) => p.personId === current_case.responderId);

            return {
              caseId: current_case.caseId,
              statusName: CaseStatus[current_case.status],
              initiatorName: initiator ? `${initiator.firstName} ${initiator.lastName}` : 'Unknown',
              responderName: responder ? `${responder.firstName} ${responder.lastName}` : 'Unknown',
              createdAt: current_case.createdAt,
              updatedAt: current_case.closedAt,
            };
          }),
        );
      }),
    );
  }
}

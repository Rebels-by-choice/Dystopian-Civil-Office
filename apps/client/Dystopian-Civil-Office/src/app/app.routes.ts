import { Routes } from '@angular/router';
import { HomePageComponent } from './features/pages/home-page.component/home-page.component';
import { PersonsPageComponent } from './features/pages/person-page.component/persons-page.component';
import { BirthsPageComponent } from './features/pages/births-page.component/births-page.component';
import { AddressesPageComponent } from './features/pages/addresses-page.component/addresses-page.component';
import { MarriagesPageComponent } from './features/pages/marriages-page.component/marriages-page.component';
import { DeathsPageComponent } from './features/pages/deaths-page.component/deaths-page.component';
import { DocumentsPageComponent } from './features/pages/documents-page.component/documents-page.component';
import { StatisticsPageComponent } from './features/pages/statistics-page.component/statistics-page.component';
import { CasesPageComponent } from './features/pages/cases-page.component/cases-page.component';
import { SingularCasePageComponent } from './features/pages/singular-case-page.component/singular-case-page.component';

export const routes: Routes = [
  {
    path: 'home',
    component: HomePageComponent,
  },
  {
    path: 'persons',
    component: PersonsPageComponent,
  },
  {
    path: 'births',
    component: BirthsPageComponent,
  },
  {
    path: 'addresses',
    component: AddressesPageComponent,
  },
  {
    path: 'marriages',
    component: MarriagesPageComponent,
  },
  {
    path: 'deaths',
    component: DeathsPageComponent,
  },
  {
    path: 'documents',
    component: DocumentsPageComponent,
  },
  {
    path: 'statistics',
    component: StatisticsPageComponent,
  },
  {
    path: 'cases',
    component: CasesPageComponent,
  },
  {
    path: 'cases/:caseId',
    component: SingularCasePageComponent,
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  // Fallback route for undefined paths (when providing wrong paths in url ex. /homee)
  {
    path: '**',
    redirectTo: 'home',
  },
];

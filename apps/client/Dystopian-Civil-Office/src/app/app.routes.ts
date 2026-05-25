import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'home',
    loadComponent: () =>
      import('./features/pages/home-page.component/home-page.component').then(
        (m) => m.HomePageComponent,
      ),
  },
  {
    path: 'persons',
    loadComponent: () =>
      import('./features/pages/persons-page.component/persons-page.component').then(
        (m) => m.PersonsPageComponent,
      ),
  },
  {
    path: 'births',
    loadComponent: () =>
      import('./features/pages/births-page.component/births-page.component').then(
        (m) => m.BirthsPageComponent,
      ),
  },
  {
    path: 'addresses',
    loadComponent: () =>
      import('./features/pages/addresses-page.component/addresses-page.component').then(
        (m) => m.AddressesPageComponent,
      ),
  },
  {
    path: 'marriages',
    loadComponent: () =>
      import('./features/pages/marriages-page.component/marriages-page.component').then(
        (m) => m.MarriagesPageComponent,
      ),
  },
  {
    path: 'deaths',
    loadComponent: () =>
      import('./features/pages/deaths-page.component/deaths-page.component').then(
        (m) => m.DeathsPageComponent,
      ),
  },
  {
    path: 'documents',
    loadComponent: () =>
      import('./features/pages/documents-page.component/documents-page.component').then(
        (m) => m.DocumentsPageComponent,
      ),
  },
  {
    path: 'statistics',
    loadComponent: () =>
      import('./features/pages/statistics-page.component/statistics-page.component').then(
        (m) => m.StatisticsPageComponent,
      ),
  },
  {
    path: 'cases',
    loadComponent: () =>
      import('./features/pages/cases-page.component/cases-page.component').then(
        (m) => m.CasesPageComponent,
      ),
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

import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

type NavItem = {
  label: string;
  path: string;
};

@Component({
  selector: 'app-top-navbar-component',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './top-navbar.component.html',
})
export class TopNavbarComponent {
  public readonly logoPath = 'assets/logo.svg';
  public readonly homePath = '/home';

  public readonly mainNavItems: NavItem[] = [
    { label: 'Persons', path: '/persons' },
    { label: 'Births', path: '/births' },
    { label: 'Addresses', path: '/addresses' },
    { label: 'Marriages', path: '/marriages' },
    { label: 'Deaths', path: '/deaths' },
    { label: 'Documents', path: '/documents' },
    { label: 'Statistics', path: '/statistics' },
  ];

  public readonly casesPath = '/cases';
}

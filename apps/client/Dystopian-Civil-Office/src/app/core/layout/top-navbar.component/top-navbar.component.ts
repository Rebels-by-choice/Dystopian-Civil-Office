import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Observable } from 'rxjs';
import { SessionStorageModel } from '../../../shared/ui/session-storage.model';
import { LoginService } from '../../login.service';
import { AsyncPipe } from '@angular/common';

type NavItem = {
  label: string;
  path: string;
};

@Component({
  selector: 'app-top-navbar-component',
  imports: [RouterLink, RouterLinkActive, AsyncPipe],
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

  sessionStorage$: Observable<SessionStorageModel>;

  constructor(private loginService: LoginService) {
    this.sessionStorage$ = loginService.getCurrentUser();
  }

  logout() {
    this.loginService.removeCurrentUser();
  }
}

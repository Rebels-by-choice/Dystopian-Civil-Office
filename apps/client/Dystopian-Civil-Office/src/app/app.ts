import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TopNavbarComponent } from './core/layout/top-navbar.component/top-navbar.component';
import { FooterComponent } from './core/layout/footer.component/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, TopNavbarComponent, TopNavbarComponent, FooterComponent],
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('Dystopian-Civil-Service');
}

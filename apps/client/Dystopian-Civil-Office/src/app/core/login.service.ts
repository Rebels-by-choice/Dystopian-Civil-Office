import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { SessionStorageModel } from '../shared/ui/session-storage.model';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private sessionStorage = new BehaviorSubject<SessionStorageModel>({"citizenId": -1, "isFunctionary": false});

  sessionStorage$ = this.sessionStorage.asObservable();
  constructor() { }

  setCurrentUser(user: SessionStorageModel) {
    this.sessionStorage.next(user);
  }

  getCurrentUser(): Observable<SessionStorageModel> {
    return this.sessionStorage$;
  }

  removeCurrentUser() {
    this.sessionStorage.next({"citizenId": -1, "isFunctionary": false});
  }
}

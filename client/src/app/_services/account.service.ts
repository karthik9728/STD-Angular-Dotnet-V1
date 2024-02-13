import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, BehaviorSubject } from 'rxjs';
import { User } from '../_models/user';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl: string = environment.apiUrl;

  private currentUserSource = new BehaviorSubject<User>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {}

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }

        return user;
      })
    );
  }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  //will used to fetch data from local storage to set user
  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }
}

import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  model: any = {};

  loggedIn = false;

  constructor(private accountService: AccountService) {}

  ngOnInit() {
    this.getCurrentUser();
  }

  //getting current user from local storage
  getCurrentUser() {
    this.accountService.currentUser$.subscribe({
      next: (user) => {
        this.loggedIn = !!user;
      },
      error: (error) => console.log(error),
    });
  }

  onLogin(form: NgForm) {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.loggedIn = true;
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log('Logged In Successfully');
      },
    });

    form.reset();
  }

  onLogout() {
    this.accountService.logout();
    this.loggedIn = false;
  }
}

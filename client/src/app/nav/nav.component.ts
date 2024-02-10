import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  model: any = {
    username: '',
    password: '',
  };

  username: string;

  loggedIn = false;

  constructor(private accountService: AccountService) {}

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
    this.loggedIn = false;
  }
}

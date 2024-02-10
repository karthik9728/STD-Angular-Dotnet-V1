import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  model: any = {};
  constructor(public accountService: AccountService) {}

  ngOnInit() {}

  onLogin(form: NgForm) {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
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
  }
}

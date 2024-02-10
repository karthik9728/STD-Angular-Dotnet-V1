import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  model: any = {};
  constructor(public accountService: AccountService, private router: Router) {}

  ngOnInit() {}

  onLogin(form: NgForm) {
    this.accountService.login(this.model).subscribe({
      next: () => this.router.navigateByUrl('/members'),
      error: (error) => console.log(error),
      complete: () => console.log('Logged In Successfully'),
    });

    form.reset();
  }

  onLogout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}

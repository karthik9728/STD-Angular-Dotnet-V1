import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  model: any = {};
  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit() {}

  onLogin(form: NgForm) {
    this.accountService.login(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/members');
        this.toastr.success('Logged In Successfully');
      },
      error: (error) => this.toastr.error(error?.error),
      complete: () => {},
    });

    form.reset();
  }

  onLogout() {
    this.accountService.logout();
    this.toastr.success('Logged Out Successfully');
    this.router.navigateByUrl('/');
  }
}

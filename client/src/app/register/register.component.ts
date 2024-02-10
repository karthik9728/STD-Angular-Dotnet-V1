import { Component, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}

  register() {
    this.accountService.register(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: (error) => {
        if (error?.error?.message) {
          this.toastr.error(error?.error?.message);
        }
        console.log(error);
      },
      complete: () => console.log('Request is completed'),
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}

import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

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

  onSubmit(form: NgForm) {
    this.model = {
      username: form.value.username,
      password: form.value.password,
    };
    console.log(this.model);

    form.reset();
  }
}

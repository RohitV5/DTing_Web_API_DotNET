import { AccountService } from './../_services/account.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent {
  model: { username: string; password: string } = {
    username: '',
    password: '',
  };

  loggedIn: boolean = false;

  constructor(private accountService: AccountService) {}

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.loggedIn = true;
      },
      error: (error) => console.log(error),
    });
  }
}

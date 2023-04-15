import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit{
  model: { username: string; password: string } = {
    username: '',
    password: '',
  };

  loggedIn: boolean = false;

  constructor(private accountService: AccountService) {}

  ngOnInit(){
    this.getCurrentUser()
  }

  getCurrentUser() {
    this.accountService.currentUser$.subscribe({
      next: (user) => {
        this.loggedIn = !!user;
      },
      error: (error) => console.log(error),
    });
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.loggedIn = true;
      },
      error: (error) => console.log(error),
    });
  }

  logout() {
    this.accountService.logout();
    this.loggedIn = false;
  }
}

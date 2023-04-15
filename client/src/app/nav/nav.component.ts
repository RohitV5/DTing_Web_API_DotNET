import { Observable, of } from 'rxjs';
import { User } from '../models/user';
import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: { username: string; password: string } = {
    username: '',
    password: '',
  };

  currentUser$: Observable<User | null> = of(null); //initialize with null;

  constructor(private accountService: AccountService) {}

  ngOnInit() {
    this.currentUser$ = this.accountService.currentUser$;
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {},
      error: (error) => console.log(error),
    });
  }

  logout() {
    this.accountService.logout();
  }
}

import { AccountService } from './_services/account.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'DTing App';
  users: any;

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }

  getUsers(){
    console.log('making api call');
    this.http.get('http://localhost:5186/api/users').subscribe({
      next: (response) => (this.users = response),
      error: (error) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user') || '');
    if (!user) return;

    console.log(user)

    this.accountService.setCurrentUser(user);
  }
}

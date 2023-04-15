import { AccountService } from './_services/account.service';
import { Component, OnInit } from '@angular/core';
import { User } from './models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'DTing App';


  constructor(
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.setCurrentUser();
  }



  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user') || '');
    if (!user) return;

    console.log(user)

    this.accountService.setCurrentUser(user);
  }
}

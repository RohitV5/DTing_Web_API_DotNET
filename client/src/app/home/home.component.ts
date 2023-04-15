import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Event } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  registerMode = false;
  users: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getUsers();
  }

  toggle() {
    console.log('toggling');
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    console.log('making api call');
    this.http.get('http://localhost:5186/api/users').subscribe({
      next: (response) => (this.users = response),
      error: (error) => {
        console.log(error);
      },
      complete: () => {},
    });
  }

  cancelRegister(event: boolean) {
    this.registerMode = event;
  }
}

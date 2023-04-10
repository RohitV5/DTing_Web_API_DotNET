import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'DTing App';
  users: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    console.log("making api call")
    this.http.get('http://localhost:5186/api/users').subscribe({
      next: (response) => (this.users = response),
      error: (error) => {
        console.log(error);
      },
      complete: () => {},
    });
  }
}

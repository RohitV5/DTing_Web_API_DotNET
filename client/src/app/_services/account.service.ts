import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
//Service is a singleton that is its instantiated when application starts and destroyed when applicaiton is closed
//Hence we can save state which we want our application to remember
export class AccountService {
  baseUrl = 'http://localhost:5186/api/';

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model);
  }
}

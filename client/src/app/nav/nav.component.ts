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

  login() {
    console.log(this.model);
  }
}

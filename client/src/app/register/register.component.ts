import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  @Input() usersFromHomeComponent:any;
  model:any = {}

  constructor(){

  }

  ngOnInit(){

  }

  register(){
    console.log(this.model)
  }

  cancel(){

  }



}

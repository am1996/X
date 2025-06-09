import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule,HttpClientModule,CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm: any;
  public error: string ="";
  constructor() {
    this.registerForm = {
      "username":"",
      "email":"",
      "firstname":"",
      "lastname":"",
      "address":"",
      "password":"",
      "confirmPassword":""
    };
  }
  passwordMismatch(){
    return this.registerForm.password !== this.registerForm.confirmPassword;
  }
  submit() {
    // Handle form submission logic here
    console.log('Form submitted:', this.registerForm);
  }

}

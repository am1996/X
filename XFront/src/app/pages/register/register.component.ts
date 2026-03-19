import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  public registerForm: any;
  public router: Router = inject(Router);
  public http = inject(HttpClient);
  public error: string = "";
  constructor() {
    this.registerForm = {
      "username": "",
      "email": "",
      "firstname": "",
      "lastname": "",
      "address": "",
      "password": "",
      "confirmPassword": ""
    };
  }
  passwordMismatch() {
    return this.registerForm.password !== this.registerForm.confirmPassword;
  }
  submit() {
    // Handle form submission logic here
    this.http.post('http://localhost:5118/api/user', this.registerForm).subscribe({
      next: (response: any) => {
        console.log('Registration successful:', response);
        this.registerForm = {
          "username": "",
          "email": "",
          "firstname": "",
          "lastname": "",
          "address": "",
          "password": "",
          "confirmPassword": ""
        };
        this.router.navigate(['/login']);
      },
      error: (error: any) => {
        console.log(error.error);
        if(error.error?.message){
          this.error = error.error.message;
        }else{
          console.log(error.error.errors);
          this.error = error.error.errors[Object.keys(error.error.errors)[0]][0];
          
        }
      }
    })
  }

}

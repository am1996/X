import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule,HttpClientModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  public email: string="";
  public password: string="";
  constructor(private http: HttpClient, private router: Router){}
  public submit(): void{
    this.http.post("http://localhost:5118/api/user/login",{
      email:this.email,
      password:this.password,
    }).subscribe(response => console.log(response));
    this.router.navigate(["/"]);
  }
}

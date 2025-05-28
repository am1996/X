import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject, PLATFORM_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

type StringMap = {[key:string]:string};

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule,HttpClientModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  public platformId = inject(PLATFORM_ID);
  public isBrowser = false;
  public email: string="";
  public password: string="";
  public error: string ="";
  constructor(private http: HttpClient, private router: Router){
    this.isBrowser = isPlatformBrowser(this.platformId);
  }
  public submit(): void{
    this.http.post<StringMap>("http://localhost:5118/api/user/login",{
      email:this.email,
      password:this.password,
    }).subscribe({
      next: (response: StringMap) => {
        if(response["jwtToken"]){
          sessionStorage.setItem("jwt",response["jwtToken"]);
          this.router.navigate(["/"]);
        }
      },
      error: (err: StringMap)=>{
        this.error = err["message"];
      },
    });
  }
}

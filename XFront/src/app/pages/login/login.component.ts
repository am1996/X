import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, PLATFORM_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { SessionStorageService } from '../../Services/SessionStorage/session-storage.service';

type StringMap = {[key:string]:string};

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  public sessionStorageService = inject(SessionStorageService);
  public platformId = inject(PLATFORM_ID);
  public email: string="";
  public password: string="";
  public error: string ="";
  constructor(private http: HttpClient, private router: Router){
  }
  public submit(): void{
    this.http.post<StringMap>("http://localhost:5118/api/user/login",{
      email:this.email,
      password:this.password,
    }, {
      withCredentials: true 
    }).subscribe({
      next: (response: StringMap) => {
        if(response){
          if(isPlatformBrowser(this.platformId)){
            this.sessionStorageService.setItem("user", "true");
            window.location.href = "/home";
          }
        }
      },
      error: (err: any)=>{
        console.log(err.error.text);
        this.error = err?.error.text ?? err?.error ?? "Login failed";
        this.email = "";
        this.password = "";
      },
    });
  }
}

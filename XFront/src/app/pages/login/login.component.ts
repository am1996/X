import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject, PLATFORM_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { SessionStorageService } from '../../Services/SessionStorage/session-storage.service';
// Update the import path to match the actual folder and file casing

type StringMap = {[key:string]:string};

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule,HttpClientModule],
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
      error: (err: StringMap)=>{
        this.error = err["message"];
        this.email = "";
        this.password = "";
      },
    });
  }
}

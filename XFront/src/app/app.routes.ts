import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { authGuard } from './guards/AuthGuard/auth.guard';
import { guestGuard } from './guards/GuestGuard/guest.guard';

export const routes: Routes = [
    {path:"", component: HomeComponent, pathMatch: "full"},
    {path:"login",loadComponent: () => LoginComponent, canMatch: [authGuard]},
    {path:"register",loadComponent: () => RegisterComponent, canMatch: [authGuard]},
    {path: "home", loadComponent: () => HomeComponent, canMatch: [authGuard]},
];

import { Routes } from '@angular/router';
import { authGuard } from './guards/AuthGuard/auth.guard';
import { guestGuard } from './guards/GuestGuard/guest.guard';
import { StartComponent } from './start/start.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { HomeComponent } from './pages/home/home.component';

export const routes: Routes = [
    {path: "", loadComponent: () => StartComponent, canMatch: [guestGuard],pathMatch: "full"},
    {path: "home", loadComponent: () => HomeComponent, canMatch: [authGuard]},
    {path:"login",loadComponent: () => LoginComponent, canMatch: [guestGuard]},
    {path:"register",loadComponent: () => RegisterComponent, canMatch: [guestGuard]},
];

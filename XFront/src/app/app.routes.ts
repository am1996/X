import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { guestGuard } from './guards/guest.guard';

export const routes: Routes = [
    {path:"", component: HomeComponent, pathMatch: "full"},
    {path:"login",loadComponent: () => LoginComponent, canMatch: [guestGuard]},
    {path:"register",loadComponent: () => RegisterComponent, canMatch: [guestGuard]},
];

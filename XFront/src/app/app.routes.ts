import { Routes } from '@angular/router';
import { authGuard } from './guards/AuthGuard/auth.guard';
import { guestGuard } from './guards/GuestGuard/guest.guard';
import { StartComponent } from './start/start.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { HomeComponent } from './pages/home/home.component';
import { AddPostComponent } from './pages/add-post/add-post.component';
import { EditPostComponent } from './pages/edit-post/edit-post.component';
import { ShowPostComponent } from './pages/show-post/show-post.component';

export const routes: Routes = [
    {path: "", loadComponent: () => StartComponent},
    {path: "home", loadComponent: () => HomeComponent, canActivate: [authGuard]},
    {path: "addpost", loadComponent: () => AddPostComponent, canActivate: [authGuard]},
    {path: "editpost/:id", loadComponent: () => EditPostComponent, canActivate: [authGuard]},
    {path: "showPost/:id", loadComponent: () => ShowPostComponent, canActivate: [authGuard]},
    {path:"login", loadComponent: () => LoginComponent, canActivate: [guestGuard]},
    {path:"register", loadComponent: () => RegisterComponent, canActivate: [guestGuard]},
];

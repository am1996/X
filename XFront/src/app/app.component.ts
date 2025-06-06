import { isPlatformBrowser } from '@angular/common';
import { Component, inject, PLATFORM_ID } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { SessionStorageService } from './Services/SessionStorage/session-storage.service';
import { LoaderComponent } from './components/loader/loader.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,RouterModule,LoaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  public title = 'X';
  public loggedIn: boolean | null = null;
  public sessionStorageService = inject(SessionStorageService);
  constructor(){
    const user = this.sessionStorageService.getItem('user');
    this.loggedIn = !!user;
  }
  logOut(){
    this.sessionStorageService.removeItem('user');
    this.loggedIn = false;
  }
}

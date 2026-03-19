import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SessionStorageService } from '../Services/SessionStorage/session-storage.service';

@Component({
  selector: 'app-start',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './start.component.html',
  styleUrl: './start.component.css'
})
export class StartComponent {
  public loggedIn: boolean | null = null;
  public sessionStorageService = inject(SessionStorageService);
  constructor(){
    const user = this.sessionStorageService.getItem('user');
    this.loggedIn = !!user;
  }
}

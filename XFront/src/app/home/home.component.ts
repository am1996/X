import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject, PLATFORM_ID, Signal } from '@angular/core';
import { SessionStorageService } from '../Services/SessionStorage/session-storage.service';
import { isPlatformBrowser } from '@angular/common';
import  { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [HttpClientModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  public platformId = inject(PLATFORM_ID);
  public isBrowsing = false;
  public data: Signal<any> | undefined;
  constructor(private http: HttpClient, private session: SessionStorageService){
    this.isBrowsing = isPlatformBrowser(this.platformId);
    const token = "Bearer " + this.session.getItem("jwt");
    this.data = toSignal(this.http.get("http://localhost:5118/api/post/",{
      headers : {
        Authorization: token,
      },
    }));
  }
}

import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject, OnInit, PLATFORM_ID, signal, Signal } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { SessionStorageService } from '../../Services/SessionStorage/session-storage.service';

@Component({
  standalone: true,
  imports: [HttpClientModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  public platformId = inject(PLATFORM_ID);
  public isBrowsing = false;
  public error: string = "";
  public data: Signal<any> | undefined;

  ngOnInit(){
    this.http.get("http://localhost:5118/api/post",{
      withCredentials: true
    }).subscribe({
      next: (response) => {
        this.data = signal(response);
      },
      error: (error) => {
        this.data = signal([]);
      }
    });
  }
  constructor(private http: HttpClient, private session: SessionStorageService){
  }
}

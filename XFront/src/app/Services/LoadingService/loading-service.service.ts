import { isPlatformBrowser } from '@angular/common';
import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  public platformId = inject(PLATFORM_ID);
  private _loading = new BehaviorSubject<boolean>(false);
  private activeRequests = 0;
  public isLoading: Observable<boolean> = this._loading.asObservable();

  constructor() { }
  showLoading(){
    if (isPlatformBrowser(this.platformId)) {
      this.activeRequests++;

      if (this.activeRequests === 1) {
        this._loading.next(true);
      }
    }
  }
  hideLoading(){
    if (isPlatformBrowser(this.platformId)) {
      this.activeRequests--;
      if (this.activeRequests <= 0) {
        this._loading.next(false);
      }
    }
  }
}

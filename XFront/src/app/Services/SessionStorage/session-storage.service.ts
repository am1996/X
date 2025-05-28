import { isPlatformBrowser } from '@angular/common';
import { inject, Injectable, PLATFORM_ID } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionStorageService {
  public platformId: Object = inject(PLATFORM_ID);
  getItem(key: string): string | null {
    if(isPlatformBrowser(this.platformId)){
      return sessionStorage.getItem(key);
    }
    return null;
  }
  setItem(key:string,value: string): boolean{
    if(isPlatformBrowser(this.platformId)){
      sessionStorage.setItem(key,value);
      return true;
    }
    return false;
  }
  clearToken(key: string): boolean{
    if(isPlatformBrowser(this.platformId)){
      sessionStorage.removeItem(key);
      return true;
    }
    return false;
  }
}

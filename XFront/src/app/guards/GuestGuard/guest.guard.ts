import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { map, catchError, of } from 'rxjs';
import { SessionStorageService } from '../../Services/SessionStorage/session-storage.service';

export const guestGuard: CanMatchFn = () => {
  const sessionStorageService = inject(SessionStorageService);
  if(sessionStorageService.getItem('user')) {
    return of(false);
  }else{
    return of(true);
  }
};
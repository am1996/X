import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { map, catchError, of } from 'rxjs';
import { SessionStorageService } from '../../Services/SessionStorage/session-storage.service';

export const authGuard: CanMatchFn = () => {
  const sessionStorageService = inject(SessionStorageService);
  if(sessionStorageService.getItem('user')) {
    return of(true);
  }else{
    return of(false);
  }
};
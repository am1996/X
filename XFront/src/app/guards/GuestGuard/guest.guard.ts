import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { map, catchError, of } from 'rxjs';
import { SessionStorageService } from '../../Services/SessionStorage/session-storage.service';

export const guestGuard: CanMatchFn = () => {
  const http = inject(HttpClient);
  const router = inject(Router);
  const sessionStorageService = inject(SessionStorageService);
  if(sessionStorageService.getItem('user')) {
    router.navigate(['/home']);
    return of(false);
  }
  return http.get('http://localhost:5118/api/authcheck', {
    withCredentials: true
  }).pipe(
    map(() => {
      sessionStorageService.setItem('user', 'true');
      return false;
    }), // ✅ User is authenticated
    catchError(() => {
      router.navigate(['/home']);
      return of(true); // ❌ Redirect if not authenticated
    })
  );
};
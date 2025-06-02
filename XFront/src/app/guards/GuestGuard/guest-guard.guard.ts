import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { map, catchError, of } from 'rxjs';

export const guestGuard: CanMatchFn = () => {
  const http = inject(HttpClient);
  const router = inject(Router);

  return http.get('http://localhost:5118/api/authcheck', {
    withCredentials: true
  }).pipe(
    map(() => false), // ✅ User is authenticated
    catchError(() => {
      router.navigate(['/home']);
      return of(true); // ❌ Redirect if not authenticated
    })
  );
};
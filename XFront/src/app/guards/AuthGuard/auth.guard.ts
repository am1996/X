import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { map, catchError, of } from 'rxjs';

export const authGuard: CanMatchFn = () => {
  const http = inject(HttpClient);
  const router = inject(Router);

  return http.get('http://localhost:5118/api/authcheck', {
    withCredentials: true
  }).pipe(
    map(() => true), // ✅ User is authenticated
    catchError(() => {
      router.navigate(['/login']);
      return of(false); // ❌ Redirect if not authenticated
    })
  );
};
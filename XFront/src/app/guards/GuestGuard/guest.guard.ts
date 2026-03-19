import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { SessionStorageService } from '../../Services/SessionStorage/session-storage.service';

export const guestGuard: CanActivateFn = () => {
  const sessionStorageService = inject(SessionStorageService);
  const router = inject(Router);
  if (sessionStorageService.getItem('user')) {
    return router.createUrlTree(['/home']);
  }
  return true;
};
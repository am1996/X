import { CanMatchFn, Router } from '@angular/router';
import { SessionStorageService } from '../Services/SessionStorage/session-storage.service';
import { inject } from '@angular/core';

export const guestGuard: CanMatchFn = (route, state) => {
  const storage: SessionStorageService = inject(SessionStorageService);
  const router: Router = inject(Router);
  const token = storage.getItem("jwt");
  if(token){
    router.navigate(["/"]);
    return false;
  }
  return true;
};

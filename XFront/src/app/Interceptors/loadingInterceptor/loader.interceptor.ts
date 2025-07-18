// src/app/interceptors/loading.interceptor.ts
import { PLATFORM_ID } from '@angular/core';
import {HttpHandlerFn, HttpInterceptorFn, HttpRequest} from '@angular/common/http';
import { finalize } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';
import { inject } from '@angular/core'; // Import inject
import { LoadingService } from '../../Services/LoadingService/loading-service.service';

// For standalone, functional interceptors are generally preferred
export const loadingInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const loadingService = inject(LoadingService); // Inject the loading service
  const platformId = inject(PLATFORM_ID);
  console.log('Loading interceptor triggered');

  if (isPlatformBrowser(platformId)) {
    loadingService.showLoading();

    return next(req).pipe(
      finalize(() => {
        loadingService.hideLoading();
      })
    );
  } else {
    // If not in browser, just pass the request through
    return next(req);
  }
};
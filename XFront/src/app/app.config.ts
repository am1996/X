import { ApplicationConfig, mergeApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { loadingInterceptor } from './Interceptors/loadingInterceptor/loader.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([loadingInterceptor])),
    provideRouter(routes),
    provideClientHydration(),
    provideHttpClient(withFetch())
  ]
};

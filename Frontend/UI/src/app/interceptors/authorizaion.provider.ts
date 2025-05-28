import { Provider } from '@angular/core';

// Injection token for the Http Interceptors multi-provider
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthorizationInterceptor } from './authorization.interceptor';

/** Provider for the Noop Interceptor. */
export const authorizationInterceptorProvider: Provider =
  { provide: HTTP_INTERCEPTORS, useClass: AuthorizationInterceptor, multi: true };
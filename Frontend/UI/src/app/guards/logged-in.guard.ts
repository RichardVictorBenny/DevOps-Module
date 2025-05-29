import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../authentication/services/auth.service';

export const loggedInCanActivateFunction: CanActivateFn = () => {
  const authenticationService = inject(AuthService);
  const router = inject(Router);
  
  if (authenticationService.IsAuthenticated()) {
    return true;
  }

  // not logged in so redirect to login page with the return url
  return router.createUrlTree(['login']);
};

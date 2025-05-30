/**
 * File: logged-in.guard.ts
 * Author: Richard Benny
 * Purpose: Defines a route guard to check if the user is authenticated before allowing access to certain routes.
 * Dependencies: @angular/core, @angular/router, ../authentication/services/auth.service
 *
 * This guard uses the AuthService to determine if a user is authenticated.
 * If authenticated, navigation proceeds. Otherwise, the user is redirected to the login page.
 */

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

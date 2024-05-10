import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthService } from '../_services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    /*
    const currentUser = this.authService.currentUserValue;
    if (currentUser?.role == 1) {
      // send the customer to login page, if requested endpoint has roles which customer does not have
      if (route.data['roles'] && route.data['roles'].indexOf(currentUser.role) === -1)  {
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
      }
      // current customer exists, meaning customer logged in, so return true
      return true;
    }

    // in general, if not currently logged in, redirect to login page with the return url
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
    */
   return true;
  }
}

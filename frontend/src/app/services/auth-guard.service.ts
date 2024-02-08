import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { LoginStatusService } from './login-status.service';
@Injectable({
  providedIn: 'root'
})
export class AuthGuardService {

  constructor(private loginService : LoginStatusService, private router : Router) { }

  canActivate(route:ActivatedRouteSnapshot, state : RouterStateSnapshot) {
    if (this.loginService.isLoggedIn()) {
      return true;  // Allow access if the user is authenticated
    } else {
      // Redirect to the login page if the user is not authenticated
      this.router.navigate(['/login']);
      return false;
    }
  }
}

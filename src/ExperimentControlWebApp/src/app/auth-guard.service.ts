import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable()
export class AuthGuardService implements CanActivate {
  constructor(public auth2: OAuthService, public router: Router) {}

  canActivate(): boolean {
    if (!this.auth2.hasValidAccessToken()) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}

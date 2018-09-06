import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-login',
  template: `
  <button class="btn btn-default" (click)="login()">
  Login
</button>
<button class="btn btn-default" (click)="logoff()">
  Logout
</button>
  `,
  styles: []
})
export class LoginComponent {
  constructor(private oauthService: OAuthService) {}

  public login() {
    this.oauthService.initImplicitFlow();
  }

  public logoff() {
    this.oauthService.logOut();
  }
}

import { Component, OnInit } from '@angular/core';
import { OAuthService } from '../../../node_modules/angular-oauth2-oidc';

@Component({
  selector: 'app-dashboard',
  template: `
    <p>
      dashboard works!
    </p>
    <div *ngIf="auth.hasValidIdToken()">
    Logged In
    </div>
    <div *ngIf="!auth.hasValidIdToken()">
    Logged Out
    </div>
    <app-experimentoverview></app-experimentoverview>
  `,
  styles: []
})
export class DashboardComponent {
  constructor(public auth: OAuthService) {}
}

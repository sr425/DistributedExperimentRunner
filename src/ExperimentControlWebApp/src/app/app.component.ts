import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { authConfig } from './auth.config';
import { filter, map } from 'rxjs/operators';
import {
  OAuthService,
  JwksValidationHandler,
  NullValidationHandler
} from 'angular-oauth2-oidc';

@Component({
  selector: 'app-root',
  template: `
    <div class="app-container mat-typography" (keydown.escape)="sidenav.close()">
      <mat-toolbar color='primary'>
        <mat-toolbar-row>
          <button mat-icon-button (click)="sidenav.toggle()"><mat-icon>menu</mat-icon></button>
          <span  routerLink="/">Experiment Controller</span>
          <span class="left-right-spacer"></span>
          <button mat-mini-fab><i class="material-icons">face</i></button>
        </mat-toolbar-row>
      </mat-toolbar>

      <mat-sidenav-container class="sidenav-container">
        <mat-sidenav #sidenav mode="side" (keydown.escape)="sidenav.close()">
          <mat-nav-list>
            <mat-list-item *ngFor="let nav of navigationContent" (click)="navigate(nav.route);sidenav.close()">
              <mat-icon>{{nav.icon}}</mat-icon>
              <span>{{nav.text}}</span>
            </mat-list-item>
          </mat-nav-list>
        </mat-sidenav>

        <mat-sidenav-content>
        <router-outlet></router-outlet>
        </mat-sidenav-content>
      </mat-sidenav-container>
    </div>
    `,
  styles: [
    `
      .left-right-spacer {
        flex: 1 1 auto;
      }

      .app-container {
        display: flex;
        flex-direction: column;
        position: absolute;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
      }

      .sidenav-container {
        flex: 1;
      }

      .nav-btn {
        width: 100%;
        flex: 1 1 auto;
      }
    `
  ]
})
export class AppComponent {
  title = 'ClientApp';
  navigationContent = [
    { icon: 'cloud', text: 'Experiments', route: '/dashboard' },
    { icon: 'folder', text: 'Datasets', route: '/datasets' }
  ];

  constructor(public router: Router, private oauthService: OAuthService) {
    this.configureWithNewConfigApi();
  }

  navigate(route: string): void {
    this.router.navigate([route]);
  }

  /*private configureWithNewConfigApi() {
    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }*/

  private configureWithNewConfigApi() {
    this.oauthService.configure(authConfig);
    //this.oauthService.setStorage(localStorage);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin();

    // Optional
    this.oauthService.setupAutomaticSilentRefresh();
  }
}

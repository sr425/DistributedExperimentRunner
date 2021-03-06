import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(public auth: OAuthService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (
      new RegExp('^http://localhost:5000/api').test(request.url) &&
      this.auth.hasValidIdToken()
    ) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.auth.getIdToken()}`
        }
      });
      return next.handle(request);
    } else {
      return next.handle(request);
    }
  }
}

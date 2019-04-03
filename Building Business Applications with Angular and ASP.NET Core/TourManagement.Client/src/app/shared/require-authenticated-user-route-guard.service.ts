import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { OpenIdConnectService } from './open-id-connect.service';

@Injectable()
export class RequireAuthenticatedUserRouteGuardService implements CanActivate {

  constructor(private openIdConnectService: OpenIdConnectService, private router: Router) { }

  canActivate() {
    if (this.openIdConnectService.userAvailable) {
      return true;
    } else {
      // trigger signin
      this.openIdConnectService.triggerSignIn();
      return false;
    }
  }

}

import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client';
import { environment } from '../../environments/environment';

@Injectable()
export class OpenIdConnectService {
  private userManager: UserManager = new UserManager(environment.OpenIdConnectSettings);
  private currentUser: User;

  get userAvailable(): boolean {
    return this.currentUser != null;
  }

  get user(): User {
    return this.currentUser;
  }
  constructor() { 
    this.userManager.clearStaleState();

    this.userManager.events.addUserLoaded(user => {
      if (!environment.production){
        console.log('User loaded.', user);
      }
      this.currentUser = user;
    });

    this.userManager.events.addUserUnloaded((e) => {
      if (!environment.production){
        console.log('User unloaded.');
      }
      this.currentUser = null;
    });
  }

  triggerSignIn(){
    this.userManager.signinRedirect().then(function () {
      if (!environment.production) {
        console.log('Redirection to signin triggered.');
      }
    });
  }

  handleCallBack(){
    this.userManager.signinRedirectCallback().then(function (user) {
      if (!environment.production){
        console.log('Callback after signin handled..', user);
      }
    });
  }

  triggerSignOut(){
    this.userManager.signoutRedirect().then(function (resp) {
      if (!environment.production) {
        console.log('Redirection to sign out triggered.');
      }
    });
  }

}

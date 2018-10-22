import { Component } from '@angular/core';
import {  } from "automapper-ts";
import { OpenIdConnectService } from './shared/open-id-connect.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Pluralsight Demo';  

  constructor(private openIdConnectService: OpenIdConnectService) {  }

  ngOnInit() {
    var path = window.location.pathname;
    if (path != "/signin-oidc") {
      if (!this.openIdConnectService.userAvailable) {
        this.openIdConnectService.triggerSignIn();
      }
    }
  }
}

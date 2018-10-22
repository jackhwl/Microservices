import { OpenIdConnectService } from '../app/shared/open-id-connect.service';
// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  apiUrl: 'https://localhost:44327/api/',
  OpenIdConnectSettings: {
    authority: 'https://localhost:44398/',
    client_id: 'tourmanagementclient',
    redirect_uri: 'https://localhost:4200/signin-oidc',
    scope: 'openid profile roles',
    response_type: 'id_token'
  }
};

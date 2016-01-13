# AuthCentral


Master build: [![Build Status](http://fswjenkins01.foodservicewarehouse.com:8080/buildStatus/icon?job=build_voltron_docs)](http://fswjenkins01.foodservicewarehouse.com:8080/job/build_voltron_docs/)

![openid_certified](https://cloud.githubusercontent.com/assets/1454075/7611268/4d19de32-f97b-11e4-895b-31b2455a7ca6.png)

[Certified](http://openid.net/certification/) OpenID Connect implementation.


## Overview ##

A .NET/Katana-based server implementing single sign-on and access control for modern web applications 
and APIs using OpenID Connect and OAuth2 protocols.

A bigger picture: 
* [Secure Token Service - big picture](https://identityserver.github.io/Documentation/docs/overview/bigPicture.html)
* [Intro Video to OpenID Connect, OAuth2 and IdentityServer](http://www.ndcvideos.com/#/app/video/2651)

[OpenID Connect specification](http://openid.net/specs/openid-connect-core-1_0.html) / [OAuth2 specification](http://tools.ietf.org/html/rfc6749 "OAuth2 specification")

* uses for MembershipReboot, backed by MongoDB, as the user store
* uses for MongoDB persistence of configuration
* support for additional Katana authentication middleware (e.g. Google, Twitter, Facebook etc)
* support for WS-Federation

## Dev Environment Setup ##

In order to run the application, some setup is required...


1. Using the Certificates snapin in the mmc, import the ssl certificate found here:  [local-fsw.com.pfx](http://gitlab.fsw.com/ansible/fsw.cert/raw/master/files/local-fsw.com.pfx)
   The password for the certificate is found in the [local.yml file](http://gitlab.fsw.com/ansible/fsw.cert/blob/master/vars/local.yml#L5)
   **NOTE**: In order to avoid permissions problems install the certificate in the `Personal` store on the `Local Computer`.  If you 
   fail to do this, much debugging and crypto errors are in your future.

2. Once the certificate has been imported, right click on the local-fsw.com certificate and choose `all tasks` --> `manage private keys`.  
   Then, make sure your user has specific access to read the private key.  If not, add your user account to the list and ensure 
   the `read` permission is checked.

3. Bind the installed fsw.com certificate to the dev port 44333 so this is the cert that is used when running locally.  The 
   following must be run from a CMD prompt running as administrator.

        netsh http delete sslcert ipport=0.0.0.0:44333
        netsh http add sslcert ipport=0.0.0.0:44333 appid={12345678-db90-4b66-8b01-88f7af2e36bf} certhash=656de34b45066d8fc9d88a3952082a6121f80c82 certstorename=my

4. In order to avoid the need to run visual studio as administrator, run the following commands in an administrator command prompt:

        PS C:\Windows\system32> netsh http add urlacl url="https://+:44333/" user =Everyone
        URL reservation successfully added

        PS C:\Windows\system32> netsh http add urlacl url="http://+:8080/" user =Everyone
        URL reservation successfully added

5. At this point you will need an entry in your hosts file.  As an administrator, open notepad.exe.  The hosts file is 
   located at `c:\Windows\System32\drivers\etc\hosts`. Add the following entry:

        127.0.0.1	localhost auth1.local-fsw.com

At this point, you should be all set to run the app from visual studio.  Be sure to select the `web` command from the dropdown (not `iisexpress`)

### Building / Running from Command Line ###

1. dnu restore
2. dnu build
3. dnx web

To watch for style changes and automatically rebuild the stylesheets, use `gulp watch`.


## Related repositories ##
* [Identity Server](https://github.com/identityserver/IdentityServer3)
* [Access Token Validation](https://github.com/identityserver/IdentityServer3.AccessTokenValidation)
* [MembershipReboot support](https://github.com/identityserver/IdentityServer3.MembershipReboot)
* [WS-Federation plugin](https://github.com/identityserver/IdentityServer3.WsFederation)
* [Samples](https://github.com/IdentityServer/IdentityServer3.Samples)

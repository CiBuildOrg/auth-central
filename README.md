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

## Related repositories ##
* [Identity Server](https://github.com/identityserver/IdentityServer3)
* [Access Token Validation](https://github.com/identityserver/IdentityServer3.AccessTokenValidation)
* [MembershipReboot support](https://github.com/identityserver/IdentityServer3.MembershipReboot)
* [WS-Federation plugin](https://github.com/identityserver/IdentityServer3.WsFederation)
* [Samples](https://github.com/IdentityServer/IdentityServer3.Samples)
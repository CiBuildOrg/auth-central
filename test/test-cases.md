# Auth Central regression acceptance criteria
This file is an experiment by the architecture team to document test cases for AuthCentral in the form of BDD-style acceptance criteria.

Individuals wishing to contribute to this document should first read [Introducing BDD](http://dannorth.net/introducing-bdd/), or at the very least, the section on acceptance criteria.

**[Administrator actions](#administrator-actions)**
- [User Admin](#client-admin)
    - [Find a user account](#find-a-user-account)
    - [Edit a user's profile](#edit-a-users-profile)
    - [Edit a user's Email address](#edit-a-users-email-address)
    - [Create a new user account](#create-a-new-user-account)
    - [Disable a user account](#disable-a-user-account)
    - [Enable a user account](#enable-a-user-account)
    - [Find a user resource claim](#find-a-user-resource-claim)
    - [Edit a user resource claim](#edit-a-user-resource-claim)
    - [Create a new claim](#create-a-new-claim)
    - [Delete a user resource claim](#delete-a-user-resource-claim)
- [Server Admin](#client-admin)
    - [Find an oauth client](#find-an-oauth-client)
    - [Edit an oauth client](#edit-an-oauth-client)
    - [Create a new client](#create-a-new-client)
    - [Delete an oauth client](#delete-an-oauth-client)
    - [Find an oauth scope](#find-an-oauth-scope)
    - [Edit an oauth scope](#edit-an-oauth-scope)
    - [Create an oauth scope](#create-an-oauth-scope)
    - [Delete an oauth scope](#delete-an-oauth-scope)

**[User account actions](#user-account-actions)**
- [Log in](#log-in)
- [View permissions](#view-permissions)
- [Edit profile](#edit-profile)
- [Self-register](#self-register)
- [Confirm Email Address](#confirm-email-address)
- [Reset forgotten password](#reset-forgotten-password)
- [Change Password](#change-password)

## Administrator actions
### User Admin
#### Find a user resource claim
---

**Given**
- The user ("the admin") has the fsw:authcentral:admin resource claim

**When** the admin loads the Claim Admin screen (at Admin/UserClaim/Show/{userId})

**Then**
- The selected user's resource claims are displayed, 
- The selected user's *name* claims are not displayed

#### Edit a user resource claim
---

**Given**:
- The user ("the admin") has the fsw:authcentral:admin resource claim, and
- The admin has loaded the Claim Admin screen (at Admin/UserClaim/Show/{userId})
- The admin has changed the type and/or value of a claim

**When** The corresponding save button is clicked

**Then**
- The changes to the existing claim are saved
- No other claims are affected.

#### Create a new user resource claim
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Claim Admin screen (at Admin/UserClaim/Show/{userId})
- The user clicked the "Create Claim" button and entered values in the required form fields

**When** the Save button is clicked

**Then** the new user claim will be persisted to the user data store

#### Delete a user resource claim
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Claim Admin screen (at Admin/UserClaim/Show/{userId})

**When** the Delete button for the claim to delete is clicked

**Then**: the user claim will be removed from the user data store


#### Find a user account
---

#### Edit a user's Email address
---
**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the user profile screen (at Admin/UserProfile/Edit/{userid}), and  
- The user has made a change to the Email address

**When** the Trigger Change Request button is clicked

**Then** an e-mail will be sent to the user's new e-mail address to confirm it

#### Edit a user's Email address
---
**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the user profile screen (at Admin/UserProfile/Edit/{userid}), and  
- The user has made a valid change to the name fields

**When** the Save button is clicked

**Then** the user's profile will be updated

#### Create a new user account
---

**Given** that:
- The user:
  - is logged in, and
  - has the fsw_platform scope, and
  - has the fsw:authcentral:admin scope, and
  - has filled out the form (at /admin/account) with valid values, and

**When** the form is submitted,

**Then**:
- An account created e-mail should be sent to the e-mail address that was entered, and
- An Account Verified e-mail should be sent to the e-mail address that was entered, and
- A password reset e-mail should be sent to the e-mail address that was entered, and
- The links in all e-mails should begin with the same base URL as the service under test.

---

**Given** that a user has been created by an administrator,

**When** that user enters correct credentials into the login form, 

**Then** that user should be successfully redirected to a page with no errors or warnings.

#### Disable a user account
---

**Given** that an admin has loaded a page of users at /admin/user,

**When** a button labeled "Disable" is clicked,

**Then** 
- the account is disabled (cannot log in)
- On the subsequent page load, the button is replaced by an "Enable" button


#### Enable a user account
---

**Given** that an admin has loaded a page of users at /admin/user,

**When** a button labeled "Enable" is clicked,

**Then** 
- the account is enabled (can log in)
- On the subsequent page load, the button is replaced by an "Enable" button


### Server Admin

#### Find an oauth scope
---

#### Edit an oauth scope
---

#### Create an oauth scope
---

#### Delete an oauth scope
---


#### Create a new client
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has filled out the form (at /Admin/Client/Create)

**When** the form is submitted,

**Then**:
- The new client will be saved, 
- A success message will be displayed
- The following Allowed Scopes will be created: 
    - openid
    - profile
    - offline_access
    - fsw_platform


#### Find an oauth client
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has filled out the form (at /admin/client) with an existing ClientId

**When** the form is submitted,

**Then** the client admin screen should be populated with the specified client

---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has filled out the form (at /admin/client) with a nonexistent ClientId

**When** the form is submitted,

**Then** the client home screen should remain with a message informing the user that the client does not exist. 

#### Edit an oauth client
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has searched for and loaded the client to be edited

**When** the save button is clicked

**Then** any changes to the client will be persisted to the client data store

#### Delete an oauth client
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has searched for and loaded the client to be deleted

**When** the delete button is clicked

**Then** the loaded client will be deleted

#### Create a Client Secret
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Secret Admin screen (at Admin/ClientSecret/Show/{clientid})
- The user has clicked the "Create Secret" button and entered the required form fields

**When** the save button is clicked

**Then**: 
- The new client secret value will be hashed
- The new client secret with hashed value will be persisted to the client data store


#### Delete a Client Secret
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Secret Admin screen (at Admin/ClientSecret/Show/{clientid})

**When** the Delete button for the secret to delete is clicked

**Then**: 
- The client secret will be removed from the client data store


#### Create a Client Claim
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Claim Admin screen (at Admin/ClientClaim/Show/{clientid})
- The user has clicked the "Create Claim" button and entered the required form fields

**When** the save button is clicked

**Then** the new client claim will be persisted to the client data store


#### Delete a Client Claim
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Claim Admin screen (at Admin/ClientClaim/Show/{clientid})

**When** the Delete button for the secret to delete is clicked

**Then**: the client claim will be removed from the client data store


#### Create an Allowed Client Scope
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Allowed Client Scope Admin screen (at Admin/ClientAllowedScope/Edit/{clientid})
- The user has entred the required form field (allowedScope)

**When** the save button is clicked

**Then** the new allowed client scope will be persisted to the client data store


#### Update an Allowed Client Scope
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Allowed Client Scope Admin screen (at Admin/ClientAllowedScope/Edit/{clientid}), and  
- The user has made the Allowed Scope updates

**When** the save button is clicked

**Then** the allowed scope updates will be persisted to the client data store


#### Delete an Allowed Client Scope

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Allowed Client Scope Admin screen (at Admin/ClientAllowedScope/Edit/{clientid}), and  

**When** the Delete button for the scope to delete is clicked

**Then**: the allowed client scope will be removed from the client data store

---

#### Create an Allowed Client Redirect Uri
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Client Redirect Uri Admin screen (at Admin/ClientRedirectUri/Edit/{clientid})
- The user has entred the required form field (redirectUri)

**When** the save button is clicked

**Then** the new client redirect uri will be persisted to the client data store


#### Update an Allowed Client Redirect Uri
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Client Redirect Uri Admin screen (at Admin/ClientRedirectUri/Edit/{clientid})
- The user has made the Allowed Redirect Uri updates

**When** the save button is clicked

**Then** the client redirect uri change will be persisted to the client data store


#### Delete an Allowed Client Redirect Uri
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Client Redirect Uri Admin screen (at Admin/ClientRedirectUri/Edit/{clientid})

**When** the Delete button for the redirect uri to delete is clicked

**Then**: the redirect uri will be removed from the client data store


#### Create an Allowed Client Post Logout Uri
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Client Post Logout Uri Admin screen (at Admin/ClientLogouttUri/Edit/{clientid})
- The user has entred the required form field (logoutUri)

**When** the save button is clicked

**Then** the new client logout uri will be persisted to the client data store


#### Update an Allowed Client Post Logout Uri
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Client Post Logout Uri Admin screen (at Admin/ClientLogouttUri/Edit/{clientid})
- The user has made the Post Logout Uri updates

**When** the save button is clicked

**Then** the client logout uri change will be persisted to the client data store


#### Delete an Allowed Client Post Logout Uri
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Client Post Logout Uri Admin screen (at Admin/ClientLogouttUri/Edit/{clientid})

**When** the Delete button for the logout uri to delete is clicked

**Then**: the logout uri will be removed from the client data store


#### Create an Allowed Client CORS Origin
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Client Allowed CORS Origin Admin screen (at Admin/ClientAllowedCorsOrigin/Edit/{clientid})
- The user has entred the required form field (allowedCorsOrigin)

**When** the save button is clicked

**Then** the new client allowed client CORS Origin will be persisted to the client data store


#### Update an Allowed Client CORS Origin
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Client Allowed CORS Origin Admin screen (at Admin/ClientAllowedCorsOrigin/Edit/{clientid})
- The user has made the Allowed CORS Origin Update 

**When** the save button is clicked

**Then** the allowed client CORS Origin change will be persisted to the client data store


#### Delete an Allowed Client CORS Origin
---

**Given**:
- The user has the fsw:authcentral:admin resource claim, and
- The user has loaded the Client Allowed CORS Origin Admin screen (at Admin/ClientAllowedCorsOrigin/Edit/{clientid})

**When** the Delete button for the CORS Origin to delete is clicked

**Then**: the cors origin will be removed from the client data store


## User account actions
### Log in
---
**Given** that the login form has been filled out with correct credentials for an account that has been disabled by an admin

**When** the login form is submitted

**Then** a message is displayed to inform the user that the account is not allowed to log in

### View permissions
---

### Edit profile
---

### Self-register

**Given**
* That a user with the given username does not already exist
* That a user with the given email does not already exist
* That the password meets all criteria
* That the password and password confirmation match

**When**
* [ ] a user registers a new account

**Then**
* The user see a screen informing them to confirm their account
* The user will receive and email with a link to confirm or cancel the new account.

---

**Given**
* That a user has submitted a user registration request
* That the user received the registration confirmation email
* That the user goes to the confirm link in the confirmation email within 20 minutes of the registration

**When**
* [ ] The user enters their password into the confirmation page

**Then**
* The user will see a page informing them that their account is registered and confirmed.
* The user will receive an email informing them that their email is verified.

---

**Given**
* That a user with the given username already exists

**When**
* [ ] a user registers a new account

**Then**
* The user will see an error message stating that the username is already in use.

---

**Given**
* That a user with the given username does not already exist
* That a user with the given email address already exists

**When**
* [ ] a user registers a new account

**Then**
* The user will see an error message stating that the email address is already in use.

---

**Given**
* That a user with the given username does not already exist
* That a user with the given email address does not already exist
* That the password and password confirmation match
* That the entered password does not match password criteria

**When**
* [ ] a user registers a new account

**Then**
* The user will see an error message stating the password criteria

---

**Given**
* That a user with the given username does not already exist
* That a user with the given email address does not already exist
* That the password and password confirmation do not match.

**When**
* [ ] a user registers a new account

**Then**
* The user will see an error message stating that the password and password confirmation must match.

---

### Confirm Email Address
---

**Given**
* That the user received a "Confirm Email Request" email
* More than 20 minutes have passed since the request was initiated

**When**
* [ ] the user goes to the register confirmation link

**Then**
* The user will see a message stating that the token has expired

---

**Given**
* That the user received a "Confirm Email Request" email
* That the user goes to the confirm link in the confirmation email within 20 minutes of the registration

**When**
* [ ] The user enters the wrong password

**Then**
* The user will see an error message stating that the password entered is invalid.

---

**Given**
* That the user received a "Confirm Email Request" email
* That the user goes to the confirm link in the confirmation email within 20 minutes of the registration

**When**
* [ ] The user enters the correct password

**Then**
* The user will see an message stating that their address has been confirmed and they can log in.

---

### Reset forgotten password
---

**Given**
* that a user exists with a known email address

**When**
* [ ] the user enters the correct email address of the account

**Then**
* An email will be received at the entered email address with a link to confirm the password reset or cancel the password reset.

---

**Given**
* that a user has submitted a password reset request
* that the user received the password reset request email
* that the user goes to the password reset confirm page within 20 minutes of the request

**When**
* [ ] the user enters a new password that meets password criteria
* [ ] the user confirms their new password

**Then**
* the user should be able to log in using the new password
* the user should receive an email informing of a password change

---

**Given**

**When**
* [ ] the user enters an email that is not associated to an account

**Then**
* The user will see an error message stating the email is invalid.

---

**Given**
* that a user has submitted a password reset request
* that the user received the password reset request email

**When**
* [ ] the user goes to the password reset confirm page more than 20 minutes after the request

**Then**
* the user should see a message stating that the token has expired.

---

**Given**
* that a user has submitted a password reset request
* that the user received the password reset request email
* that the user goes to the password reset confirm page within 20 minutes of the request

**When**
* [ ] the user-entered password and confirm password fields do not match

**Then**
* the user should see a message stating that password and confirm password fields must match.

---

**Given**
* that a user has submitted a password reset request
* that the user received the password reset request email
* that the user goes to the password reset confirm page within 20 minutes of the request

**When**
* [ ] the user enters a password that does not match password validation criteria

**Then**
* The user should see an error message stating the password criteria.

### Change Password 
---

**Given**
* that the user exists with a known email address
* that the user is logged in
* that the user has filled out the form (at /useraccount/changepassword) with valid values

**When**
* [ ] the form is submitted

**Then**
* The user's password will be changed to the new password provided
* An email will be sent to the email address stored for the logged in user notifying the user, by email, that their password has been changed

---


### Verify Email
---

**Given**
* that the user exists with a known email address
* that the user is logged in
* that user has already vefified their email address

**When**
* [ ] the user navigates to the verify page (at /useraccount/register/verify)

**Then**
* The user will see a message informing them that the account has already been verified

---

**Given**
* that the user exists with a known email address
* that the user is logged in
* that user has NOT already vefified their email address

**When**
* [ ] the user navigates to the verify page (at /useraccount/register/verify)

**Then**
* The user will receive an email with a verification link it in for the user to click


---

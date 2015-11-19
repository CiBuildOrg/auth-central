# Auth Central regression acceptance criteria
This file is an experiment by the architecture team to document test cases for AuthCentral in the form of BDD-style acceptance criteria.

Individuals wishing to contribute to this document should first read [Introducing BDD](http://dannorth.net/introducing-bdd/), or at the very least, the section on acceptance criteria.

**[Administrator actions](#administrator-actions)**
- [Find a claim](#find-a-claim)
- [Edit a claim](#edit-a-claim)
- [Create a new claim](#create-a-new-claim)
- [Delete a claim](#delete-a-claim)
- [Find a scope](#find-a-scope)
- [Edit a scope](#edit-a-scope)
- [Create a new scope](#create-a-new-scope)
- [Delete a scope](#delete-a-scope)
- [Find a user account](#find-a-user-account)
- [Edit a user account](#edit-a-user-account)
- [Create a new user account](#create-a-new-user-account)
- [Delete a user account](#delete-a-user-account)
- [Find a client](#find-a-client)
- [Edit a client](#edit-a-client)
- [Create a new client](#create-a-new-client)
- [Delete a client](#delete-a-client)

**[User account actions](#user-account-actions)**
- [Log in](#log-in)
- [View permissions](#view-permissions)
- [Edit profile](#edit-profile)
- [Self-register](#self-register)
- [Reset forgotten password](#reset-forgotten-password)

## Administrator actions
### Find a claim
---

### Edit a claim
---

### Create a new claim
---

### Delete a claim
---

### Find a scope
---

### Edit a scope
---

### Create a new scope
---

### Delete a scope
---

### Find a user account
---

### Edit a user account
---

### Create a new user account
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

### Delete a user account
---

### Find a client
---

### Edit a client
---

### Create a new client
---

### Delete a client
---

## User account actions
### Log in
---

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

**Given**
* That a user has submitted a user registration request
* That the user received the registration confirmation email
* More than 20 minutes have passed since registration

**When**
* [ ] the user goes to the register confirmation link

**Then**
* The user will see a message stating that the token has expired

---

**Given**
* That a user has submitted a user registration request
* That the user received the registration confirmation email
* That the user goes to the confirm link in the confirmation email within 20 minutes of the registration

**When**
* [ ] The user enters the wrong password

**Then**
* The user will see an error message stating that the password entered is invalid.

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

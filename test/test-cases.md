# Auth Central regression acceptance criteria (test cases)

Individuals wishing to contribute to this document should first read [Introducing BDD](http://dannorth.net/introducing-bdd/), or at the very least, the section on acceptance criteria.

[Administrator actions](#administrator-actions)
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
  - has filled out the form (at /admin/account) with a username and email that is not already registered, and
  - has entered a password with 3 of the 4 character groups, and
  - entered the same password twice
  
**When** the form is submitted,

**Then**:
- An account created e-mail should be sent to the e-mail address that was entered, and
- An Account Verified e-mail should be sent to the e-mail address that was entered, and
- A password reset e-mail should be sent to the e-mail address that was entered.

---

**Given** that a user has been created by an administrator,  
**When** that user enters correct credentials into the login form,  
**Then** that user should be successfully redirected to a page with no errors or warnings.  

### Delete a user account

### Find a client
### Edit a client
### Create a new client
### Delete a client
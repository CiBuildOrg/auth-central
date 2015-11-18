## Auth Central regression acceptance criteria (test cases)

Individuals wishing to contribute to this document should first read [Introducing BDD](http://dannorth.net/introducing-bdd/), or at the very least, the section on acceptance criteria.

### Administrator actions


#### Find a claim

#### Edit a claim

#### Create a new claim

#### Delete a claim

#### Find a scope

#### Edit a scope

#### Create a new scope

#### Delete a scope

#### Find a user account

#### Edit a user account

#### Create a new user account
**Given** that:
- The user:
  - is logged in, and
  - has the fsw_platform scope, and
  - has the fsw:authcentral:admin scope, and
  - fills out the form (at /admin/account) with a username and email that is not already registered, and
  - selects a password with 3 of the 4 character groups, and
  - enters the same password twice
  
**When** the form is submitted,

**Then**:
- A new user should be created with the requested credentials, and
- The user document should have the property IsAccountVerified set to `true`, and
- An account created e-mail should be sent to the e-mail address that was entered, and
- An Account Verified e-mail should be sent to the e-mail address that was entered, and
- A password reset e-mail should be sent to the e-mail address that was entered

#### Delete a user account

#### Find a client
#### Edit a client
#### Create a new client
#### Delete a client
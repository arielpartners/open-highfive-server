Feature: AdministerUsers
	In order to control access to the application
	As an administrator
	I want to administer users

@Ignore
Scenario: Create a new user
	Given A user with email account DefinitelyNotInTheDatabaseUser@test.com does not have an account
	When I create an user account for them
	| Email            |
	| newUser@test.com |
	Then an account should be created for them
	| Email            |
	| newUser@test.com |
	And they can login
	| Email            |
	| newUser@test.com |

@Ignore
Scenario: Delete a user
	Given A user exists
	| Email                 |
	| existingUser@test.com |
	When I delete the user
	| Email                 |
	| existingUser@test.com |
	Then The user is removed from the system
	| Email                 |
	| existingUser@test.com |
	And They can no longer login
	| Email                 |
	| existingUser@test.com |


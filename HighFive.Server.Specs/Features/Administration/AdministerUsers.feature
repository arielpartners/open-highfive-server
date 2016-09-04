Feature: AdministerUsers
	In order to control access to the application
	As an administrator
	I want to administer users

@Ignore
Scenario: Create a new user
	Given A user with email accountDefinitelyNotInTheDatabaseUser@test.com does not have an account
	When I create an user account with email of accountDefinitelyNotInTheDatabaseUser@test.com password of passw0rd and Organization of Ariel
	Then an account should be created for them with a username newUser@test.com

	@Ignore
Scenario: Delete a user
	Given A user with username existingUser@test.com and password passw0rd exists
	When I delete the user with username existingUser@test.com
	Then The user with username existingUser@test.com is removed from the database


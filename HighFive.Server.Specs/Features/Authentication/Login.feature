Feature: Login
	In order to send kudos to my coworker
	As a user
	I want to login to the recognition system

Background:
	Given The following user exists:
	| Email               | Password | FirstName | LastName | Organization  | Password |
	| test.user@email.com | password | Test      | User     | arielpartners | password |

Scenario: Successful login
	When I login with the following information:
	| Email               | Password |
	| test.user@email.com | password |

	Then the login will be successful
	And the following information will be returned
	| Email               | Password | FirstName | LastName | Organization  |
	| test.user@email.com | password | Test      | User     | arielpartners |
		
Scenario: Unsuccessful login wrong password
	When I login with the following information:
	| Email               | Password      |
	| test.user@email.com | wrongpassword |
	Then the login will be unsuccessful


Feature: Logout
	In order to prevent unauthorized access to my account
	As a user
	I want to logout from the recognition system when I am done using it

Background:
	Given The following user exists:
	| Email               | Password | FirstName | LastName | Organization  |
	| test.user@email.com | password | Test      | User     | arielpartners |

@ignore
Scenario: Logout successful
	Given the following logged in user:
	| Email               | Password |
	| test.user@email.com | password |
	When I logout
	Then I should be logged out of the system

@ignore
Scenario: Cannot logout if not logged in
	Given I am not logged in
	When I logout
	Then I should receive an error:
	| Code | message     |
	| 400  | Bad Request |

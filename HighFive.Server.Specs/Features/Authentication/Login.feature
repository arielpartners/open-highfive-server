Feature: Login
	In order to send kudos to my coworker
	As a user
	I want to login to the recognition system

Background:
	Given The following user exists:
	| Email               | Password | FirstName | LastName | Organization  |
	| test.user@email.com | password | Test      | User     | arielpartners |

@ignore
Scenario: Successful login
	Given I fill in the login form info:
	| Email               | Password |
	| test.user@email.com | password |
	When I login
	Then the login will be successful
	And the following information will be returned
	| Email               | Password | FirstName | LastName | Organization  |
	| test.user@email.com | password | Test      | User     | arielpartners |
		
@ignore
Scenario: Unsuccessful login
	Given I fill in the  login information:
	| Email               | Password      |
	| test.user@email.com | wrongpassword |
	When I login
	Then the login will be unsuccessful

@ignore
Scenario: Validate login field length
	Given I fill in the login information:
	| Email               | Password                                            |
	| test.user@email.com | 123456789012345678901234567890123456789012345678901 |
	When I login
	Then the login will be unsuccessful
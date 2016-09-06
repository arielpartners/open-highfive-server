Feature: Login
	In order to see my kudos
	As a user
	I want to login to the system

Background:
	Given The following user exists:
	| Email                | Password | FirstName | LastName | Organization | Password |
	| test.user@hq.dhs.gov | password | Test      | User     | dhs          | password |

@ignore
Scenario: Successful login
	When I login with the following information:
	| Email               | Password |
	| test.user@email.com | password |

	Then the login will be successful
	And the following information will be returned
	| Email               | Password | FirstName | LastName | Organization |
	| test.user@email.com | password | Test      | User     | dhs          |

@ignore
Scenario: View All my Recognitions
	When I view the home page
	Then I should see a list of my recognitions sorted most recent first:
	| Sender Email      | Receiver Email   | Organization Name | CorporateValue Name | Points | DateCreated       | Description                            |
	| matthew@email.com | sue@email.com    | dhs               | Honesty             | 10     | 8/7/2016 14:21:00 | Great job!                             |
	| john@email.com    | tom@email.com    | dhs               | Excellence          | 70     | 8/4/2016 09:44:00 | ipsum laurem                           |
	| nikhil@email.com  | jose@email.com   | dhs               | Respect             | 15     | 8/2/2016 19:04:00 | don't know what i would do without you |
	| joe@email.com     | suresh@email.com | dhs               | Integrity           | 30     | 8/2/2016 08:15:00 | Great job!                             |

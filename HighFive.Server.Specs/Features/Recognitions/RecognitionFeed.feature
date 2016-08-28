Feature: RecognitionFeed
	In order to stay informed of the kudos that are being sent to people in my organization
	As a user
	I want to be see recognitions

Background:
Given I am logged in as the following user:
| Email               | Password | FirstName | LastName | Organization  |
| test.user@email.com | password | Test      | User     | arielpartners |

@ignore
Scenario: View All Recognitions
	When I view the home page
	Then I should see a list of recognitions sorted by date

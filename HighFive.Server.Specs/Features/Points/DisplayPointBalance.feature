Feature: DisplayPointBalance
	In order to know how many points I have to spend 
	As a user
	I want to see my points balance on the home page after i login 
	;

Background:
Given I am logged in as the following user2:
| First Name | Last Name | email              | Organization Name | Points | 
| Joe        | Blow      | joe@yahoo.com      | Ariel Partners    | 200    | 

@framework
Scenario: Display point balance
	When I view my point balance
	Then I should see the following point balance:
	| Points |
	| 200    |

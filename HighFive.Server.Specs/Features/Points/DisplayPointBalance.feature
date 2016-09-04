Feature: DisplayPointBalance
	In order to know how many points I have to spend 
	As a user
	I want to see my points balance on the home page after i login 

@ignore 
@framework
Scenario: Display point balance
	Given I am logged in as the following user:
	| First Name | Last Name | email              | Organization Name | Points | 
	| Joe        | Blow      | joe.blow@yahoo.com | Ariel Partners    | 200    | 
	When I view my point balance
	Then I should see the following point balance:
	| Points |
	| 200    |

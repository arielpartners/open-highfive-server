Feature: CreateRecognition
	In order to recognize a co-worker 
	As a user
	I want to input my feedback

Scenario: F-13 Successfully create a recognition for an existing user
	Given I am logged in as the following user:
	| First Name | Last Name | Email              | Organization Name | Password |
	| Joe        | Blow      | joe.blow@email.com | Ariel Partners    | password |
	When I create the following recognition:
	| Sender Email       | Receiver Email         | Organization Name | CorporateValue Name | Points | Description |
	| joe.blow@email.com | suresh.nikam@email.com | Ariel Partners    | Integrity           | 1      | Great job!  |
	Then the system should confirm that the following recognition has been created:
	| Sender Email       | Receiver Email         | Organization Name | CorporateValue Name | Points | Description |
	| joe.blow@email.com | suresh.nikam@email.com | Ariel Partners    | Integrity           | 1      | Great job!  |

Scenario: F-7 Successfully create a recognition for a new user
	Given I am logged in as the following user:
	| First Name | Last Name | Email              | Organization Name | Password |
	| Joe        | Blow      | joe.blow@email.com | Ariel Partners    | password |
	And the following user does not exist:
	| Name       | Email                |
	| John Smith | john.smith@email.com |
	When I create the following recognition:
	| Sender Email       | New User Email       | New User Name | Organization Name | CorporateValue Name | Points | Description |
	| joe.blow@email.com | john.smith@email.com | John Smith    | Ariel Partners    | Integrity           | 1      | Great job!  |
	Then the system should confirm that the following recognition has been created:
	| Sender Email       | Receiver Email       | Organization Name | CorporateValue Name | Points | Description |
	| joe.blow@email.com | john.smith@email.com | Ariel Partners    | Integrity           | 1      | Great job!  |
	And the following user should exist:
	| Email                | Name       |
	| john.smith@email.com | John Smith | 

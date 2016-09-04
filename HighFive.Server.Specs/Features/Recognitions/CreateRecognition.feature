Feature: CreateRecognition
	In order to recognize a co-worker 
	As a user
	I want to input my feedback

@framework
Scenario: Successfully create a recognition
	Given I am logged in as the following user:
	| First Name | Last Name | Email              | Organization Name | Password |
	| Joe        | Blow      | joe.blow@email.com | Ariel Partners    | password |
	When I create the following recognition:
	| Sender Email       | Receiver Email         | Organization Name | CorporateValue Name | Points | Description |
	| joe.blow@email.com | suresh.nikam@email.com | Ariel Partners    | Integrity           | 30     | Great job!  |
	Then the system should confirm that the following recognition has been created:
	| Sender Email       | Receiver Email         | Organization Name | CorporateValue Name | Points | Description |
	| joe.blow@email.com | suresh.nikam@email.com | Ariel Partners    | Integrity           | 30     | Great job!  |

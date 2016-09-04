Feature: CreateRecognition
	In order to recognize a co-worker 
	As a user
	I want to input my feedback

@ignore
Scenario: Successfully create a recognition
	Given I am logged in as the following user:
	| First Name | Last Name | email              | Organization Name | Password |
	| Joe        | Blow      | joe.blow@yahoo.com | Ariel Partners    | password |
	When I create the following recognition:
	| Sender Email       | Receiver Email         | Organization Name | Description |
	| joe.blow@yahoo.com | suresh.nikam@gmail.com | Ariel Partners    | Great job!  |
	Then the system should confirm that the following recognition has been created:
	| Sender Email       | Receiver Email         | Organization Name | Description |
	| joe.blow@yahoo.com | suresh.nikam@gmail.com | Ariel Partners    | Great job!  | 

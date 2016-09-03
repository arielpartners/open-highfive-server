Feature: CreateRecognition
	In order to recognize a co-worker 
	As a user
	I want to input my feedback

@ignore
Scenario: Successfully create a recognition
	Given I have not entered any recognitions
	When I create the following recognition:
	| Sender        | Receiver         | CorporateValue | Points | DateCreated       | Description |
	| joe@yahoo.com | suresh@gmail.com | Integrity      | 30     | 8/2/2016 08:15:00 | Great job!  |
	Then the system should confirm that the following recognition has been created:
	| Sender        | Receiver         | CorporateValue | Points | DateCreated       | Description |
	| joe@yahoo.com | suresh@gmail.com | Integrity      | 30     | 8/2/2016 08:15:00 | Great job!  |

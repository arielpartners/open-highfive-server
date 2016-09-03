Feature: RecognitionFeed
	In order to stay informed of the kudos that are being sent to people in my organization
	As a user
	I want to be see the most recent recognitions

Background:
Given the following recognitions exist in the system:
| Sender            | Receiver         | CorporateValue | Points | DateCreated       | Description                            |
| joe@yahoo.com     | suresh@gmail.com | Integrity      | 30     | 8/2/2016 08:15:00 | Great job!                             |
| matthew@yahoo.com | sue@gmail.com    | Honesty        | 10     | 8/7/2016 14:21:00 | Great job!                             |
| sue@yahoo.com     | dave@gmail.com   | Vigilance      | 50     | 8/1/2016 10:15:00 | fantastic!                             |
| nikhil@yahoo.com  | jose@gmail.com   | Respect        | 15     | 8/2/2016 19:04:00 | don't know what i would do without you |
| john@yahoo.com    | tom@gmail.com    | Excellence     | 70     | 8/4/2016 09:44:00 | ipsum laurem                           |

@ignore
Scenario: View All Recognitions
	When I view the home page
	Then I should see a list of recognitions sorted most recent first:
	| Sender            | Receiver         | CorporateValue | Points | DateCreated       | Description                            |
	| matthew@yahoo.com | sue@gmail.com    | Honesty        | 10     | 8/7/2016 14:21:00 | Great job!                             |
	| john@yahoo.com    | tom@gmail.com    | Excellence     | 70     | 8/4/2016 09:44:00 | ipsum laurem                           |
	| nikhil@yahoo.com  | jose@gmail.com   | Respect        | 15     | 8/2/2016 19:04:00 | don't know what i would do without you |
	| joe@yahoo.com     | suresh@gmail.com | Integrity      | 30     | 8/2/2016 08:15:00 | Great job!                             |
	| sue@yahoo.com     | dave@gmail.com   | Vigilance      | 50     | 8/1/2016 10:15:00 | fantastic!                             |
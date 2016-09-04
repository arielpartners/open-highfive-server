Feature: RecognitionFeed
	In order to stay informed of the kudos that are being sent to people in my organization
	As a user
	I want to be see the most recent recognitions

Background:
Given the following recognitions exist in the system:
| SenderName        | ReceiverName     | OrganizationName | CorporateValueName | Points | DateCreated       | Description                            |
| joe@email.com     | suresh@email.com | Ariel Partners   | Integrity          | 30     | 8/2/2016 08:15:00 | Great job!                             |
| matthew@email.com | sue@email.com    | Ariel Partners   | Honesty            | 10     | 8/7/2016 14:21:00 | Great job!                             |
| sue@email.com     | dave@email.com   | Ariel Partners   | Vigilance          | 50     | 8/1/2016 10:15:00 | fantastic!                             |
| nikhil@email.com  | jose@email.com   | Ariel Partners   | Respect            | 15     | 8/2/2016 19:04:00 | don't know what i would do without you |
| john@email.com    | tom@email.com    | Ariel Partners   | Excellence         | 70     | 8/4/2016 09:44:00 | ipsum laurem                           |

@framework
Scenario: View All Recognitions
	When I view the home page
	Then I should see a list of recognitions sorted most recent first:
	| SenderName        | ReceiverName     | OrganizationName | CorporateValueName | Points | DateCreated       | Description                            |
	| matthew@email.com | sue@email.com    | Ariel Partners   | Honesty            | 10     | 8/7/2016 14:21:00 | Great job!                             |
	| john@email.com    | tom@email.com    | Ariel Partners   | Excellence         | 70     | 8/4/2016 09:44:00 | ipsum laurem                           |
	| nikhil@email.com  | jose@email.com   | Ariel Partners   | Respect            | 15     | 8/2/2016 19:04:00 | don't know what i would do without you |
	| joe@email.com     | suresh@email.com | Ariel Partners   | Integrity          | 30     | 8/2/2016 08:15:00 | Great job!                             |
	| sue@email.com     | dave@email.com   | Ariel Partners   | Vigilance          | 50     | 8/1/2016 10:15:00 | fantastic!                             |
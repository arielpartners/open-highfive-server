Feature: AdministerOrganization
	In order to start recognizing my peers
	As an administrator
	I want to be maintain organization information

@ignore
Scenario: Unable to administer org if not logged in
	Given I am not logged in
	And I attempt to create a new organization:
	| Name           |
	| Ariel Partners |
	Then I should receive an error:
	| Code | message         |
	| 401  | Unauthenticated |


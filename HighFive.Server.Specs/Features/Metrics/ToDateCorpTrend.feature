Feature: To Date Corporate Trends
	In order to understand recogntion trends
	As a user
	I want to be able to view the total recognitions created for our company for the week 
	grouped by corporate values

@ignore
Scenario: Zero recognitions created this week
	Given There are 0 recognitions in the database
	When I look at the weekly ring
	Then I will see 0 recognitions

@ignore
Scenario: 30 recognitions this to date
	Given The following data in the database
	| CorporateValue | Count |
	| Commitment     | 4     |
	| Courage        | 5     |
	| Excellence     | 6     |
	When I look the weekly ring
	Then I will see donut slices representing 15 total recognitions with the following data
	| CorporateValue | Count |
	| Commitment     | 4     |
	| Courage        | 5     |
	| Excellence     | 6     |


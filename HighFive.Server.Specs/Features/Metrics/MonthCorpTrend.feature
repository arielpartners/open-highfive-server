Feature: Month Corporate Trends
	In order to understand recogntion trends
	As a user
	I want to be able to view the total recognitions created for our company for the month 
	grouped by corporate values

@ignore
Scenario: Zero recognitions created this month
	Given There are 0 recognitions in the database
	When I look at the month ring
	Then I will see 0 recognitions

@ignore
Scenario: 20 recognitions this month
	Given The following data in the database
	| CorporateValue | Count | DateCreated  |
	| Commitment     | 4     | last 30 days | 
	| Courage        | 5     | last 30 days |
	| Excellence     | 6     | last 30 days |
	When I look the weekly ring
	Then I will see donut slices representing 15 total recognitions with the following data
	| CorporateValue | Count |
	| Commitment     | 4     |
	| Courage        | 5     |
	| Excellence     | 6     |


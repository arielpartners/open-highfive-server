Feature: Weekly Corporate Trends
	In order to understand recogntion trends
	As a user
	I want to be able to view the total recognitions created for our company for the week 
	grouped by corporate values

@F1-62
Scenario: Zero recognitions created this week
	Given There are 0 recognitions in the database
	| CorporateValue | Count | DateCreated |
	When I look at the weekly ring
	Then I will see 0 recognitions

@F1-62
Scenario:10 recognitions this week
	Given The following data in the database
	| CorporateValue | Count | DateCreated |
	| Commitment     | 4     | last seven days   |
	| Courage        | 5     | last seven days   |
	| Excellence     | 6     | last seven days   |
	When I look at the weekly ring
	Then I will see donut slices representing 15 total recognitions with the following data
	| CorporateValue | Count |
	| Commitment     | 4     |
	| Courage        | 5     |
	| Excellence     | 6     |

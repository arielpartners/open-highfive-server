Feature: SystemVersion
	In order to know that the system has new features or fixes
	As a power user
	I want to see the version number of the application

@framework
Scenario: See version number
	Given The system has version "0.0.7"
	When I view the version number
	Then the version should be "0.0.7"

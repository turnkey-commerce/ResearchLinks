Feature: Projects API
	In order to perform CRUD operations on the projects
	As a client of the Web Api
	I want to be able to Create, Update, Delete, and List projects

Scenario: Create a New Project
	Given the following project inputs and authentication
		| Field       | Value            |
		| ProjectName | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website
	Then a 200 status should be returned
	When the client gets the project by ID
	Then the response JSON should match the inputs
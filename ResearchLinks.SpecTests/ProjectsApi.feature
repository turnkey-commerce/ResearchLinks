Feature: Projects API
	In order to perform CRUD operations on the projects
	As a client of the Web Api
	I want to be able to Create, Update, Delete, and List projects

Scenario: Create a new project saves posted values.
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs

Scenario: Create a new project With bad credentials returns Unauthorized status
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | bogus        |
	When the client posts the inputs to the website
	Then a Unauthorized status should be returned

Scenario: Create a new project with no credentials returns Unauthorized status
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
	When the client posts the inputs to the website
	Then a Unauthorized status should be returned

Scenario: Get projects contains created project
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website
	Then a Created status should be returned
	When the client gets all projects
	Then the saved project should be in the list

Scenario: Get projects does not contain other users projects
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website
	Then a Created status should be returned
	Given the following project inputs and authentication
		| Field       | Value              |
		| Name        | Test Project 2     |
		| Description | Test Description 2 |
		| UserName    | john               |
		| Password    | john2013           |
	When the client posts the inputs to the website
	Then a Created status should be returned
	When the client gets all projects
	Then the saved project should be in the list

Scenario: Put project saves put values as edited
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs
	Given the following project inputs and authentication
		| Field       | Value                   |
		| Name        | Test Project Edited     |
		| Description | Test Description Edited |
		| UserName    | james                   |
		| Password    | james2013               |
	When the client puts the inputs to the website
	Then a OK status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs

Scenario: Put project by wrong user returns not found
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs
	Given the following project inputs and authentication
		| Field       | Value                   |
		| Name        | Test Project Edited     |
		| Description | Test Description Edited |
		| UserName    | john                   |
		| Password    | john2013               |
	When the client puts the inputs to the website
	Then a NotFound status should be returned

Scenario: Delete project removes project from database
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs
	Given the following project inputs and authentication
		| Field       | Value                  |
		| UserName    | james                  |
		| Password    | james2013              |
	When the client issues delete for the save project
	Then a NoContent status should be returned
	When the client gets the project by ID
	Then a NotFound status should be returned

Scenario: Delete project by wrong user returns not found
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs
	Given the following project inputs and authentication
		| Field       | Value                  |
		| UserName    | john                   |
		| Password    | john2013               |
	When the client issues delete for the save project
	Then a NotFound status should be returned

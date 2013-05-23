Feature: Projects API V2
	In order to support the new IsUrgent Feature in V2
	As a client of the Web Api
	I want to be able to Create and Update projects with the IsUrgent Property

Scenario: Create a new project saves posted values including IsUrgent
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for V2
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs including IsUrgent

Scenario: Create a new project where IsUrgent is missing returns bad request
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for V2
	Then a BadRequest status should be returned

Scenario: Put project saves put values with edited IsUrgent
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for V2
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs
	Given the following project inputs and authentication
		| Field       | Value                   |
		| Name        | Test Project Edited     |
		| Description | Test Description Edited |
		| IsUrgent    | False                   |
		| UserName    | james                   |
		| Password    | james2013               |
	When the client puts the inputs to the website for V2
	Then a OK status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs


Scenario: Put project where IsUrgent is missing returns bad request
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for V2
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs
	Given the following project inputs and authentication
		| Field       | Value                   |
		| Name        | Test Project Edited     |
		| Description | Test Description Edited |
		| UserName    | james                   |
		| Password    | james2013               |
	When the client puts the inputs to the website for V2
	Then a BadRequest status should be returned
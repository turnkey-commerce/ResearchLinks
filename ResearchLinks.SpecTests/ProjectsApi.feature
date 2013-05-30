Feature: Projects API
	In order to perform CRUD operations on the projects
	As a client of the Web Api
	I want to be able to Create, Update, Delete, and List projects

Scenario Outline: Create a new project saves posted values - All Versions
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for <Version>
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs for <Version>
Scenarios: 
	| Version |
	| V1      |
	| V2      |	

Scenario Outline: Create a new project where IsUrgent is missing returns bad request - V2
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for <Version>
	Then a BadRequest status should be returned
Scenarios: 
	| Version |
	| V2      |

Scenario Outline: Create a new project With bad credentials returns Unauthorized status - All Versions
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | bogus            |
	When the client posts the inputs to the website for <Version>
	Then a Unauthorized status should be returned
Scenarios:
	| Version |
	| V1      |
	| V2      |

Scenario Outline: Create a new project with no credentials returns Unauthorized status - All Versions
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| IsUrgent    | True             |
		| Description | Test Description |
	When the client posts the inputs to the website for <Version>
	Then a Unauthorized status should be returned
Scenarios:
	| Version |
	| V1      |
	| V2      |

Scenario Outline: Get projects contains created project - All Versions
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for <Version>
	Then a Created status should be returned
	When the client gets all projects
	Then the saved project should be in the list
Scenarios:
	| Version |
	| V1      |
	| V2      |


Scenario Outline: Get projects does not contain other users projects - All Versions
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for <Version>
	Then a Created status should be returned
	Given the following project inputs and authentication
		| Field       | Value              |
		| Name        | Test Project 2     |
		| Description | Test Description 2 |
		| IsUrgent    | True               |
		| UserName    | john               |
		| Password    | john2013           |
	When the client posts the inputs to the website for <Version>
	Then a Created status should be returned
	When the client gets all projects
	Then the saved project should be in the list
Scenarios:
	| Version |
	| V1      |
	| V2      |

Scenario Outline: Put project saves put values as edited - All Versions
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for <Version>
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs for <Version>
	Given the following project inputs and authentication
		| Field       | Value                   |
		| Name        | Test Project Edited     |
		| Description | Test Description Edited |
		| IsUrgent    | False                   |
		| UserName    | james                   |
		| Password    | james2013               |
	When the client puts the inputs to the website for <Version>
	Then a OK status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs for <Version>
Scenarios:
	| Version |
	| V1      |
	| V2      |

Scenario Outline: Put project where IsUrgent is missing returns bad request - V2
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for <Version>
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs for <Version>
	Given the following project inputs and authentication
		| Field       | Value                   |
		| Name        | Test Project Edited     |
		| Description | Test Description Edited |
		| UserName    | james                   |
		| Password    | james2013               |
	When the client puts the inputs to the website for <Version>
	Then a BadRequest status should be returned
Scenarios:
	| Version |
	| V2      |

Scenario Outline: Put project by wrong user returns not found - All Versions
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for <Version>
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs for <Version>
	Given the following project inputs and authentication
		| Field       | Value                   |
		| Name        | Test Project Edited     |
		| Description | Test Description Edited |
		| IsUrgent    | False                   |
		| UserName    | john                    |
		| Password    | john2013                |
	When the client puts the inputs to the website for <Version>
	Then a NotFound status should be returned
Scenarios:
	| Version |
	| V1      |
	| V2      |

Scenario Outline: Delete project removes project from database - All Versions
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for <Version>
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs for <Version>
	Given the following project inputs and authentication
		| Field       | Value                  |
		| UserName    | james                  |
		| Password    | james2013              |
	When the client issues delete for the save project
	Then a NoContent status should be returned
	When the client gets the project by ID
	Then a NotFound status should be returned
Scenarios:
	| Version |
	| V1      |
	| V2      |

Scenario Outline: Delete project by wrong user returns not found - All Versions
	Given the following project inputs and authentication
		| Field       | Value            |
		| Name        | Test Project     |
		| Description | Test Description |
		| IsUrgent    | True             |
		| UserName    | james            |
		| Password    | james2013        |
	When the client posts the inputs to the website for <Version>
	Then a Created status should be returned
	When the client gets the project by header location
	Then the saved project matches the inputs for <Version>
	Given the following project inputs and authentication
		| Field       | Value                  |
		| UserName    | john                   |
		| Password    | john2013               |
	When the client issues delete for the save project
	Then a NotFound status should be returned
Scenarios:
	| Version |
	| V1      |
	| V2      |

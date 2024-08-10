Feature: Users Service scenrios

  Scenario Outline: 01 - User Registration Happy Flow
    Given I register to the app with '<Username>' and '<Password>' and '<Firstname>' and '<LastName>' and '<Email>'
    When  I send the Request
    Then The registration request is succesfull
    Then The response parameters are valid with '<Username>'
    Examples:

      | name                     | Username        | Password | Firstname | LastName | Email            |
      | happy flow - iteration 1 | automationDemo8 | demo$$   | demoAuth  | user     | demo@example.com |


  Scenario Outline: 02 - Check and Delete Previusly Created User
    #Given I register to the app with '<Username>' and '<Password>' and '<Firstname>' and '<LastName>' and '<Email>'
    #When  I send the Request
    #Then The registration request is succesfull
    #Then The response parameters are valid with '<Username>'
    #
    Given I build the  GetUser Requrst with '<Username>' 
    When I send the GetUser request 
    Then I found the user
    Then the User is Valid with the paramaters and '<Username>' '<Firstname>' and '<LastName>' and '<Email>'
    Given I build the delete request
    When I send the delete request
    Then The delete Request is Valid
    Then The user with no longer exist on the DB
    
    Examples:
    | name                     | Username        | Password | Firstname | LastName | Email            |
    | happy flow - iteration 1 | automationDemo8 | demo$$   | demoAuth  | user     | demo@example.com |

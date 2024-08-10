Feature: Mapper Service scenrios

  Scenario Outline: 01 - date consistency
    Given I have a SQLite database
    When  there is data for <Month> and <Year> in the dev database
    Then the linked months and linked year should be consistent with the transaction date

    Examples: 
      
      | Name         | Month | Year |
      | iteration 1  | 1     | 2023 |
      | iteration 2  | 2     | 2023 |
      | iteration 3  | 3     | 2023 |
      | iteration 4  | 4     | 2023 |
      | iteration 5  | 5     | 2023 |
      | iteration 6  | 6     | 2023 |
      | iteration 7  | 7     | 2023 |
      | iteration 8  | 8     | 2023 |
      | iteration 9  | 9     | 2023 |
      | iteration 10 | 10    | 2023 |
      | iteration 11 | 11    | 2023 |
      | iteration 12 | 12    | 2023 |
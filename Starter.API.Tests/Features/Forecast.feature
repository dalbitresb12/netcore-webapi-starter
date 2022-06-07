Feature: Forecast API
As a Developer
I want to manage Forecasts through an API
In order to make it available for client applications

  Scenario: Listing Forecasts
    Given I am a client
    And the repository has data
      | Id | Date                     | TemperatureC | Summary |
      | 1  | 2022-06-04T18:52:54.912Z | 25           | Sunny   |
      | 2  | 2022-06-04T23:17:09.288Z | 19           | Chilly  |
      | 3  | 2022-06-05T04:17:23.939Z | 15           | Windy   |
    When a GET request is sent
    Then a response with status 200 is received
    And a list of Forecast resources is included in the body
      | Id | Date                     | TemperatureC | Summary |
      | 1  | 2022-06-04T18:52:54.912Z | 25           | Sunny   |
      | 2  | 2022-06-04T23:17:09.288Z | 19           | Chilly  |
      | 3  | 2022-06-05T04:17:23.939Z | 16           | Windy   |

  Scenario: Add Forecast without data
    Given I am a client
    When a POST request is sent
      | Date                     | TemperatureC | Summary |
      | 2022-06-04T18:52:54.912Z | 25           | Sunny   |
    Then a response with status 200 is received
    And a Forecast resource is included in the body
      | Id | Date                     | TemperatureC | Summary |
      | 1  | 2022-06-04T18:52:54.912Z | 25           | Sunny   |

  Scenario: Add Forecast with data
    Given I am a client
    And the repository has data
      | Id | Date                     | TemperatureC | Summary |
      | 1  | 2022-06-04T18:52:54.912Z | 25           | Sunny   |
    When a POST request is sent
      | Date                     | TemperatureC | Summary |
      | 2022-06-04T23:17:09.288Z | 19           | Chilly  |
    Then a response with status 200 is received
    And a Forecast resource is included in the body
      | Id | Date                     | TemperatureC | Summary |
      | 2  | 2022-06-04T23:17:09.288Z | 19           | Chilly  |

  Scenario: Add invalid Forecast
    Given I am a client
    When a POST request is sent
      | Invalid |
      | Invalid |
    Then a response with status 400 is received

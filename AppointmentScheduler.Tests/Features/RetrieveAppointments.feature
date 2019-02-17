Feature: Schedule New Appointment

    As a user
    I need to retrieve appointments
    So that I can track those appointments

    Background:
        Given the followings Clients
            | ClientId | Name      |
            | 1        | David     |
            | 1        | Chantelle |
            | 1        | Layla     |
            | 1        | Arya      |

        Given the following Medical Practioner
            | MedicalPractitionerId | Name          |
            | 1                     | Dr Watson     |
            | 1                     | Dr Livingston |

        Given the following Appointments
            | ClientId  | MedicalPractitionerId | Date       | FromTime | ToTime | Decription | Reason |
            | David     | Dr Livingston         | 2019-02-19 | 12:00    | 12:30  |            |        |
            | David     | Dr Watson             | 2019-02-19 | 12:30    | 13:00  |            |        |
            | David     | Dr Watson             | 2019-02-19 | 12:00    | 12:30  |            |        |
            | David     | Dr Livingston         | 2019-02-19 | 12:15    | 12:45  |            |        |
            | Chantelle | Dr Watson             | 2019-02-19 | 12:15    | 12:45  |            |        |
            | Chantelle | Dr Livingston         | 2019-02-19 | 14:15    | 14:45  |            |        |

    Scenario: Retrieve Appointments
        When I retrieve appointments for Client <Client>
        Then there must be <Count> appointments

        Examples:
            | Client    | Count |
            | David     | 4     |
            | Chantelle | 2     |

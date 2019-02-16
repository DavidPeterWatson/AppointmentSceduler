Feature: Schedule New Appointment

    As a user
    I need to schedule a new appointment
    So that I can track those appointments

    Background:
        Given the followings Clients
            | Id | Name      |
            | 1  | David     |
            | 1  | Chantelle |
            | 1  | Layla     |
            | 1  | Arya      |

        Given the following Practitioners
            | Id | Name          |
            | 1  | Dr Watson     |
            | 1  | Dr Livingston |

        Given the following Appointments
            | Client    | Practioner    | Date       | FromTime | ToTime | Decription | Reason |
            | David     | Dr Livingston | 2019-02-19 | 12:00    | 12:30  |            |        |
            | David     | Dr Watson     | 2019-02-19 | 12:30    | 13:00  |            |        |
            | David     | Dr Watson     | 2019-02-19 | 12:00    | 12:30  |            |        |
            | David     | Dr Livingston | 2019-02-19 | 12:15    | 12:45  |            |        |
            | Chantelle | Dr Watson     | 2019-02-19 | 12:15    | 12:45  |            |        |

    Scenario Outline: Retrieve Appointments
        When that appointment is scheduled
        Then a ConflictException must be thrown with message "<ConflictMessage>"
        And the calender should contain 1 appointment only

        Examples:
            | Description                    | Client    | Practioner    | Date       | From  | To    | ConflictMessage                                         |
            | Client Conflict                | David     | Dr Livingston | 2019-02-19 | 12:00 | 12:30 | Client already has an appointment for that timeslot     |
            | Practioner Conflict            | Chantelle | Dr Watson     | 2019-02-19 | 12:00 | 12:30 | Practioner already has an appointment for that timeslot |
            | Client and Practioner Conflict | David     | Dr Watson     | 2019-02-19 | 12:00 | 12:30 | Client already has an appointment for that timeslot     |
            | Client Overlap Conflict        | David     | Dr Livingston | 2019-02-19 | 12:15 | 12:45 | Client already has an appointment for that timeslot     |
            | Practioner Overlap Conflict    | Chantelle | Dr Watson     | 2019-02-19 | 12:15 | 12:45 | Practioner already has an appointment for that timeslot |
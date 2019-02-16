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

        And the following Practitioners
            | Id | Name          |
            | 1  | Dr Watson     |
            | 1  | Dr Livingston |

    Scenario: Schedule a new appointment on an empty calendar
        Given an empty schedule
        And an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30 for a "Basic checkup" so that "Medical application"
        When that appointment is scheduled
        Then the calender should contain 1 appointment only
        And the calender should contain an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30

    Scenario Outline: Schedule a new appointment without conflicting appointments
        Given an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30 for a "Basic checkup" so that "Medical application"
        And that appointment has been scheduled
        And an appointment for <Client> with <Practioner> on <Date> from <From> till <To> for a "Description" so that "Reason"
        When that appointment is scheduled
        Then the calender must contain an appointment for <Client> with <Practioner> on <Date> from <From> till <To> for a "Description" so that "Reason"

        Examples:
            | Example Description                      | Client    | Practioner    | Date       | From  | To    |
            | Same Client, same Pracitioner, diff time | David     | Dr Watson     | 2019-02-19 | 12:30 | 13:00 |
            | Same Client, diff Pracitioner, diff time | David     | Dr Livingston | 2019-02-19 | 12:30 | 13:00 |
            | Diff Client, same Pracitioner, diff time | Chantelle | Dr Watson     | 2019-02-19 | 12:30 | 13:00 |
            | Diff Client, diff Pracitioner, same time | Chantelle | Dr Livingston | 2019-02-19 | 12:00 | 12:30 |
            | Diff Client, diff Pracitioner, diff time | David     | Dr Watson     | 2019-02-19 | 12:30 | 13:00 |

    Scenario Outline: Attempt to Schedule a new appointment with a conflicting appointment
        Given an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30 for a "Basic checkup" so that "Medical application"
        And that appointment has been scheduled
        And an appointment for <Client> with <Practioner> on <Date> from <From> till <To> for a "Description" so that "Reason"
        When that appointment is scheduled
        Then a ConflictException must be thrown with the message "<ConflictMessage>"
        And the calender should contain 1 appointment only

        Examples:
            | Example Description            | Client    | Practioner    | Date       | From  | To    | ConflictMessage                                         |
            | Client Conflict                | David     | Dr Livingston | 2019-02-19 | 12:00 | 12:30 | Client already has an appointment for that timeslot     |
            | Practioner Conflict            | Chantelle | Dr Watson     | 2019-02-19 | 12:00 | 12:30 | Practioner already has an appointment for that timeslot |
            | Client and Practioner Conflict | David     | Dr Watson     | 2019-02-19 | 12:00 | 12:30 | Client already has an appointment for that timeslot     |
            | Client Overlap Conflict        | David     | Dr Livingston | 2019-02-19 | 12:15 | 12:45 | Client already has an appointment for that timeslot     |
            | Practioner Overlap Conflict    | Chantelle | Dr Watson     | 2019-02-19 | 12:15 | 12:45 | Practioner already has an appointment for that timeslot |
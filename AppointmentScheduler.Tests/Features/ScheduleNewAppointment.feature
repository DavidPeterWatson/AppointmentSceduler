Feature: Schedule New Appointment

    As a user
    I need to schedule a new appointment
    So that I can track those appointments

    Background:
        Given the followings Clients
            | ClientId  |
            | David     |
            | Chantelle |
            | Layla     |
            | Arya      |

        Given the following Medical Practitioners
            | MedicalPractitionerId |
            | Dr Watson             |
            | Dr Livingston         |

    Scenario: Schedule a new appointment on an empty calendar
        Given an empty schedule
        And an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30 for a "Basic checkup" so that "Medical application"
        When that appointment is scheduled
        Then the calender should contain 1 appointment only
        And the calender should contain an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30

    Scenario Outline: Schedule a new appointment without conflicting appointments
        Given an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30 for a "Basic checkup" so that "Medical application"
        And that appointment has been scheduled
        And an appointment for <ClientId> with <MedicalPractitionerId> on <Date> from <From> till <To> for a "Description" so that "Reason"
        When that appointment is scheduled
        Then the calender must contain an appointment for <Client> with <MedicalPractitionerId> on <Date> from <From> till <To> for a "Description" so that "Reason"

        Examples:
            | Example Description                               | ClientId  | MedicalPractitionerId | Date       | From  | To    |
            | Same Client, same MedicalPractitioners, diff time | David     | Dr Watson             | 2019-02-19 | 12:30 | 13:00 |
            | Same Client, diff MedicalPractitioners, diff time | David     | Dr Livingston         | 2019-02-19 | 12:30 | 13:00 |
            | Diff Client, same MedicalPractitioners, diff time | Chantelle | Dr Watson             | 2019-02-19 | 12:30 | 13:00 |
            | Diff Client, diff MedicalPractitioners, same time | Chantelle | Dr Livingston         | 2019-02-19 | 12:00 | 12:30 |
            | Diff Client, diff MedicalPractitioners, diff time | David     | Dr Watson             | 2019-02-19 | 12:30 | 13:00 |

    Scenario Outline: Attempt to Schedule a new appointment with a conflicting appointment
        Given an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30 for a "Basic checkup" so that "Medical application"
        And that appointment has been scheduled
        And an appointment for <Client> with <MedicalPractitionerId> on <Date> from <From> till <To> for a "Description" so that "Reason"
        When that appointment is scheduled
        Then a ConflictException must be thrown with the message "<ConflictMessage>"
        And the calender should contain 1 appointment only

        Examples:
            | Example Description                      | ClientId  | MedicalPractitionerId | Date       | From  | To    | ConflictMessage                                         |
            | Client Conflict                          | David     | Dr Livingston         | 2019-02-19 | 12:00 | 12:30 | Client already has an appointment for that timeslot     |
            | MedicalPractitioners Conflict            | Chantelle | Dr Watson             | 2019-02-19 | 12:00 | 12:30 | Practioner already has an appointment for that timeslot |
            | Client and MedicalPractitioners Conflict | David     | Dr Watson             | 2019-02-19 | 12:00 | 12:30 | Client already has an appointment for that timeslot     |
            | Client Overlap Conflict                  | David     | Dr Livingston         | 2019-02-19 | 12:15 | 12:45 | Client already has an appointment for that timeslot     |
            | MedicalPractitioners Overlap Conflict    | Chantelle | Dr Watson             | 2019-02-19 | 12:15 | 12:45 | Practioner already has an appointment for that timeslot |
Feature: Schedule New Appointment

    As a user
    I need to schedule a new appointment
    So that I can track those appointments

    # Background:
    #     Given the followings Clients
    #         | ClientId  |
    #         | David     |
    #         | Chantelle |
    #         | Layla     |
    #         | Arya      |

    #     Given the following Medical Practitioners
    #         | MedicalPractitionerId |
    #         | Dr Watson             |
    #         | Dr Livingston         |

    Scenario: Schedule a new appointment on an empty calendar
        Given an empty schedule
        And an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30 for a Basic checkup so that Medical application
        When that appointment is scheduled
        Then the schedule must contain 1 appointment/s only
        Then the schedule must contain an appointment for David with Dr Watson on 2019-02-19 from 12:00 till 12:30 for a Basic checkup so that Medical application

    Scenario Outline: Schedule a new appointment without conflicting appointments
        Given a schedule with the following appointments
            | ClientId | MedicalPractitionerId | Date       | From  | To    | Description   | Reason              |
            | David    | Dr Watson             | 2019-02-19 | 12:00 | 12:30 | Basic checkup | Medical application |
        And an appointment for <ClientId> with <MedicalPractitionerId> on <Date> from <From> till <To> for a Description so that Reason
        When that appointment is scheduled
        Then the schedule must contain an appointment for <ClientId> with <MedicalPractitionerId> on <Date> from <From> till <To> for a Description so that Reason

        Examples:
            | Example Description                               | ClientId  | MedicalPractitionerId | Date       | From  | To    |
            | Same Client, same MedicalPractitioners, diff time | David     | Dr Watson             | 2019-02-19 | 12:30 | 13:00 |
            | Same Client, diff MedicalPractitioners, diff time | David     | Dr Livingston         | 2019-02-19 | 13:00 | 13:30 |
            | Diff Client, same MedicalPractitioners, diff time | Chantelle | Dr Watson             | 2019-02-19 | 13:00 | 13:30 |
            | Diff Client, diff MedicalPractitioners, same time | Chantelle | Dr Livingston         | 2019-02-19 | 12:00 | 12:30 |
            | Diff Client, diff MedicalPractitioners, diff time | Chantelle | Dr Livingston         | 2019-02-19 | 13:30 | 14:00 |

    Scenario Outline: Attempt to Schedule a new appointment with a conflicting appointment
        Given a schedule with the following appointments
            | ClientId | MedicalPractitionerId | Date       | From  | To    | Description   | Reason              |
            | David    | Dr Watson             | 2019-02-19 | 12:00 | 12:30 | Basic checkup | Medical application |
        And an appointment for <ClientId> with <MedicalPractitionerId> on <Date> from <From> till <To> for a "Description" so that "Reason"
        When that appointment is scheduled
        Then the appointment must not be scheduled
        And a ConflictException must be thrown

        Examples:
            | Example Description                      | ClientId  | MedicalPractitionerId | Date       | From  | To    |
            | Client Conflict                          | David     | Dr Livingston         | 2019-02-19 | 12:00 | 12:30 |
            | MedicalPractitioners Conflict            | Chantelle | Dr Watson             | 2019-02-19 | 12:00 | 12:30 |
            | Client and MedicalPractitioners Conflict | David     | Dr Watson             | 2019-02-19 | 12:00 | 12:30 |

Feature: Find Next Available TimeSlot

    As a user
    I need to get the next available time slot
    So that I can schedule a new appointment

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
            | Arya      | Dr Livingston         | 2019-02-19 | 13:00    | 13:30  |            |        |
            | Chantelle | Dr Livingston         | 2019-02-19 | 14:00    | 14:30  |            |        |
            | David     | Dr Watson             | 2019-02-19 | 12:30    | 13:00  |            |        |
            | Layla     | Dr Watson             | 2019-02-19 | 13:00    | 13:30  |            |        |
            | Chantelle | Dr Watson             | 2019-02-19 | 13:30    | 14:00  |            |        |

    Scenario Outline: Retrieve Appointments
        When I find the next available time slot for <MedicalPractitionerId>
        Then the next available timeslot must be on <Date> from <From> till <To>

        Examples:
            | MedicalPractitionerId | Date       | From  | To    |
            | Dr Livingston         | 2019-02-19 | 12:30 | 13:00 |
            | Dr Watson             | 2019-02-19 | 14:00 | 14:30 |

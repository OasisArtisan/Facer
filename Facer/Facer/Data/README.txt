This Data package implements a structure that loads all of the data in memory and synchronizes all changes to underlying data file internally.
The strong encapsulation implemented makes sure that all of the data in RAM and in storage are synchronized.
Each object type has a data storage static field that must be set in order for any changes to be automatically synchronized.

Current limitations / problems:
- Each student must have a unique ID
- Deleting a student from the enrolled students dictionary does not remove its reference from attendance records.
  This can be solved easily by iterating over all records however the question is do we want to do that ?
- I should add more checks when adding removing students / records to enforce that only one student object is created for each student.
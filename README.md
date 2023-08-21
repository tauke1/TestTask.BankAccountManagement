# TestTask.BankAccountManagement
Test task about back account management system

after running migrations you can login by test manager with login = 'test' and password = 'qwerty', registration feature not implemented

BAL - business access layer
DAL - Data access layer
TestTask.BankAccountManagement project is a API project

NOTES:
1) No unit tests, but most of code have good code "testability", even passed Expressions to repository layer can be easily verified by using Mock and Neleus.LambdaCompare nuget packages
2) No pessimistic or optimistic locks, I know how to implement it
3) Unfortunately no UI app, because Backend development consumed too much time, you can use swagger with creadentials described in note 1
4) TestTask.BandAccountManagement.Shared was created for adding Blazor app purposes, but I did not have too much remaining  time to do it

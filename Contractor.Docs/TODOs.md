Fixes:
- Remove IdForUpdate
- Relate for BankIdForUpdate to BankTestValues.BankIdDbDefault2
- Fix Lambda Logic.GetBanken
- 2 Relations between Bank and Kunde: Side Effects of generation
- Improve mat-tab-group height calculation 

Ideas: 
- contractor add relation n:m UserManagement.User:Users UserManagement.Group:Groups UserGroupMembership
- contractor add relation 1:1 (In Logic/API sinnvoll, in DB ist es trotzdem nur eine Tabelle. Da es sehr generisch ist und viel Aufwand bedeutet, kommt das später.
Fixes:
- InMemoryDbContext (Methoden umbenennnen)
  - CreatePersistenceDbContextEmpty
  - CreatePersistenceDbContextWithDbDefaults
- DateTime Fix
- 2 Relations between Bank and Kunde: Side Effects of generation

Ideas: 
- contractor add relation n:m UserManagement.User:Users UserManagement.Group:Groups UserGroupMembership
- contractor add relation 1:1 (In Logic/API sinnvoll, in DB ist es trotzdem nur eine Tabelle. Da es sehr generisch ist und viel Aufwand bedeutet, kommt das später.
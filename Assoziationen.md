Prio 1: contractor add relation 1:n Bankwesen.Bank:Banken Bankwesen.Kunde:Kunden
- Detail für beide Seiten
- Detail wird per Include im Repository gebaut
- Der Foreign-Key wird beim wird unter Verwendung des Foreign-Repositories auf Existenz geprüft 

Prio 2: contractor add relation n:m UserManagement.User:Users UserManagement.Group:Groups UserGroupMembership

Prio 3: add relation 1:1 (In Logic/API sinnvoll, in DB ist es trotzdem nur eine Tabelle. Da es sehr generisch ist und viel Aufwand bedeutet, kommt das später.)

Side Quests:
- Add default folder structure for entities 
- Solve TODOs
- Using-Sorting
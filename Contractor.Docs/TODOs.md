Main Quests:
- Frontend Generation
  - Create-Page: Change Name of Main DTO to createEntity
  - Sub-Pages
  - Entities-Page (To): Generate Column for From-Entity
- contractor add relation 1:n 
  - [-n Vertragsbank:Vertragskunden] Alternative Property Names
  - Endpoint: GetTosByFromId

Side Quests:
- Solve TODOs
- ..CrudLogic / ..CrudRepository (Replace with "([^Crud])Logic")
- Improve Project Structure of Contractor.Core

Ideas: 
- contractor add relation n:m UserManagement.User:Users UserManagement.Group:Groups UserGroupMembership
- contractor add relation 1:1 (In Logic/API sinnvoll, in DB ist es trotzdem nur eine Tabelle. Da es sehr generisch ist und viel Aufwand bedeutet, kommt das später.
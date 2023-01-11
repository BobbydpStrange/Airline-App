# Back-End Project Requirements
- [x] OIDC authentication with 3rd party identity provider
    - roles/claims for different levels of access
- [x] Choose between the following technologies for the user interface
  - Razor Pages
  - Blazor Server
  - Blazor WebAssembly
- [x] Regardless of which technology you select, your data access and business logic must be run in an API (which is also secured using the same OIDC provider)
- [x] Have unit tests for all logic that's more complicated than basic CRUD operations
- [ ] Have system level tests for at least three different primary workflows in your app (e.g. new user signup, or scheduling a shift, or preparing a report)
- [x] Propose a robust (but narrow) subset of features specific to your selected problem domain (hotel app, flight app, etc.) This list of features will be presented to your instructor for approval and you will then be responsible for the implementation of those by the end of the semester.
- [ ] You will validate the design of your UI by conducting at least two rounds of user interviews (user tests) with users from your target market (after your project proposal)
- [X] Run your database, api, and front end on Azure services
- [x] Properly secure your postgres database (application runs as separate user, different than the db owner)
- [x] Log system behavior, user interactions (use the error, warning, information, and debug levels appropriately)
- [x] CI/CD pipeline to run automated tests and deploy app updates to azure
- [x] Integrate with some type of external notification system (sending live emails, SMS messages, etc.)


# Proposed Features
- [x] Admin:
    - Maintenance Policy Configuration Page
        - Set routine maintenance policy per # of flights that sends planes to needed maintenance backlog for scheduling
- [x] Maintenance Scheduler:
    - Views Routine Maintenance Backlog
    - Schedules and Assigns Planes for maintenance
        - Can view available (and certified) mechanics / open hangers to help schedule
- [x] Mechanic:
    - Mechanic Schedule Report
        - Mechanics can see their current assignments
        - Email notifications when there are assignment updates
- Depending on time, a similar system for flights/pilot scheduling

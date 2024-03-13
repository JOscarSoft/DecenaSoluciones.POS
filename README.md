# DecenaSoluciones.POS

Point of sales (POS) and customer tracking system. To see production beta click [here](https://decenasoluciones.com/)

## Overview
This solution constain:

- **DecenaSoluciones.POS.API:** Backend - Rest Api.
- **DecenaSoluciones.POS.App:** Mobile app, on progess.
- **DecenaSoluciones.POS.WebApp:** Web application, on beta.
- **DecenaSoluciones.POS.Shared:** Shared models and services.

## Getting started

### Prerequisites
- .Net 7.0
- .Net 8.0 (For mobile app)
- Microsoft Sql Server 2017 (or higher)
- Android SDK

### Run
1. Restore nugget packages
2. Open Package Console Manager and run
```
dotnet workload restore
```
3. Create your local data base and run .net migrations to update/create the database
4. Start the API prject
5. Start the Mobile or Web app as desire
6. Migration won't create any user, to create the first one do the following:
   1. Go to AuthenticationController.cs file
   2. Comment the "Authorize" anotation for the route "newuser"
   3. Run the API and call the route to register the user with the desire information
      - As recomendation the first username should be "SUPERADMIN" which constain security checks
   4. Uncomment the "Authorize" anotation.




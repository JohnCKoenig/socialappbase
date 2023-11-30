
# Social App Base
This project is designed to be a base for a mobile social application. It will soon provide all the necessary functionality a basic social application might. It can then be adapted for a specific use case. 




## Technologies Used
Name  | Use
------------- | -------------
ASP.NET  | Used to provide a base API for functionality such as authentication, account creation, etc.
SignalR  | Provides real-time socket based connections for chat and comment loading functionality.
MAUI.NET | Used to build a cross-platform client (IOS/Android) for users to access the service.
MS-SQL | Provides a database to store more static content, such as user accounts and posts
Redis | Allows for immediate token revocation/blacklisting in the otherwise stateless JWT key system. 
ScyllaDB | Allows for highly avaliable and high-throughput storage for the messaging service


## Functionality Status
### API 
* Authentication - Complete
* Authorization - Complete
* Token Revocation - Complete
* Account Creation - Complete
* Account Info Update - Complete
* Refresh Tokens - Complete
* Content Creation - WIP
* Group Creation - WIP
* Image Content - Planned
* User Profile Photos - Planned
* Friend list - Planned
* And more to come once current plans are complete.

### Real-Time Services
* Basic direct-messaging - Nearing completion
* Group messaging - WIP
* Message persistence - Planned
* User listing - Pending friend-list functionalityu
* Commenting - Planned

## X-Platform Application
* Basic sign-in - Complete
* Token persistence - Complete
* Tabular Page Design - Complete
* Icons - Complete
* API Routes - WIP
* Registration - WIP
* Post-loading - WIP 
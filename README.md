##### Summary:
The Transactions API privdes a REST interface to carry out standard CRUD operations on an assumed Transactions store.
Uri endpoint to the API is as follows: http://hostaddress/api/transactions
It is configured to accepts GET, POST, PUT, and DELETE http verbs. 
It is also configured to take an optional id parameter to identify a resource.
Accept request header switches between json and xml response result formats.

##### Tools and Techniques
- A Url is returned with transaction model to clients. This is a small step towards hypermedia. 
- Ninject is plugged into the api configuration to enable IoC. 
- RhinoMocks is used to mock out the repository from unit tests.

##### How to run
- Load solution in Visual Studio 2013 or above and hit Run. 
- Help page is provided using Microsoft.AspNet.WebApi and gets generated automatically as the api evolves.
http://hostaddress/api/help
- A fiddler archive is submitted to ~/App_Data/TransactionsApiCalls.saz.
In order to replay, right click session then Replay > Reissue from composer, then update the host address to that of where the api service is hosted. 

##### System requirements:
	.Net fx 4.5
	IISExpress, IIS, or any webserver that can host AspNet
	Visual Studio 2013 or above

##### Dependencies (summarized)
- Microsoft.AspNet.Mvc 
- Microsoft.AspNet.WebApi
- Newtonsoft.Json
- Ninject
- NUnit - for test project only
- RhinoMocks - for test project only
- Microsoft.AspNet.WebApi.Owin - for test project only to support self hosting web api

##### Other notes
1. Spent 1.5hrs to do the core of tests and implementation. Refactoring, restructuring, adding help files, documentation, and submitting is excluded from this. 
2. What could have been done given more time:
- Persistance store. Although requested in the original task, I did not get enough time to finish the persistance. Started adding the persistence store using SQLite. Would need more time to finish. 
- Hypermedia implementation: Based on RMM the API is not considered RESTful without hypermedia. e.g. Could return links to how to delete/update a transaction with every transaction returned from the API.
- Etags for caching.
- Paging of list results, i.e. GET ~/api/transations/?page=123
- Logging.
- More tests e.g. integration tests on persistance store, or unit tests on specific request headers e.g. Accept: text/xml, application/json


Regards, 

Masoud Azizpour
	
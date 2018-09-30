# DeliveryService
REST API to manage delivery routes between two graph nodes.
It returns the best route (less time, less cost, less hops) between two nodes.
In addition it is also possible to perform CRUD operations on Nodes (Warehouses) and Relationships (SHIPS_TO).
Future developments can also benefit of a generic repository for CRUD operations.
Solution architecture split into three layers: API -> DL (Domain Logic) -> DAL
Unit testing in DL layer. 

## Prerequisites:
- Neo4j Database
- .NET CORE 2.1
- Visual Studio 2017 
- Packages were managed with Nuget. Some of them are in the list below:
```
  Install-Package Neo4j.Driver
  Install-Package AutoMapper
  Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
  Install-Package Microsoft.AspNetCore.TestHost
  Install-Package Moq
```
## Before start:
- Neo4j Database needs to be Running
- Update the appsettings.json file with your Neo4j database credentials. Below is an example
```
"ConnectionStrings": {
    "Main": {
      "URI": "http://localhost:7474/db/data",
      "User": "neo4j",
      "Pass": "teste"
    }
  },
  ```
 - For this exercise it was requested that the route must not perform a direct delivery between the origin and the destiny. However feel free to update the follow section:
  ```
  "RouteSetup": {
    "MinHops": 2,
    "MaxHops": 20
  },
  ```
## API overview
The API is generally RESTFUL and returns results in JSON. JSON formats returned by the API are documented here.
The API supports HTTP and HTTPS. Examples here are provided using HTTPS.
Two resources were implemented: Warehouse and Routes

### Resource components and identifiers 
Resource components can be used in conjunction with identifiers to retrieve the metadata for that identifier.

| Resource                  | Description                                         |
| ------------------------  | --------------------------------------------------- |
| /warehouses/{id}          | Returns metadata for the specified Warehouse Id     |
| /warehouses/name/{name}    | Returns metadata for the specified Warehouse Name   |
| /routes/{name}             | Returns all routes which has Warehouse Name as origin node |
| /warehouses/to/{name}    | Returns all routes which has Warehouse Name as destiny node   |
| /warehouses/{origin}/to/{destiny}    | Returns all routes between the origin and destiny nodes   |
| /warehouses/{origin}/to/{destiny}?limit=5&orderbyparams=Hops&orderBydescending=true    | Returns all routes between the origin and destiny nodes filter by options passes as query params |
| /warehouses/{origin}/to/{destiny}/{option}    | Returns the {option} route between the origin and destiny nodes. For instance {option}=best will return the best route, with less time then by less cost and finally by less hops   |

#### Routes
A Route is a path between two nodes.
The return metadata has the follow fields:
```
        public Warehouse Origin { get; set; } 
        public Warehouse Destiny { get; set; }
        public IEnumerable<Warehouse> RoutePoints { get; set; }
        public IEnumerable<SHIPS_TO> ShipDetails { get; set; }
        public int TotalCost { get; set; }
        public int TotalTime { get; set; }
        public int Hops { get; set; }
 ```
 Any route can be filtered by the following RouteOptions
 ```
        public int Limit { get;  }
        public string[] OrderByParams { get;  } // "TotalCost","TotalTime","Hops"
        public OrderByType OrderBy { get; }
  ```
 These options can also be handled automaticaly using the URI /warehouses/{origin}/to/{destiny}/{option} where {option} must be one of the values below:
 ```
        public const string BestPath = "best";
        public const string QuickestPath = "quick";
        public const string SlownessPath = "slow";
        public const string CheapestPath = "cheap";
        public const string CostliesPath = "expensive";
        public const string ShortestPath = "short";
 ```
- - - -
### To DO
- Please check the projects tab for further details



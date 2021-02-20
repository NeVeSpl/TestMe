# Sample Modular Monolith application without fluff and hype but with tests

Yet another sample .net core application. But this one aims to be a little bit different than the rest. Instead of focusing on showing some fancy libraries and patterns on non-realistic simplified examples, this one focus on delivering a fully working application with high quality. 

1. [How to run](#HowToRun)
2. [Overview](#Overview)
3. [Architecture decision record](#ADR)
4. [Layer : Presentation.React](#React)
5. [Layer : Presentation.API](#API)
6. [Module comparison](#ModuleComparison)
7. [Communication between modules](#CommunicationBetweenModules)
8. [Module : UserManagement](#UserManagement)
9. [Module : TestCreation](#TestCreation)
10. [Module : TestConducting](#TestConducting)
11. [Module : TestResults](#TestResults)
12. [Working demo](#DEMO)
13. [ToDo](#TODO)

## <a name="Overview"></a> 1. How to run

- external dependencies (postgres, rabbitmq, elasticsearch, kibana)
``docker-compose up``
- backend
``green arrow in VS``
- frontend
``cd TestMe.Presentation.React/ClientApp/ && npm start``

| component | run | endpoint
|---- | ----| --- 
| .net core | from VS | https://localhost:44357
| swagger   | from VS |  https://localhost:44357/swagger
| postgres      | docker-compose up | n/a
| rabbitmq      | docker-compose up | http://localhost:15672/ 
| elasticsearch | docker-compose up | n/a
| kibana        | docker-compose up | http://localhost:5601 
| influxdb | n/a | n/a 
| react | npm start | http://localhost:3000/
| jest | npm test | n/a
| storybook | npm run storybook | http://localhost:9009

## <a name="Overview"></a> 2. Overview

- top architecture: modular monolith aka vertical slice architecture
- backend: asp.net core, entity framework core, postgresql
- tests: mstest v2, sqllite in-memory, detroit school of testing 
- frontend: react + typescript
- communication between modules : RabbitMQ or in-memory bus
- enabled non-null reference types 
- every module in separate transaction scope, eventual consistency between modules
- mapping between c# dtos is done by [MappingGenerator](https://github.com/cezarypiatek/MappingGenerator)
- typed api client for api calls is auto-generated with [NTypewriter](https://github.com/NeVeSpl/NTypewriter)


![projects_dependencies](docs/TestMe.Architecture.png)



## <a name="ADR"></a> 3. Architecture decision record

TBD


## <a name="React"></a> 4. Layer : Presentation.React
- with restrictive Content Security Policy set (no inline css or js, no eval)
- CSS Modules (scoping css per component)
- every page implemented in two ways: 
    - class-components with local state 
    - functionals-components with redux and hooks
- component state persisted in localstorage
- props dependency injection used to be able to display stateful components in [Storybook](https://github.com/storybookjs/storybook)
- files grouped by features/routes and not by file type 
- TypeScript all the way 

![projects_dependencies](docs/Presentation.React.png)

## <a name="API"></a> 5. Layer : Presentation.API
- integration tests for happy paths backed on sqllite in-memory with the possibility to switch to postgresql if needed for debuging

#### Endpoints

| Endpoint                  |               |
| ------------------------- | ------------- |
| /QuestionsCatalogs/       |   |
| /Questions/               | Endpoint that allows editing  whole Question aggregate (Question + Answer) as a single resource, with optimistic concurrency control |
| /TestsCatalogs/           |   |
| /Tests/                   | Endpoint that allows editing only aggregate root from Test aggregate (Test + QuestionItem)  |
| /Tests/{testId}/questions/| Endpoint that allows editing TestItem entity from Test aggregate (Test + QuestionItem) as a sub-resource |
| /Tokens/                  |   |
| /Metrics/lineprotocol/    | A special endpoint available only from localhost that returns following metrics : <br/>- CPU usage,<br/>- RAM usage,<br/>- no. GC collections,<br/>- GC heaps sizes,<br/>- GC pause time,<br/>- GC background time,<br/>- ThreadpoolThreadCount,<br/>- ThreadpoolQueueLength,<br/>- ExceptionCount,<br/>- MonitorLockContentionCount<br/> in a format that can be directly pulled by Telegraf to InfluxDB |
| /Users/                   |

#### Load testing results
- server : vps from OVH, 2 GB RAM, 1 CPU Haswell 2,4GHz, Ubuntu 18.04.3, Apache/2.4.29 as reverse proxy 

##### POST /Tokens 
A constant client count:            | 400  | 500  |  600 | 700  | 800
----------------------------------- | ---- | ---  | ---- | ---- | -----
Sync - Response Times Average (ms)  | 545  | 703  | 850  | 1021 | 1860
Sync - Req/s                        | 728  | 703  | 697  | 675  | 418
Async - Response Times Average (ms) | 651  | 802  | 947  | 1096 | 2144
Async - Req/s                       | 609  | 618  | 626  | 632  | 359

##### GET Questions/headers?catalogId=1
A constant client count:            | 400  | 500  |  600 | 700  | 800
----------------------------------- | ---- | ---  | ---- | ---- | -----
Sync - Response Times Average (ms)  | 857  | 1171 | 1375 | 1640 | 2590
Sync - Req/s                        | 455  | 423  | 428  |  421 |  300
Async - Response Times Average (ms) | 967  | 1204 | 1364 | 1633 | 2574
Async - Req/s                       | 408  | 410  | 433  |  422 |  301

##### GET Questions/5
A constant client count:            | 400  | 500  |  600 | 700  | 800
----------------------------------- | ---- | ---  | ---- | ---- | -----
Sync - Response Times Average (ms)  | 1167 | 1536 | 1802 | 2071 | 2962
Sync - Req/s                        | 339  | 321  | 331  | 328  | 261
Async - Response Times Average (ms) |   |  |  |  | 
Async - Req/s                       |   |  |  |  | 



## <a name="ModuleComparison"></a> 6. Module comparison

Description                 | UserManagement                      | TestCreation
----------------------------|-------------------------------------|--------------
architecture                | layers + transaction script         | clean architecture + minimal CQRS
domain model                | anemic + a few value objects        | rich 
data access layer           | Entity Framework                    | Repository + unit of work for writes / Entity Framework for reads
exceptional situations      | DomainException                     | Result + DomainException
pagination                  | cursor based                        | offset based

## <a name="CommunicationBetweenModules"></a> 7. Communication between modules
- reliable communication between modules without using a distributed transaction
- two available implementations, RabbitMQ based for production use and in-memory for tests  

![projects_dependencies](docs/CommunicationBetweenModules.png)

## <a name="UserManagement"></a> 8. Module : UserManagement
- domain model: anemic
- architecture : layers + transaction script
- data driven unit tests and architectural tests
- outbox pattern for publishing integration events in a reliable way without using two phase commit - at least one delivery


#### Architecture
![projects_dependencies](docs/UserManagement.png)




## <a name="TestCreation"></a> 9. Module : TestCreation
- domain model: rich
- architecture : clean architecture + minimal CQRS (with the same data store)
- data driven unit tests and architectural tests
- soft delete for all entities
- optimistic concurrency and conflic resloving for Question aggregate
- inbox pattern for receiving integration events in a reliable way without using two phase commit - deduplication of events

#### Architecture
![projects_dependencies](docs/TestCreation.png)
#### Domain
![projects_dependencies](docs/TestCreation.Domain.png)




## <a name="TestConducting"></a> 10. Module : TestConducting
not available yet




## <a name="TestResults"></a> 11. Module : TestResults
not available yet





## <a name="DEMO"></a> 12. Working demo
![projects_dependencies](docs/TestMe.ResolveOptimisticConcurrencyConflict.gif)




## <a name="TODO"></a> 13. ToDo
- setup development environment in docker (postgresql, TICK stack, RabbitMQ, Elastic stack)
- introduce Architecture decision record (ADR)
- frontend : automated tests for frontend (maybe Cypress?)
- frontend : use immerjs to create next immutable state instead of home made solution
- backend : finish /Tests endpoints
- backend : deal with poisonous integration events (dead letter queue)
- backend : add batch publishing of events on RabbitMQ
- backend : improve in-memory bus implementation 



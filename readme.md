# Sample Modular Monolith application without fluff/hype(*) but with tests
- top architecture: modular monolith aka vertical slice architecture
- backend: asp.net core, entity framework core, postgresql
- tests: mstest v2, sqllite in memory, detroit school of testing
- frontend: react + typescript

![projects_dependencies](docs/TestMe.Architecture.png)

## Module : UserManagement
- domain model: anemic
- architecture : layers + transaction script

#### Module architecture

![projects_dependencies](docs/TestMe.UserManagement.png)

## Module : TestCreation
- domain model: rich
- architecture : clean architecture + minimal CQRS

#### Module architecture

![projects_dependencies](docs/TestMe.TestCreation.png)

#### Domain

![projects_dependencies](docs/TestMe.TestCreation.Domain.png)

## Module : TestConducting
n/a

## Module : TestResults
n/a

## Working demo
n/a


(*) microservices, REST, Redux, AutoMapper, MediatR, FluentValidation, Autofac

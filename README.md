# ToolPack Exceptions

Solution to operate exceptions in .NET services, both by providing exception/problem models, guard methods and exception handlers for REST and gRPC APIs.

This resorts on two projects: `ToolPack.Exceptions.Base` and `ToolPack.Exceptions.Web`.

This aims to be a framework that fully supports the error handling in .NET services. The following main components are exposed:
* [`ToolPack.Exceptions.Base`](#ToolPack.Exceptions.Base):
   * [Exception entities](#exception-entities): Exception entities that may be referenced across the code;
   * [Exception guards](#exception-guards): Validation checkers to evaluate conditions and throw related exceptions.
* [`ToolPack.Exceptions.Web`](#ToolPack.Exceptions.Web):
   * [Dependency Injection extensions](#dependency-injection-extensions): Extension methods that add the required ToolPack Exception services to applications, including *global exception handlers both for REST and gRPC services*;
   * [Error models](#error-models): Error-related models that enrich or complete the exceptions handling, exposing `ProblemDetails`.

Other contents:
* [How to use](#how-to-use)
* [Exceptions and matching Status Codes](#exceptions-and-matching-status-codes)


# `ToolPack.Exceptions.Base`

## **Exception entities**

* Predefined custom exception entities that may be used anywhere in the code. These are also used by the guards, services and handlers.
* Namespace: `ToolPack.Exceptions.Base.Entities`

### Entities list:

* `CustomBaseException`
	* Custom exception that is parent of all the exceptions listed here
	* When an exception is a `CustomBaseException`, it is already known that such exception comes from an owned system

* `AlreadyExistsException`
	* To occur when an entity is already present in the system
	* _Ex.: When a primary key value is already in use_

* `ExternalComponentException`
	* To occur when an external component in which the system depends failed to operate correctly/answer successfully
	* _Exs.: When a partner's API failed to respond; When a third-party package broke its operation; When a message was not received_

* `NotFoundException`
	* To occur when an entity that was looked for does not exist
	* _Ex.: When an entity identified with a given value/key does not exist_

* `ValidationFailedException`
	* To occur when inputs are not correctly provided to the system
	* _Ex.: When a consumer uses an API with wrong parameters_


## **`Exception guards`**

* Guard methods that validate typical conditions and throw related exceptions when these are not met.
* Namespace: `ToolPack.Exceptions.Base.Guard`


# `ToolPack.Exceptions.Web`

## **`Dependency Injection extensions`**

* Extension methods that inject the ToolPack Exception services/dependencies to the given application.
* Namespace: `ToolPack.Exceptions.Web.Extensions`

### Methods list:

* `AddToolPackExceptions(this IServiceCollection services)`
	* Adds the ToolPack Exceptions framework services that treat exceptions. It does not include gRPC interceptors nor ASP.NET middleware (for such, see the other extension methods).

* `AddToolPackExceptionsGrpc(this IServiceCollection services)`
	* Adds all the ToolPack Exceptions framework required for gRPC services, to handle exceptions globally in gRPC calls. If handling exceptions in REST endpoints is required, proper middleware must be used (see extension `UseToolPackExceptionsMiddleware`).

* `AddToolPackExceptionsGrpcInterceptor(this IServiceCollection services)`
	* Adds the ToolPack Exceptions interceptor services, to handle exceptions globally in gRPC calls. It must not be used in REST APIs; in such cases, exceptions must be handled with proper middleware (see extension `UseToolPackExceptionsMiddleware`). It does not add the ToolPack Exceptions framework services that treat exceptions; for such, use `AddToolPackExceptionsGrpc` or `AddToolPackExceptions`.

* `UseToolPackExceptionsMiddleware(this IApplicationBuilder appBuilder)`
	* Uses the ToolPack Exceptions middleware, to handle exceptions globally in the request pipeline. It must not be used in gRPC APIs; in such cases, exceptions must be handled with interceptors (see extensions `AddToolPackExceptionsGrpc` and `AddToolPackExceptionsGrpcInterceptor`). It does not add the ToolPack Exceptions framework services that treat exceptions, so the extension `AddToolPackExceptions` must also be used.


## **Error models**

* Custom models that relate with errors information and allow the correct handling of exceptions.
* Namespace: `ToolPack.Exceptions.Web.Models`

### Models list:

* `ProblemDetails`
	* Custom model that implements the [`ProblemDetails` RFC for HTTP APIs' error responses](https://datatracker.ietf.org/doc/html/rfc7807)
	* It extends the model `Microsoft.AspNetCore.Mvc.ProblemDetails`


## **How to use**

1. In your `Startup` class or similar, add the _ToolPack Exception services_ as below. Use [`Dependency Injection extension methods`](#dependency-injection-extensions) according to your case.

```csharp
public void ConfigureServices(IServiceCollection services)
{
	// If developing a non-gRPC Web API
    services.AddToolPackExceptions();
    
	// If developing a gRPC API, use one of the following to register a Global Exception Interceptor:
	
	// 1. You may replace the call to "AddToolPackExceptions()" by:
	// services.AddToolPackExceptionsGrpc();

	// 2. Alternatively, you may call "AddToolPackExceptions()" and add:
	// services.AddToolPackExceptionsGrpcInterceptor()
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // If developing a non-gRPC Web API, use this to register a Global Exception Middleware
	app.UseToolPackExceptionsMiddleware();
}
```

2. This setup will add middleware handlers to catch exceptions thrown in the application and produce responses based on the `ProblemDetails` model [above](#error-models). These handlers also map the thrown exceptions with HTTP and RPC status codes, as shown [below](#exceptions-and-matching-status-codes).

3. Besides the dependency injection services, all the entities, models and utils described above can be used indiscriminately across the code.


## **Exceptions and matching Status Codes**

| Exception        | Exception Package | RPC Status Code  | HTTP Status Code |
| ---------------- | ----------------- | ---------------- | ---------------- |
| `AlreadyExistsException` | `ToolPack Exceptions` | 6 AlreadyExists | 409 Conflict |
| `ArgumentException` | `System` | 3 InvalidArgument | 400 BadRequest |
| `ArgumentOutOfRangeException` | `System` | 11 OutOfRange | 400 BadRequest |
| `AuthenticationException` | `System.Security.Authentication` | 16 Unauthenticated | 401 Unauthorized |
| `CustomBaseException` | `ToolPack Exceptions` | 13 Internal | 500 InternalServerError |
| `HttpRequestException` | `System.Net.Http` | 14 Unavailable | 503 ServiceUnavailable |
| `NotFoundException` | `ToolPack Exceptions` | 5 NotFound | 404 NotFound |
| `NotImplementedException` | `System` | 12 Unimplemented | 501 NotImplemented |
| `TaskCanceledException` | `System.Threading.Tasks` | 1 Cancelled | 400 BadRequest |
| `TimeoutException` | `System` | 4 DeadlineExceeded | 504 GatewayTimeout |
| `UnauthorizedAccessException` | `System` | 7 PermissionDenied | 403 Forbidden |
| `ValidationFailedException` | `ToolPack Exceptions` | 9 FailedPrecondition | 400 BadRequest |


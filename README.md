# tweetbook-api

The Tweetbook API is an example ASP.NET Core Web API project implemented with best practices for real world use. Within the project code are comments explaining the rationale behind key design and project structure considerations. Topics covered are listed below and will be continuously updated as new content is added to the project.

## Topics Covered

### Auto-generated API documentation

The API documentation for this project is generated automatically. As APIs get larger, it becomes harder to maintain synchronization between documentation and program behavior; automation goes a long way to fix this issue so that API consumers always have the latest (_accurate!_) information available. 

The Swashbuckle package has been used to auto-generate API documentation that obeys the Swagger specification. For example setup, see the [Startup](https://github.com/Alvin-Leung/tweetbook-api/blob/master/Tweetbook/Startup.cs) and [MvcInstaller](https://github.com/Alvin-Leung/tweetbook-api/blob/master/Tweetbook/Installers/MvcInstaller.cs) classes.

### Clean Installation

While installation of services usually occurs in the [Startup](https://github.com/Alvin-Leung/tweetbook-api/blob/master/Tweetbook/Startup.cs) entrypoint, that does not mean that all the installation logic needs to go in this class. This project separates installers into separate classes based on responsibility, and uses reflection to access these installers via a common interface. See [InstallerExtensions](https://github.com/Alvin-Leung/tweetbook-api/blob/master/Tweetbook/Installers/InstallerExtensions.cs) for the implementation.

### Dependency Injection of Services

ASP.NET Core has dependency injection baked right into the framework, and we make good use of it in this project. Services are defined with a particular scope at startup, and injected into classes on an as-needed basis. This has immense benefits including, but not limited to, the following:

- Greater decoupling of classes; better separation of responsibilities
- Improved testability of code
- Increased code re-useabilty

## Upcoming Topics

- Versioned Endpoints, Requests, and Responses
- Separation of Domain and Contract
- Authentication via JSON Web Tokens
- Easy Integration Testing

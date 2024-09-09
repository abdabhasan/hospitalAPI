# Hospital API

This project is a RESTful API for managing hospital data, built using .NET Core C# and PostgreSQL. The API follows a 3-tier architecture pattern and uses JWT for authentication. PL/pgSQL is used to create PostgreSQL functions to handle data operations, and CRUD endpoints are provided for managing various entities in the hospital database.

## Features

- CRUD operations.
- JWT-based authentication and authorization.
- Serilog integration for advanced logging.
- xUnit for unit testing.
- Swagger for API documentation.
- Direct database connection (no Entity Framework).

## Architecture

The project follows a **3-tier architecture** with the following layers:

1. **Presentation Layer**: This layer is responsible for handling incoming HTTP requests and sending responses. It includes the controllers that define the API endpoints. All incoming requests pass through this layer, where they are validated and authenticated (using JWT). Once validated, the requests are forwarded to the business logic layer.
2. **Business Logic Layer (BLL)**:This layer processes requests from the presentation layer and applies business rules before interacting with the data layer. It ensures that the right operations are performed on the data.
3. **Data Access Layer (DAL)**: The data layer is responsible for directly interacting with the PostgreSQL database. Unlike typical .NET applications that use Entity Framework, this project connects to the database directly using raw SQL queries and stored procedures written in PL/pgSQL. The DAL handles the execution of CRUD operations and any complex queries via PostgreSQL functions.

### Key Aspects:

- **Separation of Concerns**: Each layer has its dedicated role, making the system easier to maintain and extend.
- **Direct Database Access**: Bypassing Entity Framework allows for more control over database queries and performance optimization through custom SQL and PL/pgSQL functions.
- **Testing**: The layered architecture makes unit testing and integration testing easier by isolating each component's responsibilities.

## Technology Stack

- Backend Framework: .NET Core C#
- Database: PostgreSQL
- Stored Procedures: PL/pgSQL
- Authentication: JWT (JSON Web Tokens)
- Logging: Serilog
- Unit Testing: xUnit
- API Documentation: Swagger (OpenAPI)

## Database Structure

The database consists of the following tables:

1. **Users**: Contains user credentials and authentication details.
2. **Persons**: Stores information about individuals in the hospital (both patients and staff).
3. **Patients**: Records patient-specific information.
4. **Doctors**: Contains information about doctors, their specialties, and availability.
5. **Staff**: Non-medical staff data, such as administrative or maintenance personnel.
6. **Visitors**: Tracks visitor information.
7. **Shifts**: Defines working hours for staff, including doctors and nurses.
8. **Insurance Claims**: Manages insurance claims made by patients.

## PostgreSQL Functions

To handle complex data logic and operations, PL/pgSQL functions were created in the PostgreSQL database. These functions provide optimized data management and can be invoked from the API's data layer.

## API Endpoints

### Authentication

| HTTP Method | Endpoint          | Description                        |
| :---------- | :---------------- | :--------------------------------- |
| POST        | `/api/auth/login` | Authenticate and receive JWT token |

### Users

| HTTP Method | Endpoint          | Description       |
| :---------- | :---------------- | :---------------- |
| GET         | `/api/users`      | Get all users     |
| GET         | `/api/users/{id}` | Get user by ID    |
| POST        | `/api/users`      | Create a new user |
| PUT         | `/api/users/{id}` | Update user info  |
| DELETE      | `/api/users/{id}` | Delete a user     |

### Patients

| HTTP Method | Endpoint             | Description          |
| :---------- | :------------------- | :------------------- |
| GET         | `/api/patients`      | Get all patients     |
| GET         | `/api/patients/{id}` | Get patient by ID    |
| POST        | `/api/patients`      | Create a new patient |
| PUT         | `/api/patients/{id}` | Update patient info  |
| DELETE      | `/api/patients/{id}` | Delete a patient     |

### Additional Endpoints

Endpoints are available for all other entities (Doctors, Staff, Visitors, Shifts, Insurance Claims) and follow a similar CRUD structure.

## Logging

The application uses **Serilog** for structured logging, allowing for detailed insight into API operations and errors.

## Unit Testing

**xUnit** is used for writing and running unit tests. Test cases have been written to cover the CRUD operations for all entities. Unit tests ensure the API functions as expected, with test coverage for both success and failure scenarios.

### Running Tests

    dotnet test

## Authentication

JWT (JSON Web Token) is used for securing the API. Users must authenticate via the `/api/auth/login` endpoint to receive a token. This token must then be included in the Authorization header of subsequent requests.

### Example JWT

    Authorization: Bearer your-jwt-token

## Accessing Swagger

The API should be running on https://localhost:{port}.

    https://localhost:{port}/swagger

## Testing

Run unit tests using the following command:

    dotnet test

## License

This project is licensed under the [MIT License](https://opensource.org/license/mit).

## Conclusion

This API is designed to efficiently manage hospital operations and provide secure data management with structured logging, robust testing, and authentication mechanisms. Feel free to contribute to the project or raise issues for any feature enhancements.

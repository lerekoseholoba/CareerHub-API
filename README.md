# CareerHub API 

## Overview

The **CareerHub API** is a foundational ASP.NET Core Web API built using .NET 10.
It serves as the backend engine for a future job board platform called CareerHub, which will eventually integrate with a React/Next.js frontend.

This version of the API provides basic **read-only job listing endpoints** using in-memory data to simulate a real-world backend service.

---

## API Architecture Choice

### Controllers vs Minimal APIs

This project uses **Controllers-based architecture** instead of Minimal APIs.

### Reason for Choosing Controllers

- **Scalability**: Suitable for large applications with multiple modules and endpoints.
- **Separation of Concerns**: Routing, business logic, and request handling are clearly separated.
- **Maintainability**: Easier to extend with future features like authentication, job applications, and user management.
- **Industry Standard**: Commonly used in enterprise ASP.NET Core applications.
- **Testability**: Works well with dependency injection and unit testing patterns.

While Minimal APIs are lightweight and useful for microservices or quick prototypes, Controllers were selected because CareerHub is intended to evolve into a full-scale production system.

---

## Project Structure

```
CareerHub_API/
│
├── Controllers/
│   └── JobsController.cs       # Handles HTTP requests for job endpoints
│
├── Models/
│   └── JobListing.cs           # Domain model for job postings
│
├── Data/
│   └── JobService.cs           # In-memory data store and business logic
│
├── Program.cs                  # Application configuration and startup
├── appsettings.json
└── CareerHub_API.csproj
```

---

## Domain Model

### JobListing

The API uses a simple job listing model with the following properties:

| Property | Type |
|---|---|
| `Id` | int |
| `Title` | string |
| `Description` | string |
| `Company` | string |
| `Location` | string |
| `Type` | string |

---

## API Endpoints

### 1. Get All Jobs

**Description:**
Returns all available job listings.

**Response:**
- `200 OK` → Returns a list of job listings

---

### 2. Get Job by ID

**Description:**
Returns a specific job matching the provided ID.

**Responses:**
- `200 OK` → Job found and returned
- `404 Not Found` → No job exists with the given ID

---

## Data Source

This API uses an **in-memory data store** implemented in `JobService`.

- No database integration is used
- Contains 3 sample job listings
- Designed for learning and early API development

---

## Async Implementation

All endpoints are implemented using **async/await patterns**:

- `Task<IActionResult>` is used for all endpoints
- `Task.FromResult()` is used for in-memory operations
- Ensures non-blocking request handling
- Prepares the API for future database integration

---

## How to Run the Project

### 1. Clone the repository

```bash
git clone <your-repo-url>
cd CareerHub_API
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Run the application

```bash
dotnet run
```

### 4. Open Scalar API UI

Once the application is running, open:

```
http://localhost:5038/scalar
```

This opens the Scalar API documentation interface, where you can:

- View all endpoints
- Test API requests in the browser
- Inspect request/response structures

---

## Testing the API

### Get all jobs

```
GET http://localhost:5038/jobs
```

### Get job by ID

```
GET http://localhost:5038/jobs/1
```

---

## Technologies Used

- ASP.NET Core Web API (.NET 10)
- C#
- Scalar OpenAPI UI
- Dependency Injection (DI)
- In-memory data storage
- Async/await patterns

---

## Key Design Principles

- Clean architecture separation (Controllers, Models, Data/Services)
- RESTful API design
- Proper HTTP status codes (200, 404)
- Asynchronous programming practices
- Scalable project structure for future expansion

---

## Author

**Lereko Seholoba**
Software Development Trainee (Bitcube)
# ProductAPI

## Project Description

ProductAPI is an ASP.NET Core Web API built with .NET 10 for managing product records. This project demonstrates CRUD operations, request validation, custom business rules, centralized exception handling, standardized Problem Details responses, and pagination.

## Technologies Used

- C#
- ASP.NET Core Web API (.NET 10)
- Swagger UI / OpenAPI
- Data Annotations
- `IValidatableObject`
- `IExceptionHandler`
- Problem Details

## Product Model

| Property | Type | Description |
|---|---|---|
| `ProductId` | `int` | Unique product identifier |
| `Code` | `string` | Unique product code |
| `Name` | `string` | Product name |
| `Description` | `string` | Product description |
| `Price` | `decimal` | Product price |

## Features

- Create, retrieve, update, and delete products.
- Validate incoming requests using Data Annotations.
- Apply a custom Premium product pricing rule.
- Prevent duplicate product codes.
- Handle missing products using `NotFoundException`.
- Handle duplicate product codes using `ConflictException`.
- Centralize exception handling with `GlobalExceptionHandler`.
- Return standardized Problem Details responses for errors.
- Support pagination using `page` and `pageSize`.
- Return paginated results using a `data` and `meta` response envelope.

## API Endpoints

Base route: `/api/Products`

| Method | Endpoint | Description | Success Status | Error Status |
|---|---|---|---|---|
| GET | `/api/Products?page=1&pageSize=10` | Get paginated products | `200 OK` | `400 Bad Request`, `500 Internal Server Error` |
| GET | `/api/Products/{id}` | Get a product by ID | `200 OK` | `404 Not Found`, `500 Internal Server Error` |
| POST | `/api/Products` | Create a product | `201 Created` with `Location` header | `400 Bad Request`, `409 Conflict`, `500 Internal Server Error` |
| PUT | `/api/Products/{id}` | Update a product | `200 OK` | `400 Bad Request`, `404 Not Found`, `409 Conflict`, `500 Internal Server Error` |
| DELETE | `/api/Products/{id}` | Delete a product | `204 No Content` | `404 Not Found`, `500 Internal Server Error` |

### HTTP Status Codes

- `200 OK` — The request succeeded.
- `201 Created` — A product was created successfully. The response includes a `Location` header.
- `204 No Content` — A product was deleted successfully.
- `400 Bad Request` — The request contains invalid data or pagination parameters.
- `404 Not Found` — The requested product does not exist.
- `409 Conflict` — The product code already exists.
- `500 Internal Server Error` — An unexpected server error occurred.

## Validation Rules

The API validates incoming product requests using request DTOs.

- `Code` is required and cannot exceed 20 characters.
- `Name` is required and must contain between 2 and 100 characters.
- `Description` is required and cannot exceed 500 characters.
- `Price` must be greater than `0` and no more than `1,000,000`.
- Product names containing `Premium`, regardless of capitalization, require a price of at least `100`.
- Product codes must be unique.
- The page number must be at least `1`.
- The page size must be between `1` and `100`.

Invalid requests return `400 Bad Request` with validation details.

## Pagination

The collection endpoint supports pagination through the `page` and `pageSize` query parameters.

Example request:

`GET /api/Products?page=1&pageSize=2`

Example response structure:

```json
{
  "data": [
    {
      "productId": 1,
      "code": "P001",
      "name": "Bread",
      "description": "Freshly baked bread",
      "price": 200
    }
  ],
  "meta": {
    "page": 1,
    "pageSize": 2,
    "totalItems": 5,
    "totalPages": 3
  }
}
```

This example illustrates the response format. The actual values depend on the current in-memory product list.

## Exception Handling

The application uses a centralized exception handler to produce consistent error responses.

- `NotFoundException` maps to `404 Not Found`.
- `ConflictException` maps to `409 Conflict`.
- Unexpected exceptions map to `500 Internal Server Error`.

Error responses use Problem Details where configured. Unexpected exceptions should be logged on the server, and internal exception details or stack traces should not be exposed to clients.

## How to Run

1. Open the `ProductAPI` project in Visual Studio.
2. Restore NuGet packages if prompted.
3. Build the project.
4. Run the application.
5. Open the Swagger UI URL displayed by the application.

The port may differ depending on your launch settings.

## Lab Report and Test Evidence

See [LAB.md](LAB.md) for the lab report, test results, screenshot references, and reflection.

It contains screenshots of the final API behavior. Some required lab checkpoints, including the original baseline behavior and the matching server log line, are not shown in the supplied screenshots and may require additional evidence.

## Limitations

Products are stored in an in-memory list rather than a database. Data resets when the application restarts. Persistent database storage would be more suitable for a production application.

## Conclusion

ProductAPI demonstrates CRUD operations, request validation, custom business rules, centralized exception handling, standardized HTTP responses, and pagination. These practices improve consistency, validation, and error handling in an ASP.NET Core Web API.

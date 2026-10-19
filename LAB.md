# Lab Report: Product API

## Project Overview

This project is an ASP.NET Core Web API for managing products. It implements CRUD operations, request validation, custom exceptions, global exception handling, and pagination.

The API uses an in-memory list to store product data. Changes are reset when the application restarts.

## Part 0 – Baseline Behavior

The purpose of this part is to observe the API's original behavior before implementing validation and exception handling.

The original API should be tested using valid and invalid product data, including requests for product IDs that do not exist.

**Evidence:** Add screenshots of the original behavior if available.

## Part 1 – Request Validation

The API validates incoming product data using Data Annotations and custom validation rules.

The validation requirements are:

- `Code` is required and has a maximum length of 20 characters.
- `Name` is required and must contain between 2 and 100 characters.
- `Description` is required and has a maximum length of 500 characters.
- `Price` must be greater than 0 and must not exceed 1,000,000.
- Product codes must be unique.
- Product names containing `Premium`, regardless of capitalization, must have a price of at least 100.

The `[ApiController]` attribute automatically returns a `400 Bad Request` response when model validation fails.

## Part 2 – Custom Exceptions

The API uses custom exceptions to handle errors consistently.

- `NotFoundException` is used when a requested product does not exist.
- `ConflictException` is used when an operation conflicts with existing data, such as creating a product with a duplicate code.

Expected responses include `404 Not Found` for missing products and `409 Conflict` for duplicate product codes.

## Part 3 – Global Exception Handling

The API uses `GlobalExceptionHandler`, which implements `IExceptionHandler`, to handle exceptions centrally.

The exception handler is registered in `Program.cs` and activated using `UseExceptionHandler()`.

The API handles errors using appropriate HTTP status codes and Problem Details responses where configured. Unexpected errors should return `500 Internal Server Error` without exposing internal stack traces to clients.

## Part 4 – Pagination

The API supports pagination through the `page` and `pageSize` query parameters.

Example request:

`GET /api/Products?page=1&pageSize=2`

The response contains:

- `data`: the products returned for the requested page.
- `meta`: pagination information, including the current page, page size, total number of items, and total number of pages.

The page number must be at least 1, and the page size must be between 1 and 100. Invalid pagination parameters should return `400 Bad Request`.

## Test Evidence

The following test scenarios are covered by the API:

| Test Scenario | Expected Status |
|---|---|
| Get paginated products | 200 OK |
| Get an existing product | 200 OK |
| Get a nonexistent product | 404 Not Found |
| Create a valid product | 201 Created |
| Submit invalid product data | 400 Bad Request |
| Create a product with a duplicate code | 409 Conflict |
| Create a Premium product priced below 100 | 400 Bad Request |
| Update a nonexistent product | 404 Not Found |
| Delete a nonexistent product | 404 Not Found |
| Delete an existing product | 204 No Content |
| Use invalid pagination parameters | 400 Bad Request |

Screenshots from Swagger UI should show the endpoint tested, the request parameters or body, and the resulting status code and response.

For the create operation, include the `201 Created` response and the `Location` header if available. For pagination, show the product data and metadata. For validation and exception handling, show the returned error response.

Only tests that have actually been executed should be marked as completed. Add screenshots of the original baseline behavior and server logs if these are required by the lab instructions.

## Reflection

Through this lab, I learned how to create CRUD endpoints in an ASP.NET Core Web API, validate incoming requests, implement custom exceptions, and handle errors centrally. I also learned how pagination returns data in manageable groups along with metadata. Swagger UI helped me execute requests and inspect the API responses during testing.

## Conclusion

The Product API demonstrates product management through CRUD operations, request validation, custom exceptions, global exception handling, and pagination. Swagger UI provides an interface for testing the endpoints and examining their responses. The screenshots serve as evidence of the tests performed.

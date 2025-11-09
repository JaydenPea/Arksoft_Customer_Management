# Arksoft Customer Management API Documentation

## Authentication
All API endpoints require an API key to be provided in the header:
```
X-API-KEY: arksoft-demo-key-123456
```

## Base URL
```
https://localhost:7000/api/customers
```

## Endpoints

### 1. Get All Customers
**GET** `/api/customers`

**Query Parameters:**
- `search` (optional): Filter by name or VAT number
- `page` (optional): Page number (default: 1)
- `pageSize` (optional): Items per page (default: 10)

**Example Request:**
```bash
curl -X GET "https://localhost:7000/api/customers?search=acme&page=1&pageSize=5" \
  -H "X-API-KEY: arksoft-demo-key-123456"
```

### 2. Get Customer by ID
**GET** `/api/customers/{id}`

**Example Request:**
```bash
curl -X GET "https://localhost:7000/api/customers/1" \
  -H "X-API-KEY: arksoft-demo-key-123456"
```

### 3. Create New Customer
**POST** `/api/customers`

**Request Body:**
```json
{
  "name": "New Company Ltd",
  "address": "123 Business Street, City, 1234",
  "telephoneNumber": "+27 11 123 4567",
  "contactPersonName": "John Doe",
  "contactPersonEmail": "john@company.com",
  "vatNumber": "ZA1234567890"
}
```

**Example Request:**
```bash
curl -X POST "https://localhost:7000/api/customers" \
  -H "X-API-KEY: arksoft-demo-key-123456" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "New Company Ltd",
    "address": "123 Business Street, City, 1234",
    "telephoneNumber": "+27 11 123 4567",
    "contactPersonName": "John Doe",
    "contactPersonEmail": "john@company.com",
    "vatNumber": "ZA1234567890"
  }'
```

### 4. Update Customer
**PUT** `/api/customers/{id}`

**Request Body:** (Same as Create)

**Example Request:**
```bash
curl -X PUT "https://localhost:7000/api/customers/1" \
  -H "X-API-KEY: arksoft-demo-key-123456" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Company Name",
    "address": "Updated Address"
  }'
```

### 5. Delete Customer
**DELETE** `/api/customers/{id}`

**Example Request:**
```bash
curl -X DELETE "https://localhost:7000/api/customers/1" \
  -H "X-API-KEY: arksoft-demo-key-123456"
```

## Response Formats

### Success Response
```json
{
  "id": 1,
  "name": "Acme Corporation",
  "address": "123 Business Park, Johannesburg, 2001",
  "telephoneNumber": "+27 11 123 4567",
  "contactPersonName": "John Smith",
  "contactPersonEmail": "john.smith@acme.com",
  "vatNumber": "ZA1234567890",
  "createdAt": "2024-11-09T10:30:56Z",
  "updatedAt": null
}
```

### Error Response
```json
{
  "error": "Customer not found",
  "statusCode": 404
}
```

## Status Codes
- `200` - OK (Success)
- `201` - Created (New resource created)
- `204` - No Content (Delete successful)
- `400` - Bad Request (Invalid data)
- `401` - Unauthorized (Invalid API key)
- `404` - Not Found (Resource doesn't exist)
- `500` - Internal Server Error

## Logging
All API requests are logged with Serilog to:
- Console output
- Daily rolling log files in `/Logs/` directory
- Includes request details, response status, and performance metrics
# Product Inventory App

A full-stack application built with ASP.NET Core Web API and Blazor WebAssembly for managing a simple product inventory system. Includes CRUD operations, filtering, pagination, and client-side state management.

---

##  Features

-  RESTful API with ASP.NET Core
-  Blazor WebAssembly UI with MudBlazor components
-  In-memory data with EF Core and seeded from `products_data.json`
-  Pagination, filtering, and URL query syncing
-  Client-side state management to preserve UI state between navigations
-  Logging using Serilog (output to `/Logs` folder)
-  Docker support for easy deployment


##  Technologies

- .NET 9
- Blazor WebAssembly
- Entity Framework Core (InMemory)
- MudBlazor UI library
- Serilog for logging


##  Project Structure

ProductInventory/\
 ProductInventory.Api/  ASP.NET Core Web API project\
 ProductInventory.Blazor/  Blazor WebAssembly front-end\
 Dockerfile  Docker setup to serve both frontend and backend\
 README.md

---

##  Running the App

###  Locally (Development mode)

#### 1. Run API

```bash
cd ProductInventory.Api
dotnet run --launch-profile "https"
```
API runs at: https://localhost:7194, and Swagger UI is available on https://localhost:7194/swagger

#### 2. Run Blazor Frontend
```bash
cd ProductInventory.Blazor
dotnet run --launch-profile "https"
```

Blazor app runs at: https://localhost:7101. Ensure CORS policies in API allow Blazor port (https://localhost:7101).

###  With Docker
```bash
cd ProductInventory
docker build -t product-inventory-app .
docker run -d -p 8080:80 --name inventory-app product-inventory-app
```

Visit the app at: http://localhost:8080. Note: HTTPS not enabled in Docker by default.

---

##  API Overview

###  Swagger

Launch your browser at
https://localhost:7194/swagger

**1. Get products**
```http
GET /api/products
```
Response:
```json
{
  "products":
  [
    {
      "id": 1,
      "name": "Laptop",
      "description": "Mechanical keyboard",
      "price": 6000,
      "quantity": 100
    },
    ...
  ],
  "totalCount": 10
}
```

**2. Get product**
```http
GET /api/products/1
```

Response:
```json
{
  "id": 1,
  "name": "Keyboard",
  "description": "Mechanical keyboard",
  "price": 6000,
  "quantity": 100
}
```

**3. Create product**
```http
POST /api/products
```

Request:
```http
POST /api/products
Content-Type: application/json

{
  "name": "Mouse",
  "description": "Wireless mouse",
  "price": 25,
  "quantity": 200
}
```

**4. Update product**
```http
PUT /api/products/1
```

Request:
```http
PUT /api/products/1
Content-Type: application/json

{
  "id": 1,
  "name": "Updated Keyboard",
  "description": "RGB Mechanical",
  "price": 70,
  "quantity": 90
}
```

**5. Delete product**
```http
DELETE /api/products/1
```
---

##  Design Decisions

 EF Core InMemory Provider\
Used for simplicity—no external database needed. Initial data is loaded from products_data.json on app startup.

 Seed File: products_data.json\
This file contains mock data to populate the in-memory database when the app launches.

 UI with MudBlazor\
Provides clean, modern components. Used for forms, tables, dialogs, and snackbar messages.

 Client State Management\
A singleton AppStateService stores:
- Current filter (name, minPrice, maxPrice)
- Product cache (this prevents unnecessary API calls between navigations and preserves state).

 Logging with Serilog\
All API requests/responses are logged using Serilog and stored in the `/Logs` folder.

---

##  Testing the API

After running `dotnet run` in the API project:
```bash
curl https://localhost:7194/api/products
```
Or use Swagger UI to explore interactively.

##  Testing the Blazor Application

After starting the app locally (https://localhost:7101):

**1. Add Product**
- Click *Add Product*.
- Fill out the form and submit.
- The product should appear in the table at the last page immediately.

**2. Edit/Delete Product**
- Use the action buttons on the product table.
- Confirm updates or deletions reflect in the list without refreshing the page.

**3. Filtering**
- Enter product name, min price, and/or max price.
- Verify the table updates to show only products matching the filter.
- Check that filter parameters persist in the URL.

**4. Paging**
- Use pagination controls to navigate between pages.
- Confirm the correct page is shown, and the product list updates accordingly.

---

Created by Aleksandr Toroshchin, 2025

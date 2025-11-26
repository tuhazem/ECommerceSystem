# Supermarket Inventory & Sales System (ASP.NET Core + EF Core)

## 🛒 Overview

This project is a **Supermarket Inventory and Sales Management System** built using **ASP.NET Core Web API** and **Entity Framework Core**. The system manages:

* Products & stock levels
* Inventory history
* Expiry tracking
* Sales (POS)
* Discounts & receipts
* Systems (multiple branches or inventories)

The project follows clean RESTful API structure and includes custom endpoints for real business needs.

---

## 🚀 Features

### 📦 Inventory Management

* Add / Update / Delete Products
* Track Stock Quantity
* "Low Stock" endpoint
* "Expiring Soon" endpoint
* Track stock changes in StockHistory

### 🧾 Sales (POS) Module

* Create Sales
* Apply discounts
* Auto‑generate receipt
* Deduct stock based on sales
* Handle multi‑item checkout

### 🏪 System Management

* Each product belongs to a **System** (branch / warehouse)
* Filter products per system

---

## 🧱 Tech Stack

* **ASP.NET Core 8 Web API**
* **Entity Framework Core**
* **SQL Server**
* **AutoMapper**
* **Repository Pattern**
* **DTOs**

---

## 📂 Project Architecture

```
SuperMarket.API
│
├── Controllers
├── DTOs
├── Models
├── Repositories
│   ├── Interfaces
│   ├── Implementations
├── Services (optional)
├── Migrations
└── Program.cs
```

---

## 🗂️ Important Endpoints

### Products

```
GET  /api/products
GET  /api/products/low-stock
GET  /api/products/expiring-soon
POST /api/products
PUT  /api/products/{id}
DELETE /api/products/{id}
```

### Sales

```
POST /api/sales        # create sale
GET  /api/sales/{id}   # get receipt
```

### Systems

```
GET  /api/systems
POST /api/systems
```

---

## 💡 Example Sale JSON

```
{
  "systemId": 1,
  "items": [
    { "productId": 3, "quantity": 2 },
    { "productId": 5, "quantity": 1 }
  ],
  "discount": 10
}
```

---

## 🧪 How to Run

1. Create a SQL Server database
2. Update `appsettings.json`
3. Run migrations:

```
dotnet ef database update
```

4. Run the API:

```
dotnet run
```

---


## 👨‍💻 Author

Hazem Mohamed

If you like the project ⭐ **Star it on GitHub!**


#  Orders API – .NET 8 + Redis

This project is a simple Orders API built for a technical assessment.
It demonstrates:

* .NET 8 Web API
* EF Core (Code-First + Migrations)
* Redis caching (5-minute TTL)
* Clean structure
* Async/await
* Error Handling
* Basic logging

---

##  Endpoints

| Method | Endpoint       | Description                       |
| ------ | -------------- | --------------------------------- |
| POST   | `/orders`      | Create a new order                |
| GET    | `/orders/{id}` | Get order by ID (cached in Redis) |
| GET    | `/orders`      | List all orders                   |
| DELETE | `/orders/{id}` | Delete order + remove from cache  |

---

## Tech Used

* .NET 8
* Entity Framework Core
* SQL Server (or your chosen DB)
* Redis (StackExchange.Redis)
* Swagger

---

##  Swagger Screenshot

### 1. Create Endpoint
![Create Endpoint](/assets/Create.png)

### 2. Get Single Endpoint
![Get Endpoint](/assets/GetOrder.png)

### 3. Get All Endpoint
![Get All Endpoint](/assets/GetOrders.png)

### 4. Delete Endpoint
![Delete Endpoint](/assets/Delete.png)

### 5. Redis 
![Redis Endpoint](/assets/Redis.png)
---

#  Written Questions & Answers

### 1. **Redis vs SQL**

* Redis: in-memory, key-value, very fast, non-relational, used for caching
* SQL: disk-based, relational, supports transactions and complex queries

### 2. **When not to use caching?**

* When data changes frequently
* When data must always be fresh
* When memory is limited
* When stale data may cause issues

### 3. **What if Redis is down?**

* API should still work using SQL
* Cache failures must not break the request
* Log the error and continue normally

### 4. **Optimistic vs pessimistic locking**

* Optimistic: assumes no conflicts, checks version on update
* Pessimistic: locks the row to prevent other writes

### 5. **Ways to scale a .NET backend**

* Horizontal scaling
* Vertical scaling
* Caching (Redis)
* Architectural Patterns
* Code Optimizations
---

##  Author

Hashem Tarek

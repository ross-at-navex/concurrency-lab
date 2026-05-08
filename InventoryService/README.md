# Inventory Concurrency Lab

A small .NET + EF Core project used as a **hands-on lab for database concurrency concepts**.  
This is not a production application — it is a controlled environment to **create, observe, and fix concurrency bugs**.

Inspired by classic ideas from *Designing Data-Intensive Applications* and similar database design literature.

---

## Purpose

- Demonstrate **lost updates**, **write conflicts**, and **invariant violations**
- Compare **naïve**, **optimistic**, and **pessimistic** concurrency approaches
- Experiment with **transactions** and **isolation levels**
- Build intuition about database behavior under contention

If a feature does not help explain concurrency, it does not belong in this project.

---

## Domain (Intentionally Minimal)

### Product

- `Id`
- `Name`
- `StockQuantity`
- `RowVersion` (used for optimistic concurrency experiments)
- `LastUpdatedAt`

### Order

- `Id`
- `ProductId`
- `Quantity`
- `CreatedAt`

**Invariant (often violated on purpose):**
```
StockQuantity >= 0
```

---

## Core Operation

**Purchase**

1. Read product
2. Check available stock
3. Create order
4. Decrement stock
5. Save changes to product stock, insert new order

The same logical operation is implemented multiple ways to observe different concurrency behaviors.

---

## Concurrency Scenarios

Each experiment changes only the **concurrency control strategy**.

### 1. Naïve Implementation

- No explicit transactions
- No concurrency tokens

**Expected results:**
- Lost updates
- Overselling
- Silent data corruption

---

### 2. Optimistic Concurrency

- `RowVersion` on `Product`
- Handle `DbUpdateConcurrencyException`
- Retry logic

**Expected results:**
- No lost updates
- Some failed or retried purchases
- Higher throughput than locking

---

### 3. Pessimistic Locking

- Explicit transactions
- Locked reads (`SELECT ... FOR UPDATE` or equivalent)

**Expected results:**
- Serialized access
- No invariant violations
- Reduced concurrency

---

### 4. Isolation Level Experiments

- Same purchase logic
- Different isolation levels:
  - Read Committed
  - Repeatable Read
  - Serializable

**Expected results:**
- Blocking
- Possible deadlocks
- Clear safety vs throughput tradeoffs

---

## Execution Model

- Real relational database (SQL Server or PostgreSQL)
- Parallel execution to force contention
- Usually run via automated tests
- One hot row (`Product`) to maximize conflicts

Observed outputs typically include:
- Final stock quantity
- Number of orders created
- Failed or retried attempts
- Exceptions encountered

---

## Non-Goals

- No user interface
- No users, authentication, or payments
- No repository pattern or CQRS
- No attempt to fix everything immediately

This is a **learning lab**, not an application template.

---

## Project Structure

```
InventoryConcurrencyLab/
 ├─ Domain/
 ├─ Infrastructure/
 ├─ Experiments/
 └─ README.md
```

Each experiment should briefly document:
- What was expected
- What actually happened
- Why the result makes sense

---

## Success Criteria

This project is successful if you can:

- Reproduce concurrency bugs on demand
- Fix them using multiple techniques
- Clearly explain when each approach is appropriate


## Up next
- Add code first database creation for postgres
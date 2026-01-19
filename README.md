# C# Interview Preparation Exercises

A comprehensive collection of C# exercises and demonstrations covering topics from core concepts to advanced system design, high-performance programming, and microservices resilience.

## 🏗️ Project Structure

The project follows a clean architecture approach, separating business logic into reusable services.

### 🛠️ Core Services (`/Services`)
- **`StringAlgorithms`**: High-performance string manipulations and analysis.
- **`ArrayAlgorithms`**: Efficient array operations (Merging, Rotation, Searching).
- **`NumericAlgorithms`**: Mathematical sequences and FizzBuzz implementations.
- **`DataProcessingService`**: LINQ-based data aggregation and filtering.

### 🧪 Exercise Modules (`/Exercises`)
1.  **Core C# Concepts**: LINQ, Collections, and basic logic.
2.  **Problem Solving**: Anagrams, Duplicates, and Two-pointer techniques.
3.  **Advanced Topics**: Async/Await, Race conditions, and Decorator patterns.
4.  **OOP & Design Patterns**: Strategy, Factory, Singleton, Adapter, and Observer.
5.  **System Design**: Large file sorting, Leaderboards, and Thread-safe counters.
6.  **High Performance**: Zero-allocation code using `Span<T>`.
7.  **Controller Challenge**: Real EF Core CRUD with Dependency Injection, In-Memory DB, and Structured Logging.
8.  **Middleware Pipeline**: ASP.NET Core request/response lifecycle simulation.
9.  **Critical Concepts**: Reflection, `IDisposable`, and `yield return`.
10. **Concurrency Patterns**: High-throughput Producer-Consumer using `System.Threading.Channels`.
11. **Resilience**: Implementing Retry patterns with Exponential Backoff.
12. **Memory Management**: Stack vs Heap, Boxing costs, and `ReadOnlySpan<T>`.
13. **Async Streams**: `IAsyncEnumerable` for Big Data and `SemaphoreSlim` for Throttling.
14. **Modern LINQ**: New features in .NET 6-9 (`CountBy`, `AggregateBy`, `Index`).

## 🚦 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker** (Optional, for building the container)

## 🚀 How to Run

### Execute Exercises
To run all demonstrations and see the console output:
```bash
dotnet run
```

### Run Tests
The project includes an xUnit test suite for algorithms and the API controller:
```bash
dotnet test
```

### Run with Docker
Build and run the application as a container:
```bash
docker build -t csharp-interview-prep .
docker run --rm csharp-interview-prep
```

## 💎 Key Highlights

- **Performance First**: Demonstrations of `Span<T>` and memory-efficient patterns.
- **Big Data Ready**: Usage of Async Streams and Channels for high-throughput processing.
- **Real-World ORM**: Full CRUD implementation using **Entity Framework Core**.
- **Production Ready**: Structured Logging (`ILogger`), Integration Tests, and Docker support.
- **Clean Code**: Adherence to SOLID principles and DRY (Don't Repeat Yourself).

## 📜 Technologies Used

- **.NET 9.0**
- **Entity Framework Core 9.0** (InMemory Provider)
- **xUnit** (Unit Testing)
- **Microsoft.Extensions.DependencyInjection**
- **Microsoft.Extensions.Logging**
- **System.Threading.Channels**
- **System.Runtime.InteropServices** (Memory Management)
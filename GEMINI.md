# Gemini Project Memories - CSharpInterviewPrep

This file tracks the architectural decisions, modifications, and progress made by the Gemini AI assistant.

## Current Status
- **Phase 3: Kissterra Interview Alignment (Complete)**
    - **Final Polish (Senior Level)**:
        - Added `Dockerfile` for containerization support.
        - Refactored `OrdersController` to use `ILogger<T>` (Structured Logging) instead of `Console.WriteLine`.
        - Added `OrdersControllerTests.cs` (Integration Testing) to verify API logic + Database interactions.
    - **Architecture & Features**:
        - Refactored project structure: Created `Services` namespace to separate logic from presentation (MVC pattern).
        - Added `L_MemoryManagement.cs`: Deep dive into `Span<T>`, Stack vs Heap, and Boxing/Unboxing.
        - Added `M_AsyncStreams.cs`: Implemented `IAsyncEnumerable` and `SemaphoreSlim`.
        - Added `N_ModernLinq.cs`: Demonstrating .NET 9 specific features.
        - Integrated **Entity Framework Core**: Replaced `FakeDbContext` with `AppDbContext` (InMemory).
    - **Cleanup**: Refactored `A_CoreConcepts` and `B_ProblemSolving` to use Service classes.

- **Phase 2: Job Requirements Alignment (Complete)**
    - Added `J_ConcurrencyPatterns.cs` (Producer-Consumer).
    - Added `K_Resilience.cs` (Retry Pattern).
    - Added `CSharpInterviewPrep.Tests` (xUnit).

## Architectural Notes
- **ORM**: Uses **Entity Framework Core** with In-Memory DB for realistic data layer simulation.
- **Logging**: Uses `Microsoft.Extensions.Logging` for structured logs, following cloud-native best practices.
- **Testing**: Includes both Unit Tests (Algorithms) and Integration Tests (Controller + DB).
- **Concurrency**: `J_ConcurrencyPatterns` uses `BoundedChannel` for backpressure handling.

## Key Findings & Fixes
- **Package Compatibility**: Resolved NuGet conflicts by upgrading `Microsoft.Extensions` packages to `9.0.0`.
- **Ambiguity**: Resolved `ILogger` naming conflict between custom interface and `Microsoft.Extensions.Logging` by using fully qualified names.
- **Race Conditions**: Fixed using `lock` and `Interlocked` in Section 3.
# Gemini Project Memories - CSharpInterviewPrep

This file tracks the architectural decisions, modifications, and progress made by the Gemini AI assistant.

## Current Status
- **Phase 3: Kissterra Interview Alignment (Complete)**
    - Refactored project structure: Created `Services` namespace to separate logic from presentation (MVC pattern).
    - Added `L_MemoryManagement.cs`: Deep dive into `Span<T>`, Stack vs Heap, and Boxing/Unboxing for high-performance apps.
    - Added `M_AsyncStreams.cs`: Implemented `IAsyncEnumerable` for streaming big data and `SemaphoreSlim` for throttling high-concurrency requests.
    - Refactoring: Moved OOP concepts (Notification System, Decorator Pattern) from `C_AdvancedTopics` to `D_ObjectOrientedProgramming` for better logical organization.
    - Full project Refactoring: Cleaned up `A_CoreConcepts` and `B_ProblemSolving` to use new Service classes.

- **Phase 2: Job Requirements Alignment (Complete)**
    - Added `J_ConcurrencyPatterns.cs` to demonstrate High-Throughput Producer-Consumer patterns using `System.Threading.Channels`.
    - Added `K_Resilience.cs` to demonstrate Microservice Resilience using Retry patterns with Exponential Backoff.
    - Added `CSharpInterviewPrep.Tests` (xUnit) project to demonstrate Clean Code and TDD practices.
    - Integrated new exercises into `Program.cs`.

- **Phase 1: Refactoring & Cleanup (Complete)**
    - Standardized all exercise files with a static `Run()` or `async Task Run()` method.
    - Simplified `Program.cs` to act as a clean entry point for all sections.
    - Verified all exercises execute correctly and produce the expected output.
    - Fixed inconsistent section numbering in console logs.

## Architectural Notes
- **Memory Management**: `L_MemoryManagement` demonstrates "Zero Allocation" techniques essential for Big Data.
- **Async Streams**: `M_AsyncStreams` showcases modern .NET patterns for handling large datasets without blocking.
- **Concurrency**: `J_ConcurrencyPatterns` uses `BoundedChannel` to handle backpressure, a key concept for processing "Big Data" streams without OOM errors.
- **Resilience**: `K_Resilience` implements a custom retry logic. In a production scenario, I would recommend using the `Polly` library, but this manual implementation proves understanding of the underlying algorithm.
- **Testing**: The project now has a dedicated test assembly `CSharpInterviewPrep.Tests` referencing the main project, allowing for automated verification of logic.

## Key Findings & Fixes
- **Race Conditions**: Demonstrated and fixed race conditions using `lock` and `Interlocked` in Section 3.
- **Memory Efficiency**: Implemented `Span<char>` for zero-allocation string parsing in Section 6.
- **Middleware Order**: Implemented a nested delegate structure to simulate the onion architecture of ASP.NET Core middleware in Section 8.

## Future Tasks
- [ ] Expand `E_SystemDesignProblems` with a more robust `LargeFileSorter` implementation.
- [ ] Add a section for gRPC or SignalR basics.
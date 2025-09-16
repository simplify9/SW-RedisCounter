# SW.RedisCounter

[![Build and Publish NuGet Package](https://github.com/simplify9/SW-RedisCounter/actions/workflows/nuget-publish.yml/badge.svg)](https://github.com/simplify9/SW-RedisCounter/actions/workflows/nuget-publish.yml)
[![NuGet version](https://badge.fury.io/nu/SimplyWorks.RedisCounter.svg)](https://badge.fury.io/nu/SimplyWorks.RedisCounter)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A .NET Core library that provides Redis-based atomic counters with environment-aware key namespacing for distributed applications.

## Features

- **Atomic Operations**: Thread-safe increment and reset operations using Redis
- **Environment Aware**: Automatically namespaces keys with application name and environment
- **Easy Integration**: Simple dependency injection setup for ASP.NET Core applications
- **SSL Support**: Built-in SSL support for secure Redis connections
- **Multiple Server Support**: Supports comma-separated Redis server endpoints

## Installation

Install the package via NuGet Package Manager:

```bash
Install-Package SimplyWorks.RedisCounter
```

Or via .NET CLI:

```bash
dotnet add package SimplyWorks.RedisCounter
```

## Configuration

Add Redis configuration to your `appsettings.json`:

```json
{
  "Redis": {
    "ApplicationName": "MyApp",
    "Password": "your-redis-password",
    "Server": "redis-server1.com:6380,redis-server2.com:6380"
  }
}
```

## Usage

### 1. Register the Service

In your `Startup.cs` or `Program.cs`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register Redis Counter with configuration from appsettings.json
    services.AddRedisCounter();
    
    // Or configure programmatically
    services.AddRedisCounter(options =>
    {
        options.ApplicationName = "MyApp";
        options.Password = "your-redis-password";
        options.Server = "redis-server.com:6380";
    });
}
```

### 2. Use in Your Controllers/Services

```csharp
[ApiController]
[Route("[controller]")]
public class MyController : ControllerBase
{
    private readonly IAtomicCounter _counter;

    public MyController(IAtomicCounter counter)
    {
        _counter = counter;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // Increment a counter by 1
        var count = await _counter.IncrementAsync("api-calls");
        
        // Increment by a specific amount
        var pageViews = await _counter.IncrementAsync("page-views", 5);
        
        // Reset a counter
        await _counter.ResetAsync("api-calls");
        
        return Ok(new { ApiCalls = count, PageViews = pageViews });
    }
}
```

## Key Naming Convention

Keys are automatically namespaced using the pattern:
```
{ApplicationName}:{EnvironmentName}:{CounterName}
```

For example, with:
- ApplicationName: "MyApp"
- Environment: "Production" 
- CounterName: "api-calls"

The Redis key becomes: `myapp:production:api-calls`

## API Reference

### IAtomicCounter Interface

```csharp
public interface IAtomicCounter
{
    Task<long> IncrementAsync(string counterName, long increment = 1);
    Task ResetAsync(string counterName);
}
```

#### Methods

- **IncrementAsync(string counterName, long increment = 1)**
  - Atomically increments the counter by the specified amount
  - Returns the new counter value
  - Default increment is 1

- **ResetAsync(string counterName)**
  - Deletes the counter (resets to 0)
  - Returns a Task for async completion

### RedisOptions Class

```csharp
public class RedisOptions
{
    public string ApplicationName { get; set; }
    public string Password { get; set; }
    public string Server { get; set; }
}
```

## Dependencies

- **StackExchange.Redis**: Redis client for .NET
- **SimplyWorks.PrimitiveTypes**: Provides the IAtomicCounter interface
- **Microsoft.AspNetCore.App**: ASP.NET Core framework

## Target Framework

- .NET Core 3.1

## License

This project is licensed under the [MIT License](LICENSE).

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Repository

Source code is available at: https://github.com/simplify9/SW-RedisCounter
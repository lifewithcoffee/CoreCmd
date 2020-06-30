CoreCmd supports Microsoft.Extensions.DependencyInjection.

Example:

``` csharp
public interface IGreeting
{
    void Greet();
}

public class Greeting : IGreeting
{
    public void Greet()
    {
        Console.WriteLine("Greeting.Greet() called");
    }
}

public class HelloCommand
{
    IGreeting _greeting
    
    public HelloCommand(IGreeting greeting)
    {
        _greeting = greeting;
    }

    public void World()
    {
        _greeting.Greet();
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        await new AssemblyCommandExecutor().ExecuteAsync(args, services => {
            services.AddScoped<IGreeting, Greeting>();
        });
    }
}
```
If a command class has async methods, the entry Main() method is recommended to
use async as well:


``` csharp
public class HelloCommand
{
    public async Task World()
    {
        // some await operation here
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        await new AssemblyCommandExecutor().ExecuteAsync(args);
    }
}
```
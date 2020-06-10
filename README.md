# About CoreCmd

CoreCmd is a MVC-like convention based command line automatic parser.

See [full document](https://li-rongcheng.github.io/CoreCmd)

## Quick Start

1. Build a command line project

2. Install [CoreCmd NuGet package](https://www.nuget.org/packages/CoreCmd): 

   > dotnet add package CoreCmd

3. Implement the entry `Main` method in the `Program.cs` file:

    ``` charp
    using CoreCmd.CommandExecution;
    using System;

    namespace MyFancyCmd
    {
        // the 1st level subcommand: hello
        class HelloCommand
        {
            // the 2nd level subcommand: ben
            public void Ben()
            {
                Console.WriteLine("Ben() is called");
            }
        }

        // the 1st level subcommand: good-morning
        class GoodMorningCommand
        {
            // the 2nd level subcommand: harry-potter
            public void HarryPotter(string param1, int param2)
            {
                Console.WriteLine($"HarryPotter() is called, param1={param1}, param2={param2}");
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                // the only implementation for command parsing
                new AssemblyCommandExecutor().Execute(args);
            }
        }
    }
    ```

4. Execute:

    ``` console
    d:\(project-output-dir)> dotnet myfancycmd.dll hello ben
    Ben() is called
    
    d:\(project-output-dir)> dotnet myfancycmd.dll good-morning harry-potter abcd 1234
    HarryPotter() is called, param1=abcd, param2=1234
    ```



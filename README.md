# CoreCmd

**NOTICE: CoreCmd is still an experiment project, it will be your own risk to**
**use it in any project or product.**

## Basic usage

1. Build a command line project

2. Install CoreCmd package:
 
   `dotnet add package CoreCmd`

3. Implement the entry `Main` method in the `Program.cs` file:

    ``` c#
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
    ``` shell
    d:\(project-output-dir)> dotnet myfancycmd.dll hello ben
    Ben() is called
    
    d:\(project-output-dir)> dotnet myfancycmd.dll good-morning harry-potter abcd 1234
    HarryPotter() is called, param1=abcd, param2=1234
    ```


## Release notes

### v1.2

- Provide an 'alias' attribute to specify a subcommand's short name
- Separate project CoreCmd.Cli from project CoreCmd
- Add support to release as global dotnet tool command
- Rename buildin command 'cmd' to 'config"
- Display the version info of both entry dll and execution dll
- Able to customize the global configuration file name

### v1.0

- Add an interactive shell (activate by: `core shell`)
- Display dll location when print command help info

### v0.4

- Upgrade to .net core 2.2

### v0.3.0

- Use Assembly.LoadFrom() instead of Assembly.LoadFile() to load DLLs
- Change version schema to use three digits

### v0.2.5.11

*Basic features:*

- Customized command discovery
  - Global command assembly registration
- Command execution
  - Parameter matching
- Help information printing
  - Subcommand and parameter listing
  - [Help()] attribute support
- Buildin commands (help, version, default)

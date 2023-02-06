# Latest Releases

## Working (v2.0)

```
- Upgrade to .net7
```

## Releases

v1.6.0

```
CoreCmd Enhancement:
- Improve help info
- Remove alias prompt "... is an alias command for ..."

CoreCmd Bug fixes:
- #4 'Default' subcommand can't work
- #5 Subcommands can't work if all their parameters have default values

CoreCmd.Cli:
- Upgrade CoreCmd dependency to v1.6.0
```

v1.5.0

```
CoreCmd Bugfix:
- #1 Subcommands in a base command class cannot be printed in the help info
- #3 MethodMatcher.GetMethodInfo() should not be called twice when executing a normal subcommand

CoreCmd Enhancement:
- #2 Every command shall have a 'help' sub-command

CoreCmd.Cli:
- Upgrade CoreCmd dependency to v1.5.0
```

v1.4.0

```
CoreCmd:
- Integrate with Microsoft.Extensions.DependencyInjection
- Upgrade NetCoreUtils dependency

CoreCmd.Cli:
- Upgrade CoreCmd dependency to v1.4.0
```

v1.3.0.1

```
CoreCmd:
- Add support for async commands
- upgrade to .net core 3.1

CoreCmd.Cli:
- Upgrade to .net core 3.1
- Upgrade CoreCmd dependency to v1.3.0.1
- Change Main() method to async
```

v1.3.0

```
- Add support for async commands
- upgrade to .net core 3.1
```

v1.2

```
- Add 'alias' attribute to specify a subcommand's short name
- Separate project CoreCmd.Cli from project CoreCmd
- Add support to release as global dotnet tool command
- Rename buildin command 'cmd' to 'config"
- Display the version info of both entry dll and execution dll
- Able to customize the global configuration file name
```

v1.0

```
- Add an interactive shell (activate by: `core shell`)
- Display dll location when print command help info
```

v0.4

```
- Upgrade to .net core 2.2
```

v0.3.0

```
- Use Assembly.LoadFrom() instead of Assembly.LoadFile() to load DLLs
- Change version schema to use three digits
```

v0.2.5.11

```
- Global command assembly registration & customized command discovery
- Parameter matching
- Help information printing:
	* Subcommand and parameter listing
	* [Help()] attribute support
- Buildin commands (help, version, default etc.)
```
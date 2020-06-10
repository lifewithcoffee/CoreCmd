# Release Notes

## 1.3-working

- Add support for async commands

## 1.2

- Add 'alias' attribute to specify a subcommand's short name
- Separate project CoreCmd.Cli from project CoreCmd
- Add support to release as global dotnet tool command
- Rename buildin command 'cmd' to 'config"
- Display the version info of both entry dll and execution dll
- Able to customize the global configuration file name

## 1.0

- Add an interactive shell (activate by: `core shell`)
- Display dll location when print command help info

## 0.4

- Upgrade to .net core 2.2

## 0.3.0

- Use Assembly.LoadFrom() instead of Assembly.LoadFile() to load DLLs
- Change version schema to use three digits

## 0.2.5.11

*Basic features:*

- Customized command discovery
  - Global command assembly registration
- Command execution
  - Parameter matching
- Help information printing
  - Subcommand and parameter listing
  - [Help()] attribute support
- Buildin commands (help, version, default)
# CoreCmd

## Motivation

## How does it work

## Usage scenarios

## Release notes

### v1.1-working

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

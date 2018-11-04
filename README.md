# Reloadble Injection Demo

A demo project outlining how to inject C# dlls into remote processes and reload them. Supports both x86 and x64 injection.

## How It Works

Example: An exmaple executable to be injected into. Sits waiting to be injected into.

Library: An example library to be executed. Must contain a single method with STAThread attribute and expect a single string parameter.

Host: Library that manages the reloading. It bootstraps itself and then controls creating/unloading app domains with the requested library loaded.

Injecter: Console application to execute the demo.

A high level overview of the execution:

1. Load Host library into process (using CreateRemoteThread injection)
2. Find offset of Host's 'Run' function in process
3. Call 'Run' function in process passing library to run arguments (using CreateRemoteThread again)
4. Execute now proceeds within process
5. Create new app domain and load requested library
6. Invoke 'Run' function in requested library by finding method with STAThread attribute
7. Wait for 'Run' to complete and repeat 5-7 on key press

## Issues

If you see build error 'Error The RGiesecke.DllExport.MSBuild.DllExportAppDomainIsolatedTask task could not be loaded from the assembly' then install ```Microsoft .NET Framework 3.5```.

## License

MIT
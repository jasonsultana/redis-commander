# redis-commander
A utility to manage your Redis keys

# setup
Aside from common tooling (eg: Visual Studio, .NET 6 SDK, etc), you'll need to install the electronize dotnet tool via: `dotnet tool install ElectronNET.CLI -g`.

# running in electron
Navigate to `Redis.Commander.Blazor` and run `electronize start`.

# packaging for distribution
```
electronize build /target win
electronize build /target osx
electronize build /target linux
```

# links
https://github.com/ElectronNET/Electron.NET

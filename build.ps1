# Build source generator
dotnet build -c Release ".\src\Vertical.CommandLine.SourceGenerators\Vertical.CommandLine.SourceGenerators.csproj"

# Clean
$nuget="$env:USERPROFILE/.nuget/packages"
Remove-Item -Path "$nuget/vertical-cli" -Force -Recurse

Remove-Item -Path .\src\Vertical.CommandLine\bin\Release\*.nupkg

# Pack library
dotnet pack -c Release ".\src\Vertical.CommandLine\Vertical.CommandLine.csproj"

# Push local package
$nupkg = (Get-ChildItem -Path .\src\Vertical.CommandLine\bin\Release\ -Filter "*-beta.nupkg").FullName
dotnet nuget push "$nupkg" -s "$nuget"
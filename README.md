# Play.Catalog
Play Economy Catalog microservice

## Create and publish package
```powershell

$version="1.0.3"
$owner="DotNetMicroservicesBasics"
$local_packages_path="D:\Dev\NugetPackages"
$gh_pat="PAT HERE"

dotnet pack src\Play.Catalog.Contracts --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/Play.Catalog -o $local_packages_path

dotnet nuget push $local_packages_path\Play.Catalog.Contracts.$version.nupkg --api-key $gh_pat --source github
```


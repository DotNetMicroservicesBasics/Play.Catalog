# Play.Catalog
Play Economy Catalog microservice

## Create and publish package
```powershell

$version="1.0.4"
$owner="DotNetMicroservicesBasics"
$local_packages_path="D:\Dev\NugetPackages"
$gh_pat="PAT HERE"

dotnet pack src\Play.Catalog.Contracts --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/Play.Catalog -o $local_packages_path

dotnet nuget push $local_packages_path\Play.Catalog.Contracts.$version.nupkg --api-key $gh_pat --source github
```

## Build the docker image
```powershell
$env:GH_OWNER="DotNetMicroservicesBasics"
$env:GH_PAT="[PAT HERE]"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t play.catalog:$version .
```

## Run the docker image on local machine
```powershell
docker run -it --rm -p 5169:5169 --name catalog -e MongoDbSettings__Host=mongo -e RabbitMqSettings__Host=rabbitmq --network playinfrastructure_default play.catalog:$version
```

## Run the docker image on Azure
```powershell
$cosmosDbConnectionString="[CONNECTION_STRING HERE]"
$serviceBusConnetionString="[CONNECTION_STRING HERE]"
$messageBroker="AZURESERVICEBUS"
docker run -it --rm -p 5169:5169 --name catalog -e MongoDbSettings__ConnectionString=$cosmosDbConnectionString -e ServiceSettings__MessageBroker=$messageBroker -e ServiceBusSettings__ConnectionString=$serviceBusConnetionString play.catalog:$version
```


## Publish the docker image on Azure
```powershell
$acrname="playeconomyazurecontainerregistry"
docker tag play.catalog:$version "$acrname.azurecr.io/play.catalog:$version"
az acr login --name $acrname
docker push "$acrname.azurecr.io/play.catalog:$version"
```
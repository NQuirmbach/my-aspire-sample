# Deployment

1. Install Azure Developer
   CLI [Link](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd?tabs=winget-windows%2Cbrew-mac%2Cscript-linux&pivots=os-mac)
2. Go into the AppHost and run `azd init`
3. Log into azure using `azd auth login`
4. Provision infrastructure and deployd via `azd up`

## Azure Developer CLI

The `azd up` command acts as wrapper for the following individual `azd` commands to provision and deploy your resources in a single step:

1. `azd package`: The app projects and their dependencies are packaged into containers.
2. `azd provision`: The Azure resources the app will need are provisioned.
3. `azd deploy`: The projects are pushed as containers into an Azure Container Registry instance, and then used to create new revisions of Azure Container Apps in which the code will be hosted.
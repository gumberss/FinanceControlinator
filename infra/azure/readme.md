az group create --name finance-controlinator --location eastus

 az acr create --resource-group finance-controlinator --name myContainerRegistry --sku Basic // talvez


## az aks create --resource-group finance-controlinator --name finance-controlinator-aks --node-count 1 --enable-addons monitoring --generate-ssh-keys --kubernetes-version 1.16.10

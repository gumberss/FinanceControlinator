

To run local, do not forget to run the command placed in this link:
https://kubernetes.github.io/ingress-nginx/deploy/


Add Rabbitmq:
Handling connection for 15672
```
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install finance-controlinator-rabbitmq bitnami/rabbitmq
```


# SonarQube:
- Run compose
- dotnet tool install --global dotnet-sonarscanner
- install sonar scanner: https://github.com/SonarSource/sonar-scanner-msbuild/releases/
	- ref: https://docs.sonarqube.org/latest/analysis/scan/sonarscanner-for-msbuild/
- Commands to get metrics:
```
	dotnet F:\Github\FinanceControlinator\infra\sonar\runner\SonarScanner.MSBuild.dll begin /k:"Financeinator" /d:sonar.login="sonar qube key"

dotnet build F:\Github\FinanceControlinator\FinanceControlinator.sln

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

dotnet F:\Github\FinanceControlinator\infra\sonar\runner\SonarScanner.MSBuild.dll end /d:sonar.login="sonar qube key" 

```

## Problems running SonarQube

- Memory:
``` run: sysctl -w vm.max_map_count=262144 ```

	If on Windows:
	1) wsl -d docker-desktop
	2) sysctl -w vm.max_map_count=262144
	ref: https://stackoverflow.com/questions/42111566/elasticsearch-in-windows-docker-image-vm-max-map-count



To run local, do not forget to run the command placed in this link:
https://kubernetes.github.io/ingress-nginx/deploy/


Add Rabbitmq:
Handling connection for 15672
```
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install finance-controlinator-rabbitmq bitnami/rabbitmq
GetPassword: $(kubectl get secret --namespace default finance-controlinator-rabbitmq -o jsonpath="{.data.rabbitmq-password}" | base64 --decode)"
```



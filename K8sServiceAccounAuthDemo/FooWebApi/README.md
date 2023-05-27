# Overview
A ASP.NET Core API application that reads the service account token from /var/run/secrets/kubernetes.io/serviceaccount/token directory
and invokes BarWebApi with access token in the **X-Client-Id** Http header.

 
## Docker build

```shell

cd C:\dev\gitrepos\k8s-service-account-auth-poc\K8sServiceAccounAuthDemo\FooWebApi

docker build -t k8sdemo/foowebapi:latest -f C:\dev\gitrepos\k8s-service-account-auth-poc\K8sServiceAccounAuthDemo\FooWebApi\Dockerfile C:\dev\gitrepos\k8s-service-account-auth-poc\K8sServiceAccounAuthDemo
```


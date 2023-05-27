# Overview
A demonstration applications to show the Authentication between microservices using Kubernetes identities

## Highlevel context overview
![image](https://github.com/madhub/k8s-service-account-auth-poc/assets/8907962/b42183db-0d8e-4afb-898e-bdba751484fb)


### Project structure
| ProjectName      | Description |
| ----------- | ----------- |
| FooWebApi      |  ASP.NET Core API app using kubernetes service account as a identity        |
| BarWebApi   | ASP.NET Core API authenticate  kubernetes service account set via HTTP header        |

### Build
#### Docker build
```shell
cd C:\dev\gitrepos\k8s-service-account-auth-poc\K8sServiceAccounAuthDemo\FooWebApi

docker build -t k8sdemo/foowebapi:latest -f C:\dev\gitrepos\k8s-service-account-auth-poc\K8sServiceAccounAuthDemo\FooWebApi\Dockerfile C:\dev\gitrepos\k8s-service-account-auth-poc\K8sServiceAccounAuthDemo
```

```shell
cd C:\dev\gitrepos\k8s-service-account-auth-poc\K8sServiceAccounAuthDemo\BarWebApi
docker build -t k8sdemo/barwebapi:latest -f C:\dev\gitrepos\k8s-service-account-auth-poc\K8sServiceAccounAuthDemo\BarWebApi\Dockerfile C:\dev\gitrepos\k8s-service-account-auth-poc\K8sServiceAccounAuthDemo
```
  
## Kubernetes deployment 
A Kubernetes deployment yaml also available to host open source mosquitto MQTT broker in Docker desktop with Kubernetes support
```shell
cd deployment
kubectl apply -f foo-deployment.yaml
kubectl apply -f bar-deployment.yaml
```
Browse to http://kubernetes.docker.internal/foo/swagger/index.html to see swagger API documentation , invoke API to see the response 










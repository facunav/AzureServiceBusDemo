# Azure Service Bus Demo – Sender.API + Worker

Este proyecto demuestra una arquitectura basada en colas utilizando **Azure Service Bus**. Está compuesto por dos aplicaciones:

- **Sender.API**: API ASP.NET Core que actúa como productor, enviando mensajes a la cola.
- **Worker**: aplicación de fondo que escucha la cola y procesa los mensajes entrantes.

## 🧩 Arquitectura

- **ASP.NET Core 8.0** – API REST para envío de mensajes
- **.NET Worker Service** – proceso background que consume mensajes
- **Azure Service Bus** – transporte de mensajes entre productor y consumidor
- **Azure Container Apps** – despliegue serverless y escalable
- **Docker** – empaquetado y ejecución de ambos servicios

## 🧪 Proyectos
/AzureServiceBusDemo
│
├── Sender.API # API REST con Swagger para enviar mensajes
├── Worker # Servicio de fondo que consume desde la cola
├── Shared # Biblioteca compartida entre proyectos


## 🚀 Endpoints de la API

- `POST /send` – Envía un mensaje a la cola (JSON)
- `GET /swagger` o `/` – Acceso a Swagger UI

🔗 **API desplegada en Azure (Swagger incluido)**:  
[https://sender-api-app.delightfuldesert-1760922d.brazilsouth.azurecontainerapps.io](https://sender-api-app.delightfuldesert-1760922d.brazilsouth.azurecontainerapps.io)

## 🐳 Docker

### Construcción de imágenes

```bash
# Desde raíz del repo
docker build -t sender-api -f Sender.API/Dockerfile .
docker build -t worker-app -f Worker/Dockerfile .

Ejecución local

docker run -p 8080:8080 sender-api
docker run worker-app

Luego podés abrir:
http://localhost:8080/swagger

☁️ Despliegue en Azure Container Apps
1. Crear una Container App para cada componente:
sender-api

worker-app (sin ingress público)

2. Subir imágenes a un Container Registry:
GitHub Container Registry (GHCR)

Azure Container Registry (ACR)

Docker Hub

3. Configurar:
Puerto 8080 para sender-api

Sin puerto expuesto para worker-app

Variables de entorno necesarias (por ejemplo, connection string de Azure Service Bus)

🧰 Requisitos
Docker

Cuenta de Azure + Azure CLI

.NET 8 SDK

Visual Studio o VS Code

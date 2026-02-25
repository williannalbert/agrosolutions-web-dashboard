
# 🌱 AgroSolutions - Web Dashboard

O **AgroSolutions Web Dashboard** é a interface de usuário (Front-end) da plataforma AgroSolutions. Desenvolvido como uma Single Page Application (SPA) utilizando **Blazor WebAssembly (.NET 10)** e **MudBlazor**, o painel permite que produtores rurais monitorem a telemetria de suas fazendas em tempo real através de gráficos interativos.

## 🚀 Tecnologias Utilizadas

* **Framework:** .NET 10 (Blazor WebAssembly)
* **UI/UX:** MudBlazor (Componentes Material Design)
* **Autenticação:** OIDC / JWT (Integração com Keycloak)
* **Servidor Web:** Nginx (Alpine)
* **Containerização:** Docker
* **Orquestração:** Kubernetes (Amazon EKS)
* **CI/CD:** GitHub Actions
* **Roteamento:** Ocelot API Gateway (Backend)

## ✨ Funcionalidades

* **🔐 Autenticação Segura:** Login centralizado via Keycloak (`CustomAuthStateProvider`) com interceptação automática de tokens (`UnauthorizedInterceptor`) para chamadas à API.
* **📍 Gestão de Propriedades:** Filtros em cascata dinâmicos (Fazenda ➔ Talhão ➔ Sensor).
* **📊 Monitoramento Gráfico:** Visualização avançada de telemetria baseada no tipo de sensor:
  * **Solo:** Umidade, pH e Nutrientes NPK (Nitrogênio, Fósforo, Potássio).
  * **Clima (Meteorológica):** Temperatura, Umidade, Velocidade do Vento, Chuva e Ponto de Orvalho.
  * **Silo:** Nível de Preenchimento, Temperatura Média e CO2 (com escala visual adaptada).
* **📅 Filtros de Período:** Consulta de dados históricos por intervalo de datas.

## 🏗️ Arquitetura e Comunicação

O Dashboard não se comunica diretamente com os microsserviços individuais. Toda a comunicação externa é roteada através do nosso **API Gateway (Ocelot)**, hospedado em `http://api.agrosolutions.site`. 

Os principais serviços consumidos são:
* `PropertiesService`: Consulta de Fazendas, Talhões e Sensores.
* `TelemetryApiService`: Consulta do histórico de telemetria no ElasticSearch/MongoDB.

## ⚙️ Como Executar Localmente

### Pré-requisitos
* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* Editor (Visual Studio 2022, VS Code)

### Passos
1. Clone este repositório:
`git clone [https://github.com/williannalbert/agrosolutions-web-dashboard.git]`
2. Acesse a pasta do projeto:
`cd agrosolutions-web-dashboard`
3. Restaure as dependências e execute:
`dotnet watch run`
4. O projeto abrirá no seu navegador (geralmente em `https://localhost:7153` ou `http://localhost:5241`).
`dotnet watch run`
## 🐳 Executando com Docker

O projeto inclui um `Dockerfile` multi-stage que compila a aplicação .NET e a serve utilizando um servidor **Nginx** leve, configurado (`nginx.conf`) para roteamento correto de SPAs.
`docker build -t agrosolutions-dashboard . 
docker run -d -p 8080:80 agrosolutions-dashboard`

Acesse `http://localhost:8080`
## 🚀 CI/CD e Deploy (Kubernetes)

O processo de deploy é **100% automatizado** via GitHub Actions.

**Fluxo de Publicação:**

1.  Todo o desenvolvimento ocorre na branch `dev`.
    
2.  Ao abrir um **Pull Request** da `dev` para a `main`, a action `.github/workflows/deploy.yml` é acionada.
    
3.  A esteira realiza o build da imagem Docker, faz o push para o **Amazon ECR** e aplica os manifestos na pasta `/k8s` (`deployment.yaml`, `service.yaml`, `ingress.yaml`) no cluster **Amazon EKS**.

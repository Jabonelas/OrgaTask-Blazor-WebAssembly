# OrgaTask Blazor WebAssembly

![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-%23512BD4)
![.NET](https://img.shields.io/badge/.NET-8-%23512BD4)
![JWT](https://img.shields.io/badge/JWT-Auth-%23000000)


## 🌐 **Sobre o OrgaTask**  
**Aplicativo Web** para gestão de tarefas com autenticação segura e sincronização em tempo real.

<p><em>Interface principal</em></p>

![image](https://github.com/user-attachments/assets/4f2376e8-6571-410e-8549-2f475137be38)

> Frontend web do ecossistema OrgaTask - Aplicação Blazor para gerenciamento de tarefas
 

## 📋 Visão Geral

Frontend Blazor WebAssembly que consome a OrgaTask API para:

- Autenticação JWT segura

- CRUD completo de tarefas (Criar/gerenciar tarefas com prioridades e status)

- Controle de prioridades (Baixa/Média/Alta)

- Sincronização em tempo real


## 🌐 Ecossistema OrgaTask
Esta aplicação consome:
- [OrgaTask API](https://github.com/Jabonelas/OrgaTask-API) (Backend principal)
  
<!--
Outros frontends do sistema:
- [OrgaTask Desktop](https://github.com/Jabonelas/OrgaTask-Windows-Forms) (Versão Desktop)
-->


![OrganizacaoOrgaTask](https://github.com/user-attachments/assets/bae20b56-ace7-4ef0-8d14-7fe13f1d9d31)
Figura 1: Visão geral da integração entre os componentes do OrgaTask.
A API central (Backend) serve dados para os frontends Web e Desktop.


## 🛠 Tecnologias

- **Core**: Blazor WASM (.NET 8)

- **Arquitetura MVVM Adaptada**

  - **Padrão customizado** otimizado para Blazor WASM:
  - **View**: Componentes Razor (`*.razor`)
  - **ViewModel**: Classes com estado/logica da UI (ex: `TaskViewModel.cs`)
  - **Service Layer**: Substitui o Model tradicional, lidando com:
    - Comunicação API (`HttpClient`)
    - Lógica de negócio
  - **Injeção de Dependência**: Nativa do .NET (IServiceCollection)

- **Comunicação**
  - **Consumo de API**: REST API (via HttpClient)
  - **Autenticação**: JWT (Bearer Token)
  - **Serialização JSON**

## 🚀 Como Executar
1. **Pré-requisitos**:
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - OrgaTask API em execução (siga o [README da API](https://github.com/Jabonelas/OrgaTask-API#-como-executar))

2. **Configuração**:
   ```bash
   git clone https://github.com/Jabonelas/OrgaTask-Blazor-WebAssembly.git
   cd OrgaTask-Blazor-WebAssembly

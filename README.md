# OrgaTask Blazor WebAssembly

![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-%23512BD4)
![.NET](https://img.shields.io/badge/.NET-8-%23512BD4)
![JWT](https://img.shields.io/badge/JWT-Auth-%23000000)


## 🌐 **Sobre o OrgaTask**  
**Aplicativo Web** para gestão de tarefas, onde usuários podem:  
- Criar/gerenciar tarefas com prioridades e status  
- Acesso rápido de qualquer navegador
- Ter dados sincronizados em tempo real  
 

> Frontend web do ecossistema OrgaTask - Aplicação Blazor para gerenciamento de tarefas


![image](https://github.com/user-attachments/assets/4f2376e8-6571-410e-8549-2f475137be38)



  <p><em>Interface principal</em></p>

## 📋 Visão Geral
Aplicação web que consome a **OrgaTask API** para:
- Autenticação segura de usuários via JWT
- Gerenciamento completo de tarefas (CRUD)
- Controle de prioridades (Baixa, Média, Alta)
- Filtros por status (Pendente, Em Progresso, Concluída)

## 🌐 Ecossistema OrgaTask
Esta aplicação consome:
- [OrgaTask API](https://github.com/Jabonelas/OrgaTask-API) (Backend principal)
  
<!--
Outros frontends do sistema:
- [OrgaTask Desktop](https://github.com/Jabonelas/OrgaTask-Windows-Forms) (Versão Desktop)
-->

- 📊 Arquitetura do Sistema

![OrganizacaoOrgaTask](https://github.com/user-attachments/assets/bae20b56-ace7-4ef0-8d14-7fe13f1d9d31)
Figura 1: Visão geral da integração entre os componentes do OrgaTask.
A API central (Backend) serve dados para os frontends Web e Desktop.


## 🛠 Tecnologias
- **Core**: Blazor WASM (.NET 8)
- **Autenticação**: JWT Bearer Tokens
- **Consumo de API**: REST (HttpClient)
- **Padrões Arquiteturais**:
  - **Service Layer**: Separação clara entre componentes UI e lógica de negócio
  - **Injeção de Dependência**: Nativa do .NET (IServiceCollection)

## 🚀 Como Executar
1. **Pré-requisitos**:
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - OrgaTask API em execução (siga o [README da API](https://github.com/Jabonelas/OrgaTask-API#-como-executar))

2. **Configuração**:
   ```bash
   git clone https://github.com/Jabonelas/OrgaTask-Blazor-WebAssembly.git
   cd OrgaTask-Blazor-WebAssembly

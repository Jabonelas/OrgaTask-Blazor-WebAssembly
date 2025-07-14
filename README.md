# OrgaTask Blazor WebAssembly

## Visão Geral

OrgaTask Blazor WebAssembly é uma aplicação web single-page (SPA) que consome a OrgaTask API para gerenciar tarefas de forma interativa. Construída com Blazor WASM, a aplicação utiliza uma arquitetura MVVM adaptada, proporcionando uma experiência de usuário fluida e responsiva.

## Tecnologias Utilizadas

- **Core:** Blazor WebAssembly (.NET 8)
- **Arquitetura:** MVVM Adaptada
  - View: Componentes Razor (*.razor)
  - ViewModel: Classes com estado e lógica da UI
  - Service Layer: Comunicação com API e lógica de negócio

- **Comunicação:**

  - Consumo de API REST via HttpClient
  - Autenticação com JWT Bearer Tokens
  - Serialização JSON
- **Injeção de Dependência:** Nativa do .NET (IServiceCollection)

## Funcionalidades

- Login e autenticação com JWT
- Gerenciamento de tarefas (listar, criar, editar, excluir)
- Interface responsiva e interativa
- Tratamento de erros e feedback visual
- Sincronização com API: Atualização em tempo real com a OrgaTask API.

## Pré-requisitos

- .NET 8 SDK
- OrgaTask API rodando localmente ou em um servidor
- Navegador moderno (Chrome, Firefox, Edge)

## Como Executar o Projeto

1. Clone o repositório:

```bash
git clone https://github.com/Jabonelas/OrgaTask-Blazor-WebAssembly.git
cd OrgaTask-Blazor-WebAssembly
```

2. Restaure as dependências:

```bash
dotnet restore
```

3. Configure a URL da API:

Edite o arquivo Program.cs para apontar para a URL da OrgaTask API. 
URL padrão: https://localhost:7170/ (modo desenvolvimento)

4. Execute a aplicação:

```bash
dotnet run
```

5. Acesse no navegador: https://localhost:7170/

## Exemplo de Uso

1. Acesse a página de login e insira credenciais válidas.

2. Após o login, visualize e gerencie suas tarefas na dashboard.

3. Use os formulários para criar ou editar tarefas.



<p><em>Interface Dashboard</em></p>

<img width="1497" height="1302" alt="image" src="https://github.com/user-attachments/assets/54b4db61-5f04-4b01-ae63-bbc9e7db12f2" />

> Painel visual com acompanhamento do progresso e status de todas as atividades

<p><em>Interface Tarefas</em></p>

<img width="1513" height="1299" alt="image" src="https://github.com/user-attachments/assets/7784932f-547f-4e31-8bd0-e1644051ba79" />

> Visualização integrada de todas as tarefas registradas

Contribuições

Contribuições são bem-vindas! Abra issues para relatar bugs ou sugerir melhorias, ou envie pull requests.

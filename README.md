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

![image](https://github.com/user-attachments/assets/3866f0ab-9589-4e93-ab15-4d6ab71e2290)
> Painel visual com acompanhamento do progresso e status de todas as atividades

<p><em>Interface Tarefas</em></p>

![image](https://github.com/user-attachments/assets/5f193776-f492-4dcb-981c-cdf942224c3a)
> Visualização integrada de todas as tarefas registradas

Contribuições

Contribuições são bem-vindas! Abra issues para relatar bugs ou sugerir melhorias, ou envie pull requests.

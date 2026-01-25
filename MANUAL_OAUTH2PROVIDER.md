# Manual de Usabilidade - OAuth2Provider

**Versão:** 1.0  
**Data:** 2024  
**Equipe:** Desenvolvimento

---

## Índice

1. [Introdução](#introdução)
2. [Pré-requisitos](#pré-requisitos)
3. [Iniciando a Aplicação](#iniciando-a-aplicação)
4. [Configuração Inicial](#configuração-inicial)
5. [Gerenciamento de Usuários](#gerenciamento-de-usuários)
6. [Gerenciamento de Clientes OAuth2](#gerenciamento-de-clientes-oauth2)
7. [Fluxo de Autenticação OAuth2](#fluxo-de-autenticação-oauth2)
8. [API REST - Endpoints Disponíveis](#api-rest---endpoints-disponíveis)
9. [Troubleshooting](#troubleshooting)
10. [Referências](#referências)

---

## Introdução

O **OAuth2Provider** é um servidor de autenticação OAuth 2.0 desenvolvido em C# .NET Framework, que permite gerenciar usuários e clientes OAuth2 de forma centralizada. Este sistema serve como provedor de autenticação para suas APIs e aplicações, garantindo segurança e controle de acesso através de tokens de autenticação.

### Objetivo do Manual

Este manual tem como objetivo orientar a equipe de desenvolvimento e administradores do sistema sobre como utilizar o OAuth2Provider, desde a inicialização até a integração com outras aplicações.

### Público-Alvo

- Desenvolvedores
- Administradores de Sistema
- Integradores de APIs
- Equipe de QA/Testes

---

## Pré-requisitos

Antes de iniciar o uso do OAuth2Provider, certifique-se de que os seguintes requisitos estão atendidos:

### Software Necessário

- **Visual Studio 2022 ou superior** (ou IDE compatível)
- **.NET Framework 4.7.2 ou superior**
- **MongoDB** (versão 4.4 ou superior)
- **Navegador Web** (Chrome, Firefox, Edge ou similar)
- **Ferramenta de Teste de API** (Postman, Insomnia ou similar) - opcional, mas recomendado

### Serviços em Execução

- **MongoDB** deve estar rodando e acessível
  - Se estiver usando Docker: `docker start mongodb`
  - Verifique se o MongoDB está respondendo na porta padrão `27017`

### Configurações

- String de conexão do MongoDB configurada no arquivo `Web.config`
- Porta do servidor web disponível (geralmente configurada automaticamente pelo Visual Studio)

---

## Iniciando a Aplicação

### Passo 1: Abrir o Projeto no Visual Studio

1. Abra o Visual Studio
2. Selecione **File > Open > Project/Solution**
3. Navegue até a pasta do projeto e abra o arquivo `OAuth2Provider.sln`
4. Aguarde o Visual Studio carregar todas as dependências

### Passo 2: Verificar Dependências

1. No **Solution Explorer**, expanda a pasta **Dependencies > Packages**
2. Verifique se todos os pacotes NuGet estão instalados corretamente
3. Se algum pacote estiver faltando, clique com o botão direito no projeto e selecione **Restore NuGet Packages**

### Passo 3: Compilar o Projeto

1. No menu, selecione **Build > Build Solution** (ou pressione `Ctrl + Shift + B`)
2. Verifique se não há erros de compilação na janela **Error List**
3. Se houver erros, corrija-os antes de prosseguir

### Passo 4: Iniciar a Aplicação

1. Certifique-se de que o projeto `OAuth2Provider` está definido como projeto de inicialização
2. Pressione `F5` ou clique no botão **Start** (ícone de play verde)
3. A aplicação será compilada e iniciada automaticamente
4. O navegador padrão será aberto automaticamente com a URL da aplicação (ex: `http://localhost:5000/` ou `http://localhost:44300/`)

### Passo 5: Verificar Inicialização

- Se a aplicação iniciou corretamente, você verá a página inicial ou será redirecionado para a tela de login
- Verifique o console do Visual Studio para confirmar que não há erros de inicialização
- Se houver erros relacionados ao MongoDB, verifique se o serviço está rodando

---

## Configuração Inicial

### Chaves RSA e JWKS (RS256)

Use o script para gerar o par de chaves fora do repositório:

```
.\tools\generate-rsa-keys.ps1 -OutputDir "C:\keys\oauth2"
```

Configure uma das opções abaixo (não armazenar chaves no repositório):

- Caminhos de arquivo:
  - `JWT_PRIVATE_KEY_PATH`
  - `JWT_PUBLIC_KEY_PATH`
- PEM em variáveis de ambiente (preferir secret manager):
  - `JWT_PRIVATE_KEY_PEM`
  - `JWT_PUBLIC_KEY_PEM`

JwtSettings obrigatórios:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:AccessTokenMinutes`
- `Jwt:KeyId` (mesmo `kid` do JWKS)

### Criando o Primeiro Usuário Administrador

Como o sistema requer autenticação para acessar o painel administrativo, é necessário criar o primeiro usuário administrador através da API REST, antes de poder utilizar a interface web.

#### Método 1: Usando Postman ou Insomnia (Recomendado)

1. Abra o Postman (ou Insomnia)
2. Crie uma nova requisição com as seguintes configurações:
   - **Método:** `POST`
   - **URL:** `http://localhost:[porta]/api/users`
     - Substitua `[porta]` pela porta onde sua aplicação está rodando
   - **Headers:**
     - `Content-Type: application/json`
   - **Body (raw JSON):**
     ```json
     {
       "UserName": "admin",
       "PasswordHash": "senha123",
       "Role": "admin"
     }
     ```
3. Clique em **Send**
4. Você deve receber uma resposta de sucesso (Status 200) com os dados do usuário criado

#### Método 2: Usando CURL (Linha de Comando)

Abra o PowerShell ou CMD e execute:

```bash
curl -X POST http://localhost:[porta]/api/users -H "Content-Type: application/json" -d "{\"UserName\":\"admin\",\"PasswordHash\":\"senha123\",\"Role\":\"admin\"}"
```

**Importante:** Substitua `[porta]` pela porta real da sua aplicação.

#### Método 3: Usando Navegador (Apenas para Verificação)

Para verificar se a API está funcionando, você pode acessar:
- `http://localhost:[porta]/api/users`
- Deve retornar uma lista JSON (vazia inicialmente, ou com os usuários já cadastrados)

### Verificando o Usuário Criado

Após criar o usuário, você pode verificar se foi criado corretamente:

1. Acesse: `http://localhost:[porta]/api/users`
2. Você deve ver o usuário "admin" na lista retornada

---

## Gerenciamento de Usuários

### Acessando o Painel Administrativo

1. No navegador, acesse: `http://localhost:[porta]/Auth/Login`
2. Você verá a tela de login com o logo da empresa
3. Digite suas credenciais:
   - **Nome de Usuário:** `admin` (ou o usuário que você criou)
   - **Senha:** `senha123` (ou a senha que você definiu)
4. Clique no botão **Entrar**
5. Você será redirecionado para o Dashboard administrativo

### Gerenciando Usuários via Interface Web

#### Visualizar Lista de Usuários

1. No Dashboard, clique no menu **Usuários**
2. Você verá uma tabela com todos os usuários cadastrados no sistema
3. A tabela exibe: Nome de Usuário, Role e opções de ação

#### Criar Novo Usuário

1. Na tela de Usuários, clique no botão **Novo Usuário**
2. Preencha o formulário:
   - **Usuário:** Nome de usuário único (ex: `usuario1`)
   - **Senha:** Senha do usuário (será criptografada automaticamente)
   - **Role:** Função do usuário (ex: `user`, `admin`)
3. Clique em **Salvar**
4. O usuário será criado e aparecerá na lista

#### Editar Usuário Existente

1. Na lista de usuários, localize o usuário que deseja editar
2. Clique no botão **Editar**
3. O formulário será preenchido com os dados atuais do usuário
4. Modifique os campos desejados
5. Clique em **Salvar** para aplicar as alterações

#### Remover Usuário

1. Na lista de usuários, localize o usuário que deseja remover
2. Clique no botão **Remover**
3. Confirme a exclusão na caixa de diálogo
4. O usuário será removido permanentemente do sistema

### Gerenciando Usuários via API REST

#### Listar Todos os Usuários

**Requisição:**
```
GET http://localhost:[porta]/api/users
```

**Resposta:**
```json
[
  {
    "Id": { "$oid": "..." },
    "UserName": "admin",
    "PasswordHash": "...",
    "Role": "admin"
  }
]
```

#### Obter Usuário Específico

**Requisição:**
```
GET http://localhost:[porta]/api/users/{id}
```

#### Criar Usuário

**Requisição:**
```
POST http://localhost:[porta]/api/users
Content-Type: application/json

{
  "UserName": "novousuario",
  "PasswordHash": "senha123",
  "Role": "user"
}
```

#### Atualizar Usuário

**Requisição:**
```
PUT http://localhost:[porta]/api/users/{id}
Content-Type: application/json

{
  "UserName": "novousuario",
  "PasswordHash": "novasenha",
  "Role": "admin"
}
```

#### Remover Usuário

**Requisição:**
```
DELETE http://localhost:[porta]/api/users/{id}
```

---

## Gerenciamento de Clientes OAuth2

### O que são Clientes OAuth2?

Clientes OAuth2 são aplicações ou serviços que estão autorizados a solicitar tokens de autenticação do OAuth2Provider. Cada cliente possui um `ClientId` e `ClientSecret` únicos, que são utilizados para autenticar as requisições de token.

### Gerenciando Clientes via Interface Web

#### Visualizar Lista de Clientes

1. No Dashboard, clique no menu **Clientes OAuth2**
2. Você verá uma tabela com todos os clientes cadastrados
3. A tabela exibe: ClientId, Nome e opções de ação

#### Criar Novo Cliente OAuth2

1. Na tela de Clientes, clique no botão **Novo Cliente**
2. Preencha o formulário:
   - **ClientId:** Identificador único do cliente (ex: `minhaapi`)
   - **ClientSecret:** Segredo do cliente (ex: `segredo123`)
   - **Nome:** Nome descritivo do cliente (ex: `Minha API de Produção`)
3. Clique em **Salvar**
4. O cliente será criado e aparecerá na lista

**Importante:** Anote o `ClientId` e `ClientSecret`, pois eles serão necessários para solicitar tokens OAuth2.

#### Editar Cliente Existente

1. Na lista de clientes, localize o cliente que deseja editar
2. Clique no botão **Editar**
3. Modifique os campos desejados
4. Clique em **Salvar** para aplicar as alterações

#### Remover Cliente

1. Na lista de clientes, localize o cliente que deseja remover
2. Clique no botão **Remover**
3. Confirme a exclusão
4. O cliente será removido permanentemente

### Gerenciando Clientes via API REST

#### Listar Todos os Clientes

**Requisição:**
```
GET http://localhost:[porta]/api/clients
```

#### Criar Cliente

**Requisição:**
```
POST http://localhost:[porta]/api/clients
Content-Type: application/json

{
  "ClientId": "minhaapi",
  "ClientSecret": "segredo123",
  "Name": "Minha API"
}
```

#### Atualizar Cliente

**Requisição:**
```
PUT http://localhost:[porta]/api/clients/{id}
Content-Type: application/json

{
  "ClientId": "minhaapi",
  "ClientSecret": "novosegredo",
  "Name": "Minha API Atualizada"
}
```

#### Remover Cliente

**Requisição:**
```
DELETE http://localhost:[porta]/api/clients/{id}
```

---

## Fluxo de Autenticação OAuth2

### Visão Geral

O OAuth2Provider implementa o fluxo **Resource Owner Password Credentials Grant**, onde o cliente solicita um token de acesso usando as credenciais do usuário (username e password).

### Passo 1: Obter Access Token

Para obter um token de acesso, o cliente deve fazer uma requisição ao endpoint `/token` com as credenciais do usuário e do cliente.

#### Requisição

**Método:** `POST`  
**URL:** `http://localhost:[porta]/token`  
**Headers:**
- `Content-Type: application/x-www-form-urlencoded`
- `Authorization: Basic [base64(clientId:clientSecret)]`

**Body (form-urlencoded):**
```
grant_type=password
username=[nome_do_usuario]
password=[senha_do_usuario]
```

#### Exemplo usando Postman

1. Crie uma nova requisição POST
2. URL: `http://localhost:[porta]/token`
3. Na aba **Authorization**, selecione **Basic Auth**
4. Preencha:
   - **Username:** `minhaapi` (ClientId)
   - **Password:** `segredo123` (ClientSecret)
5. Na aba **Body**, selecione **x-www-form-urlencoded**
6. Adicione os campos:
   - `grant_type`: `password`
   - `username`: `admin`
   - `password`: `senha123`
7. Clique em **Send**

#### Exemplo usando CURL

```bash
curl -X POST http://localhost:[porta]/token ^
  -H "Content-Type: application/x-www-form-urlencoded" ^
  -u "minhaapi:segredo123" ^
  -d "grant_type=password&username=admin&password=senha123"
```

#### Resposta de Sucesso

```json
{
  "access_token": "eyJ0eXAiOiJKV1QiLCJhbGc...",
  "token_type": "bearer",
  "expires_in": 7200
}
```

**Campos da Resposta:**
- `access_token`: Token JWT que deve ser usado nas requisições autenticadas
- `token_type`: Tipo do token (sempre "bearer")
- `expires_in`: Tempo de expiração em segundos (padrão: 7200 = 2 horas)

### Passo 2: Usar o Access Token

Após obter o token, você pode usá-lo para acessar endpoints protegidos do OAuth2Provider ou de outras APIs que aceitem este token.

#### Acessar Endpoint de Informações do Usuário

**Requisição:**
```
GET http://localhost:[porta]/api/userinfo
Authorization: Bearer [seu_access_token]
```

#### Exemplo usando Postman

1. Crie uma nova requisição GET
2. URL: `http://localhost:[porta]/api/userinfo`
3. Na aba **Authorization**, selecione **Bearer Token**
4. Cole o `access_token` obtido anteriormente
5. Clique em **Send**

#### Resposta

```json
{
  "username": "admin",
  "id": "admin",
  "role": "admin"
}
```

### Integrando com Outras Aplicações

Para integrar o OAuth2Provider com suas outras aplicações:

1. **Cadastre um Cliente OAuth2** no painel administrativo
2. **Obtenha o ClientId e ClientSecret** do cliente criado
3. **Na sua aplicação**, implemente o fluxo de obtenção de token
4. **Use o token** nas requisições para APIs protegidas

---

## API REST - Endpoints Disponíveis

### Endpoints de Autenticação

#### Obter Token OAuth2

```
POST /token
Content-Type: application/x-www-form-urlencoded
Authorization: Basic [base64(clientId:clientSecret)]

Body:
grant_type=password&username=[user]&password=[pass]
```

#### Obter Informações do Usuário Autenticado

```
GET /api/userinfo
Authorization: Bearer [access_token]
```

### Endpoints de Usuários

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/users` | Listar todos os usuários | Não requerida |
| GET | `/api/users/{id}` | Obter usuário específico | Não requerida |
| POST | `/api/users` | Criar novo usuário | Não requerida |
| PUT | `/api/users/{id}` | Atualizar usuário | Não requerida |
| DELETE | `/api/users/{id}` | Remover usuário | Não requerida |

### Endpoints de Clientes OAuth2

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/clients` | Listar todos os clientes | Não requerida |
| GET | `/api/clients/{id}` | Obter cliente específico | Não requerida |
| POST | `/api/clients` | Criar novo cliente | Não requerida |
| PUT | `/api/clients/{id}` | Atualizar cliente | Não requerida |
| DELETE | `/api/clients/{id}` | Remover cliente | Não requerida |

### Endpoints de Logs

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/logs` | Listar últimos 100 logs | Não requerida |

---

## Troubleshooting

### Problema: Erro ao iniciar a aplicação

**Sintomas:**
- Aplicação não inicia
- Erros no console do Visual Studio

**Soluções:**
1. Verifique se o MongoDB está rodando
2. Verifique a string de conexão no `Web.config`
3. Verifique se todas as dependências NuGet estão instaladas
4. Faça Clean e Rebuild do projeto

### Problema: Erro 404 nas rotas

**Sintomas:**
- Páginas não encontradas ao acessar rotas

**Soluções:**
1. Verifique se o projeto está configurado como Web Application
2. Verifique se o `WebApiConfig.cs` está configurado corretamente
3. Verifique se o `Startup.cs` está sendo executado

### Problema: Erro ao criar usuário via API

**Sintomas:**
- Requisição retorna erro 500
- Mensagem de erro relacionada ao MongoDB

**Soluções:**
1. Verifique se o MongoDB está acessível
2. Verifique se a string de conexão está correta
3. Verifique os logs da aplicação para mais detalhes

### Problema: Não consigo fazer login no dashboard

**Sintomas:**
- Mensagem "Usuário ou senha inválidos"
- Redirecionamento para login mesmo com credenciais corretas

**Soluções:**
1. Verifique se o usuário foi criado corretamente
2. Verifique se o usuário tem role "admin"
3. Verifique se a senha está correta (lembre-se que é a senha em texto puro, não o hash)
4. Verifique os logs da aplicação

### Problema: Erro ao obter token OAuth2

**Sintomas:**
- Requisição ao `/token` retorna erro
- Mensagem "invalid_client" ou "invalid_grant"

**Soluções:**
1. Verifique se o ClientId e ClientSecret estão corretos
2. Verifique se o cliente está cadastrado no sistema
3. Verifique se o username e password estão corretos
4. Verifique se o `grant_type` está como "password"

### Problema: Token expirado

**Sintomas:**
- Requisições com token retornam erro 401 (Unauthorized)

**Soluções:**
1. Obtenha um novo token através do endpoint `/token`
2. Tokens expiram após 2 horas (7200 segundos) por padrão
3. Implemente renovação automática de token na sua aplicação

### Problema: MongoDB não conecta

**Sintomas:**
- Erros de conexão ao MongoDB
- Timeout nas requisições

**Soluções:**
1. Verifique se o MongoDB está rodando: `docker ps` (se usar Docker)
2. Verifique a string de conexão no `Web.config`
3. Teste a conexão manualmente usando MongoDB Compass ou mongo shell
4. Verifique se o firewall não está bloqueando a porta 27017

---

## Referências

### Documentação Técnica

- **OAuth 2.0 Specification:** [RFC 6749](https://tools.ietf.org/html/rfc6749)
- **.NET Framework Documentation:** [Microsoft Docs](https://docs.microsoft.com/dotnet/framework/)
- **MongoDB Documentation:** [MongoDB Docs](https://docs.mongodb.com/)

### Ferramentas Recomendadas

- **Postman:** [https://www.postman.com/](https://www.postman.com/)
- **Insomnia:** [https://insomnia.rest/](https://insomnia.rest/)
- **MongoDB Compass:** [https://www.mongodb.com/products/compass](https://www.mongodb.com/products/compass)

### Suporte

Para dúvidas ou problemas, entre em contato com a equipe de desenvolvimento.

---

**Fim do Manual**

---

*Este documento foi gerado automaticamente. Última atualização: 2024*


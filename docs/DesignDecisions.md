# Design Decisions - OAuth2 Provider

## Visão geral
O projeto foi estruturado para operar como Authorization Server interno em ambiente corporativo, com foco em segurança de tokens, simplicidade operacional e escalabilidade horizontal para ecossistemas de microsserviços.

## Por que MongoDB
MongoDB foi adotado por aderência ao modelo de documentos usado para clients, códigos e tokens, reduzindo atrito de mapeamento e evolução de schema. A estratégia de expiração automática com TTL index simplifica a limpeza de authorization codes e refresh tokens sem jobs adicionais.

## Por que PKCE obrigatório
PKCE (`S256`) foi tornado obrigatório no Authorization Code para mitigar interception e authorization code leakage. Essa decisão reduz a superfície de ataque no fluxo de troca de code por token, mesmo em integrações internas.

## Por que JWT stateless
JWT stateless com RS256 permite validação distribuída sem consulta central a sessão, favorecendo escalabilidade horizontal e baixo acoplamento entre serviços. A publicação da chave pública via JWKS padroniza a confiança entre emissores e consumidores.

## O que foi explicitamente evitado
- Sessões server-side para tokens de acesso.
- OAuth Password Grant.
- Assinatura de access token com chaves simétricas compartilhadas.
- Dependência de provedores externos como IdP para o cenário principal interno.

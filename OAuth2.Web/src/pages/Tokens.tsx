function Tokens() {
  return (
    <div className="content-placeholder">
      <h2>Tokens</h2>

      <section>
        <h3>Client Credentials</h3>
        <p>Fluxo para aplicações de serviço sem usuário.</p>
        <pre>{`curl -X POST https://seu-host/oauth/token \\
  -H "Content-Type: application/x-www-form-urlencoded" \\
  -d "grant_type=client_credentials&client_id=CLIENT_ID&client_secret=CLIENT_SECRET&scope=api.read"`}</pre>
      </section>

      <section>
        <h3>Authorization Code + PKCE</h3>
        <ol>
          <li>Gerar code_verifier e code_challenge (S256).</li>
          <li>Chamar /oauth/authorize para obter o code.</li>
          <li>Trocar o code por token em /oauth/token com code_verifier.</li>
        </ol>
      </section>

      <section>
        <h3>JWKS</h3>
        <p>Endpoint público:</p>
        <pre>{`/.well-known/jwks.json`}</pre>
      </section>

      <section>
        <h3>Observações</h3>
        <ul>
          <li>client_secret aparece apenas uma vez no momento da criação.</li>
          <li>access_token expira em poucos minutos.</li>
          <li>refresh_token disponível quando aplicável.</li>
          <li>clients são gerenciados via UI.</li>
        </ul>
      </section>
    </div>
  )
}

export default Tokens

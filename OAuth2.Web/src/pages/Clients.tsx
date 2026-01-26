import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'
import { useAuth } from '../auth/AuthContext'

type ClientItem = {
  clientId: string
  scopes?: string[]
  redirectUris?: string[]
}

function parseList(value: string) {
  return value
    .split(/[\s\r\n]+/)
    .map((item) => item.trim())
    .filter(Boolean)
}

function Clients() {
  const { token } = useAuth()
  const [clients, setClients] = useState<ClientItem[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [scopesInput, setScopesInput] = useState('')
  const [redirectUrisInput, setRedirectUrisInput] = useState('')

  const loadClients = async () => {
    setLoading(true)
    setError(null)
    try {
      const data = await apiRequest<ClientItem[]>('/clients', { token })
      setClients(data)
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Falha ao carregar'
      setError(message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    void loadClients()
  }, [])

  const handleCreate = async (event: React.FormEvent) => {
    event.preventDefault()
    setError(null)

    try {
      const response = await apiRequest<Record<string, unknown>>('/clients', {
        method: 'POST',
        token,
        body: {
          scopes: parseList(scopesInput),
          redirectUris: parseList(redirectUrisInput)
        }
      })

      const secret =
        (response.clientSecret as string | undefined) ??
        (response.client_secret as string | undefined)

      if (secret) {
        alert(`Client secret: ${secret}`)
      } else {
        alert('Client criado com sucesso.')
      }

      setScopesInput('')
      setRedirectUrisInput('')
      await loadClients()
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Falha ao criar'
      setError(message)
    }
  }

  return (
    <div className="content-placeholder">
      <h2>Clients</h2>

      <form onSubmit={handleCreate}>
        <div>
          <label>
            Scopes
            <textarea
              rows={3}
              value={scopesInput}
              onChange={(event) => setScopesInput(event.target.value)}
              placeholder="api.read api.write"
            />
          </label>
        </div>
        <div>
          <label>
            Redirect URIs
            <textarea
              rows={3}
              value={redirectUrisInput}
              onChange={(event) => setRedirectUrisInput(event.target.value)}
              placeholder="https://app.exemplo.com/callback"
            />
          </label>
        </div>
        <button type="submit">Novo Client</button>
      </form>

      {loading ? <p>Carregando...</p> : null}
      {error ? <p>{error}</p> : null}

      <table>
        <thead>
          <tr>
            <th>Client ID</th>
            <th>Scopes</th>
            <th>Redirect URIs</th>
          </tr>
        </thead>
        <tbody>
          {clients.length === 0 ? (
            <tr>
              <td colSpan={3}>Nenhum client cadastrado.</td>
            </tr>
          ) : (
            clients.map((client) => (
              <tr key={client.clientId}>
                <td>{client.clientId}</td>
                <td>{(client.scopes ?? []).join(', ')}</td>
                <td>{(client.redirectUris ?? []).join(', ')}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  )
}

export default Clients

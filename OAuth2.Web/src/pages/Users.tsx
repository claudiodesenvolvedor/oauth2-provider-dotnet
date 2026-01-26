import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'
import { useAuth } from '../auth/AuthContext'

type UserItem = {
  id: string
  email: string
  isActive: boolean
}

function Users() {
  const { token } = useAuth()
  const [users, setUsers] = useState<UserItem[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')

  const loadUsers = async () => {
    setLoading(true)
    setError(null)
    try {
      const data = await apiRequest<UserItem[]>('/users', { token })
      setUsers(data)
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Falha ao carregar'
      setError(message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    void loadUsers()
  }, [])

  const handleCreate = async (event: React.FormEvent) => {
    event.preventDefault()
    setError(null)

    try {
      await apiRequest('/users', {
        method: 'POST',
        token,
        body: {
          email,
          password
        }
      })

      setEmail('')
      setPassword('')
      await loadUsers()
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Falha ao criar'
      setError(message)
    }
  }

  const handleToggle = async (user: UserItem) => {
    setError(null)
    try {
      await apiRequest(`/users/${user.id}`, {
        method: 'PUT',
        token,
        body: {
          isActive: !user.isActive
        }
      })
      await loadUsers()
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Falha ao atualizar'
      setError(message)
    }
  }

  return (
    <div className="content-placeholder">
      <h2>Users</h2>

      <form onSubmit={handleCreate}>
        <div>
          <label>
            Email
            <input
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              placeholder="usuario@local"
              required
            />
          </label>
        </div>
        <div>
          <label>
            Senha
            <input
              type="password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              placeholder="••••••••"
              required
            />
          </label>
        </div>
        <button type="submit">Novo Usuário</button>
      </form>

      {loading ? <p>Carregando...</p> : null}
      {error ? <p>{error}</p> : null}

      <table>
        <thead>
          <tr>
            <th>Email</th>
            <th>Ativo</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {users.length === 0 ? (
            <tr>
              <td colSpan={3}>Nenhum usuário cadastrado.</td>
            </tr>
          ) : (
            users.map((user) => (
              <tr key={user.id}>
                <td>{user.email}</td>
                <td>{user.isActive ? 'Sim' : 'Não'}</td>
                <td>
                  <button type="button" onClick={() => handleToggle(user)}>
                    {user.isActive ? 'Desativar' : 'Ativar'}
                  </button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  )
}

export default Users

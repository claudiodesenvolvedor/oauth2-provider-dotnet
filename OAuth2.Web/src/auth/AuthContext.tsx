import { createContext, useCallback, useContext, useMemo, useState } from 'react'

type AuthContextValue = {
  token: string | null
  login: (token: string) => void
  logout: () => void
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined)

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [token, setToken] = useState<string | null>(() => {
    return localStorage.getItem('auth_token')
  })

  const login = useCallback((value: string) => {
    setToken(value)
    localStorage.setItem('auth_token', value)
  }, [])

  const logout = useCallback(() => {
    setToken(null)
    localStorage.removeItem('auth_token')
  }, [])

  const value = useMemo(
    () => ({
      token,
      login,
      logout
    }),
    [token, login, logout]
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider')
  }
  return context
}

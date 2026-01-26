import { useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'

interface HeaderProps {
  onMenuClick: () => void
}

function Header({ onMenuClick }: HeaderProps) {
  const { logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login', { replace: true })
  }

  return (
    <header className="header">
      <button
        className="hamburger"
        type="button"
        aria-label="Abrir menu"
        onClick={onMenuClick}
      >
        <span />
        <span />
        <span />
      </button>
      <div className="brand">OAuth2 Admin</div>
      <button className="header__logout" type="button" onClick={handleLogout}>
        Sair
      </button>
    </header>
  )
}

export default Header

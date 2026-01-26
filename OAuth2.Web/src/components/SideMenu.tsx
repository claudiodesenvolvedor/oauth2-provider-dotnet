import { NavLink } from 'react-router-dom'

interface SideMenuProps {
  open: boolean
  onClose: () => void
}

const items = [
  { label: 'Administração', to: '/users' },
  { label: 'Previsão', to: '/dashboard' },
  { label: 'Importação', to: '/clients' },
  { label: 'Relatórios', to: '/tokens' },
  { label: 'Alterações Materiais', to: '/clients' },
  { label: 'Ajuda', to: '/dashboard' }
]

function SideMenu({ open, onClose }: SideMenuProps) {
  return (
    <>
      {open ? <div className="side-menu__backdrop" onClick={onClose} /> : null}
      <aside className={`side-menu ${open ? 'side-menu--open' : ''}`}>
        <nav>
          <ul className="side-menu__list">
            {items.map((item) => (
              <li key={item.label}>
                <NavLink
                  className="side-menu__item"
                  to={item.to}
                  onClick={onClose}
                >
                  {item.label}
                </NavLink>
              </li>
            ))}
          </ul>
        </nav>
      </aside>
    </>
  )
}

export default SideMenu

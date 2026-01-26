import { Outlet } from 'react-router-dom'
import { useState } from 'react'
import Header from './Header'
import Footer from './Footer'
import SideMenu from './SideMenu'

function Layout() {
  const [menuOpen, setMenuOpen] = useState(false)

  return (
    <div className="app-shell">
      <Header onMenuClick={() => setMenuOpen((open) => !open)} />
      <SideMenu open={menuOpen} onClose={() => setMenuOpen(false)} />
      <main className="content">
        <Outlet />
      </main>
      <Footer />
    </div>
  )
}

export default Layout

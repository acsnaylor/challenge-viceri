import { Link, NavLink, Outlet } from 'react-router-dom';

export function AppLayout() {
  return (
    <div className="flex min-h-screen flex-col">
      <header className="bg-gray-900 text-white">
        <div className="container mx-auto flex items-center justify-between px-4 py-3">
          <Link to="/" className="font-bold text-white">Super Heróis</Link>
          <nav className="flex gap-4">
            <NavLink to="/heroes" className={({isActive}) => (isActive ? 'text-white' : 'text-gray-300') + ' hover:text-white'}>Heróis</NavLink>
            <NavLink to="/heroes/new" className={({isActive}) => (isActive ? 'text-white' : 'text-gray-300') + ' hover:text-white'}>Novo</NavLink>
          </nav>
        </div>
      </header>
      <main className="container mx-auto px-4 py-6">
        <Outlet />
      </main>
      <footer className="mt-auto py-4 text-center text-sm text-gray-500">
        <div className="container mx-auto px-4">Powered by React + Vite</div>
      </footer>
    </div>
  );
}

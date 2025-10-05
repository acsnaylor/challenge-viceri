import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { AppLayout } from '../components/layout/AppLayout'
import { HeroesListPage } from '../pages/Heroes/HeroesListPage'
import { HeroFormPage } from '../pages/Heroes/HeroFormPage'
import { NotFoundPage } from '../pages/NotFoundPage'
import { HomePage } from '../pages/Home/HomePage'
import '../styles/globals.css'
import { ToastProvider } from '@components/ui/toast'

export function App() {
  return (
    <ToastProvider>
      <BrowserRouter>
        <Routes>
          <Route path='/' element={<AppLayout />}>
            <Route index element={<HomePage />} />
            <Route path='heroes' element={<HeroesListPage />} />
            <Route
              path='heroes/new'
              element={<HeroFormPage onBack={() => window.history.back()} />}
            />
            <Route
              path='heroes/:id/edit'
              element={<HeroFormPage onBack={() => window.history.back()} />}
            />
            <Route path='*' element={<NotFoundPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </ToastProvider>
  )
}

export default App

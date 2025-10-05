import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AppLayout } from '../components/layout/AppLayout';
import { HeroesListPage } from '../pages/Heroes/HeroesListPage';
import { HeroFormPage } from '../pages/Heroes/HeroFormPage';
import { HeroDetailsPage } from '../pages/Heroes/HeroDetailsPage';
import { NotFoundPage } from '../pages/NotFoundPage';
import '../styles/globals.css';

export function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AppLayout />}> 
          <Route index element={<Navigate to="/heroes" replace />} />
          <Route path="heroes" element={<HeroesListPage />} />
          <Route path="heroes/new" element={<HeroFormPage mode="create" />} />
          <Route path="heroes/:id" element={<HeroDetailsPage />} />
          <Route path="heroes/:id/edit" element={<HeroFormPage mode="edit" />} />
          <Route path="*" element={<NotFoundPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;


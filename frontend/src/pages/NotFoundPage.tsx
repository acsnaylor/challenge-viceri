import { Link } from 'react-router-dom'
import { Compass, HomeIcon, Zap } from 'lucide-react'

export function NotFoundPage() {
  return (
    <div className='min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-red-900'>
      <div className='relative z-10 container mx-auto px-4 py-16'>
        <div className='mx-auto max-w-md rounded-lg bg-white/10 p-8 text-center backdrop-blur-sm'>
          <Compass className='mx-auto mb-4 h-16 w-16 text-yellow-400' />
          <h1 className='mb-2 bg-gradient-to-r from-yellow-400 via-red-500 to-blue-500 bg-clip-text text-3xl text-transparent md:text-4xl'>
            Página não encontrada
          </h1>
          <p className='mb-6 text-gray-200'>
            A rota que você tentou acessar não existe ou foi movida. Use as ações abaixo para continuar navegando.
          </p>

          <div className='flex flex-col items-center justify-center gap-3 sm:flex-row'>
            <Link
              to='/'
              className='inline-flex items-center rounded-md border-0 bg-gradient-to-r from-green-600 to-blue-600 px-4 py-2 text-white hover:from-green-700 hover:to-blue-700'
            >
              <HomeIcon className='mr-2 h-4 w-4' /> Ir para Home
            </Link>

            <Link
              to='/heroes'
              className='inline-flex items-center rounded-md border-0 bg-gradient-to-r from-purple-600 to-pink-600 px-4 py-2 text-white hover:from-purple-700 hover:to-pink-700'
            >
              <Zap className='mr-2 h-4 w-4' /> Ver Heróis
            </Link>
          </div>
        </div>
      </div>
    </div>
  )
}


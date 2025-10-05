import { ImageWithFallback } from '@components/figma/ImageWithFallback'
import { Plus, Users, Zap } from 'lucide-react'
import { Button } from '@components/ui/button'
import { Card, CardContent } from '@components/ui/card'

export function HomePage() {
  return (
    <div className='min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-red-900'>
      <div className='relative z-10 container mx-auto px-4 py-8'>
        <div className='text-center mb-12'>
          <div className='flex items-center justify-center gap-3 mb-4'>
            <Zap className='h-10 w-10 text-yellow-400' />
            <h1 className='text-white text-4xl md:text-6xl font-bold bg-gradient-to-r from-yellow-400 via-red-500 to-blue-500 bg-clip-text text-transparent'>
              HERO REGISTRY
            </h1>
            <Zap className='h-10 w-10 text-yellow-400' />
          </div>
          <p className='text-gray-200 text-lg md:text-xl max-w-2xl mx-auto'>
            O seu banco de dados de super-heróis! Crie e gerencie novos heróis e
            explore as lendas que protegem nossa realidade.
          </p>
        </div>

        <div className='grid md:grid-cols-2 gap-8 max-w-4xl mx-auto'>
          <Card className='group cursor-pointer transform transition-all duration-300 hover:scale-105 hover:shadow-2xl bg-gradient-to-br from-red-600 to-orange-600 border-0 overflow-hidden'>
            <CardContent className='p-0'>
              <div className='relative h-64 overflow-hidden'>
                <ImageWithFallback
                  src='https://static.dc.com/2025-01/SoS_Publishing%20Banner_0.jpeg'
                  alt='Create Hero'
                  className='w-full h-full object-cover group-hover:scale-110 transition-transform duration-300'
                />
                <div className='absolute inset-0 bg-gradient-to-t from-black/70 via-transparent to-transparent' />

                <div className='absolute top-4 right-4'>
                  <div className='bg-white/20 backdrop-blur-sm rounded-full p-3'>
                    <Plus className='h-8 w-8 text-white' />
                  </div>
                </div>
              </div>

              <div className='p-6'>
                <h3 className='text-white text-2xl mb-3'>Crie Seu Herói</h3>
                <p className='text-white/90 mb-6'>
                  Forge uma nova lenda! Crie um super-herói único com poderes
                  extraordinários e uma história épica.
                </p>
                <Button
                  className='w-full bg-white text-red-600 hover:bg-gray-100 transition-colors'
                  size='lg'
                  onClick={() => {
                    window.location.href = '/heroes/new'
                  }}
                >
                  <Plus className='mr-2 h-5 w-5' />
                  Criar Herói
                </Button>
              </div>
            </CardContent>
          </Card>

          <Card className='group cursor-pointer transform transition-all duration-300 hover:scale-105 hover:shadow-2xl bg-gradient-to-br from-blue-600 to-purple-600 border-0 overflow-hidden'>
            <CardContent className='p-0'>
              <div className='relative h-64 overflow-hidden'>
                <ImageWithFallback
                  src='https://assets.newsweek.com/wp-content/uploads/2025/08/1570070-legion-superheroes.jpg'
                  alt='Heroes List'
                  className='w-full h-full object-cover group-hover:scale-110 transition-transform duration-300'
                />
                <div className='absolute inset-0 bg-gradient-to-t from-black/70 via-transparent to-transparent' />

                <div className='absolute top-4 right-4'>
                  <div className='bg-white/20 backdrop-blur-sm rounded-full p-3'>
                    <Users className='h-8 w-8 text-white' />
                  </div>
                </div>
              </div>

              <div className='p-6'>
                <h3 className='text-white text-2xl mb-3'>Listagem de Heróis</h3>
                <p className='text-white/90 mb-6'>
                  Explore o arquivo completo de heróis registrados. Descubra
                  suas origens, poderes e feitos heroicos.
                </p>
                <Button
                  className='w-full bg-white text-blue-600 hover:bg-gray-100 transition-colors'
                  size='lg'
                  onClick={() => {
                    window.location.href = '/heroes'
                  }}
                >
                  <Users className='mr-2 h-5 w-5' />
                  Ver Heróis
                </Button>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  )
}

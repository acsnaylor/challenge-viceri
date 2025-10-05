import { useEffect, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { Card, CardContent, CardHeader } from '@components/ui/card'
import { Button } from '@components/ui/button'
import { Badge } from '@components/ui/badge'
import { Edit, Trash2, Zap, Plus, HomeIcon } from 'lucide-react'
import { deleteHero, getHeroes } from '../../services/heroes'
import { ApiResponse, HeroResponse } from '../../services/types'
import { format } from 'date-fns'

export function HeroesListPage() {
  const [loading, setLoading] = useState(true)
  const [data, setData] = useState<ApiResponse<HeroResponse[]> | null>(null)
  const navigate = useNavigate()

  async function load() {
    setLoading(true)
    try {
      const res = await getHeroes()
      setData(res)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    load()
  }, [])

  async function onDelete(id: number, heroName: string) {
    if (!confirm(`Tem certeza que deseja excluir o herói ${heroName}?`)) return
    await deleteHero(id)
    await load()
  }

  return (
    <div className='min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-red-900'>
      <div className='relative z-10 container mx-auto px-4 py-8'>
        <div className='mb-8 flex items-center justify-between'>
          <div className='flex items-center gap-4'>
            <Button
              onClick={() => (window.location.href = '/')}
              variant='outline'
              size='icon'
              className='border-white/20 bg-white/10 text-white hover:bg-white/20'
            >
              <HomeIcon className='h-4 w-4' />
            </Button>
            <div className='flex items-center gap-3'>
              <Zap className='h-8 w-8 text-yellow-400' />
              <h1 className='bg-gradient-to-r from-yellow-400 via-red-500 to-blue-500 bg-clip-text text-3xl text-transparent md:text-4xl'>
                Heróis Registrados
              </h1>
            </div>
          </div>
          <Link
            to='/heroes/new'
            className='inline-flex items-center rounded-md border-0 bg-gradient-to-r from-green-600 to-blue-600 px-3 py-2 text-white hover:from-green-700 hover:to-blue-700'
          >
            <Plus className='mr-2 h-4 w-4' /> Novo Herói
          </Link>
        </div>

        <div className='mb-8'>
          <div className='max-w-xs rounded-lg bg-white/10 p-4 backdrop-blur-sm'>
            <p className='text-lg text-white'>
              <span className='bg-gradient-to-r from-yellow-400 to-orange-500 bg-clip-text text-transparent'>
                {data?.result?.length ?? 0}
              </span>{' '}
              {(data?.result?.length ?? 0) === 1
                ? 'herói registrado'
                : 'heróis registrados'}
            </p>
          </div>
        </div>

        {loading ? (
          <div className='py-16 text-center text-gray-200'>Carregando...</div>
        ) : !data || !data.result || data.result.length === 0 ? (
          <div className='py-16 text-center'>
            <div className='mx-auto max-w-md rounded-lg bg-white/10 p-8 backdrop-blur-sm'>
              <Zap className='mx-auto mb-4 h-16 w-16 text-yellow-400' />
              <h3 className='mb-2 text-xl text-white'>
                Nenhum herói cadastrado
              </h3>
              <p className='mb-6 text-gray-300'>
                Comece criando seu primeiro super-herói!
              </p>
              <Link
                to='/heroes/new'
                className='inline-flex items-center rounded-md border-0 bg-gradient-to-r from-green-600 to-blue-600 px-3 py-2 text-white hover:from-green-700 hover:to-blue-700'
              >
                <Plus className='mr-2 h-4 w-4' /> Criar Primeiro Herói
              </Link>
            </div>
          </div>
        ) : (
          <div className='grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3'>
            {data.result.map((h) => (
              <Card
                key={h.id}
                className='transform border-white/20 bg-white/10 backdrop-blur-sm transition-all duration-300 hover:scale-105 hover:bg-white/15'
              >
                <CardHeader className='pb-3'>
                  <div className='flex items-start justify-between'>
                    <div>
                      <h3 className='mb-1 text-xl text-white'>{h.heroName}</h3>
                      <p className='text-sm text-gray-300'>{h.name}</p>
                    </div>
                    <div className='flex gap-2'>
                      <Button
                        onClick={() => navigate(`/heroes/${h.id}/edit`)}
                        size='icon'
                        variant='outline'
                        className='h-8 w-8 border-white/20 bg-white/10 text-white hover:border-blue-600 hover:bg-blue-600'
                      >
                        <Edit className='h-4 w-4' />
                      </Button>
                      <Button
                        onClick={() => onDelete(h.id, h.heroName)}
                        size='icon'
                        variant='outline'
                        className='h-8 w-8 border-white/20 bg-white/10 text-white hover:border-red-600 hover:bg-red-600'
                      >
                        <Trash2 className='h-4 w-4' />
                      </Button>
                    </div>
                  </div>
                </CardHeader>
                <CardContent className='space-y-4'>
                  <div className='text-sm'>
                    <p className='text-gray-400'>Data de Nascimento</p>
                    <p className='text-white'>
                      {h.birthDate
                        ? format(new Date(h.birthDate), 'dd/MM/yyyy')
                        : '-'}
                    </p>
                  </div>
                  <div className='grid grid-cols-2 gap-4 text-sm'>
                    <div>
                      <p className='text-gray-400'>Altura</p>
                      <p className='text-white'>{h.height} m</p>
                    </div>
                    <div>
                      <p className='text-gray-400'>Peso</p>
                      <p className='text-white'>{h.weight} kg</p>
                    </div>
                  </div>
                  <div>
                    <p className='mb-2 text-sm text-gray-400'>Superpoderes</p>
                    <div className='flex flex-wrap gap-1'>
                      {(h.superpowers || []).slice(0, 3).map((p) => (
                        <Badge
                          key={p.id}
                          variant='secondary'
                          className='bg-gradient-to-r from-yellow-500 to-orange-500 text-xs text-white'
                        >
                          {p.name}
                        </Badge>
                      ))}
                      {(h.superpowers || []).length > 3 && (
                        <Badge
                          variant='secondary'
                          className='bg-gray-600 text-xs text-white'
                        >
                          +{(h.superpowers || []).length - 3}
                        </Badge>
                      )}
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}

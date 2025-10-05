import { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { Input } from '@components/ui/input'
import { Label } from '@components/ui/label'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@components/ui/select'
import { Badge } from '@components/ui/badge'
import { ArrowLeft, Save, Zap, X } from 'lucide-react'
import { Button } from '@components/ui/button'
import { Card, CardContent, CardHeader } from '@components/ui/card'
import { useNavigate, useParams } from 'react-router-dom'
import { HeroRequest, SuperpowerResponse } from '@services/types'
import { getSuperpowers } from '@services/superpowers'
import { createHero, getHero, updateHero } from '@services/heroes'
import { useToast } from '@components/ui/toast'

type HeroFormData = {
  name: string
  heroName: string
  birthDate: string
  height: string
  weight: string
  superpowerIds: number[]
}

type HeroFormPageProps = { onBack: () => void }

export function HeroFormPage({ onBack }: HeroFormPageProps) {
  const { id } = useParams()
  const isEdit = Boolean(id)
  const navigate = useNavigate()
  const { show } = useToast()

  const [superpowers, setSuperpowers] = useState<SuperpowerResponse[]>([])
  const [selectedPowers, setSelectedPowers] = useState<number[]>([])
  const [_, setLoading] = useState(true)

  const {
    register,
    handleSubmit,
    formState: { errors },
    setValue,
  } = useForm<HeroFormData>({
    defaultValues: {
      name: '',
      heroName: '',
      birthDate: '',
      height: '',
      weight: '',
      superpowerIds: [],
    },
  })

  const handleAddPower = (powerId: number) => {
    if (!selectedPowers.includes(powerId)) {
      const next = [...selectedPowers, powerId]
      setSelectedPowers(next)
      setValue('superpowerIds', next)
    }
  }

  const handleRemovePower = (powerId: number) => {
    const next = selectedPowers.filter((idp) => idp !== powerId)
    setSelectedPowers(next)
    setValue('superpowerIds', next)
  }

  const onSubmit = async (data: HeroFormData) => {
    const payload: HeroRequest = {
      name: data.name,
      heroName: data.heroName,
      birthDate: new Date(data.birthDate).toISOString(),
      height: parseFloat(data.height.replace(',', '.')),
      weight: parseFloat(data.weight.replace(',', '.')),
      superpowerIds: selectedPowers,
    }
    try {
      if (isEdit && id) await updateHero(Number(id), payload)
      else await createHero(payload)
      navigate('/heroes')
    } catch (err: any) {
      const api = err?.response?.data as any
      const errorsObj = api?.errors as Record<string, string> | undefined
      const validationMsg = errorsObj?.Validation || (errorsObj ? Object.values(errorsObj)[0] : undefined)
      const msg = validationMsg || api?.message || 'Ocorreu um erro ao salvar.'
      show(msg, { variant: 'error' })
    }
  }

  useEffect(() => {
    async function load() {
      setLoading(true)
      try {
        const powersRes = await getSuperpowers()
        setSuperpowers(powersRes.result || [])
        if (isEdit && id) {
          const heroRes = await getHero(Number(id))
          const h = heroRes.result!
          setValue('name', h.name)
          setValue('heroName', h.heroName)
          setValue('birthDate', h.birthDate?.substring(0, 10) || '')
          setValue('height', String(h.height))
          setValue('weight', String(h.weight))
          const ids = h.superpowers?.map((s) => s.id) || []
          setSelectedPowers(ids)
          setValue('superpowerIds', ids)
        }
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [id, isEdit, setValue])

  return (
    <div className='min-h-screen bg-gradient-to-br from-blue-900 via-purple-900 to-red-900'>
      <div className='relative z-10 container mx-auto px-4 py-8'>
        <div className='mb-8 flex items-center gap-4'>
          <Button
            onClick={onBack}
            variant='outline'
            size='icon'
            className='border-white/20 bg-white/10 text-white hover:bg-white/20'
          >
            <ArrowLeft className='h-4 w-4' />
          </Button>
          <div className='flex items-center gap-3'>
            <Zap className='h-8 w-8 text-yellow-400' />
            <h1 className='bg-gradient-to-r from-yellow-400 via-red-500 to-blue-500 bg-clip-text text-3xl text-transparent md:text-4xl'>
              {isEdit ? 'Editar Herói' : 'Criar Novo Herói'}
            </h1>
          </div>
        </div>

        <Card className='mx-auto max-w-2xl border-white/20 bg-white/10 backdrop-blur-sm'>
          <CardHeader>
            <h2 className='text-center text-2xl text-white'>
              {isEdit ? 'Atualize os dados do herói' : 'Forge uma Nova Lenda'}
            </h2>
            <p className='text-center text-gray-200'>
              {isEdit
                ? 'Ajuste as informações e poderes deste herói'
                : 'Preencha os dados para registrar um novo super-herói'}
            </p>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit(onSubmit)} className='space-y-6'>
              <div className='space-y-2'>
                <Label htmlFor='name' className='text-white'>
                  Nome Real
                </Label>
                <Input
                  id='name'
                  {...register('name', {
                    required: 'Nome real é obrigatório',
                  })}
                  className='border-white/30 bg-white/20 text-white placeholder:text-gray-300 focus:bg-white/30'
                  placeholder='Ex: Peter Parker'
                />
                {errors.name && (
                  <p className='text-sm text-red-400'>{errors.name.message}</p>
                )}
              </div>

              <div className='space-y-2'>
                <Label htmlFor='birthDate' className='text-white'>
                  Data de Nascimento
                </Label>
                <Input
                  type='date'
                  id='birthDate'
                  {...register('birthDate', {
                    required: 'Data de nascimento é obrigatória',
                  })}
                  className='border-white/30 bg-white/20 text-white placeholder:text-gray-300 focus:bg-white/30'
                />
                {errors.birthDate && (
                  <p className='text-sm text-red-400'>
                    {errors.birthDate.message}
                  </p>
                )}
              </div>

              <div className='space-y-2'>
                <Label htmlFor='heroName' className='text-white'>
                  Nome de Herói
                </Label>
                <Input
                  id='heroName'
                  {...register('heroName', {
                    required: 'Nome do herói é obrigatório',
                  })}
                  className='border-white/30 bg-white/20 text-white placeholder:text-gray-300 focus:bg-white/30'
                  placeholder='Ex: Homem-Aranha'
                />
                {errors.heroName && (
                  <p className='text-sm text-red-400'>
                    {errors.heroName.message}
                  </p>
                )}
              </div>

              <div className='grid grid-cols-1 gap-4 md:grid-cols-2'>
                <div className='space-y-2'>
                  <Label htmlFor='height' className='text-white'>
                    Altura (m)
                  </Label>
                  <Input
                    id='height'
                    {...register('height', {
                      required: 'Altura é obrigatória',
                    })}
                    className='border-white/30 bg-white/20 text-white placeholder:text-gray-300 focus:bg-white/30'
                    placeholder='Ex: 1.78'
                  />
                  {errors.height && (
                    <p className='text-sm text-red-400'>
                      {errors.height.message}
                    </p>
                  )}
                </div>
                <div className='space-y-2'>
                  <Label htmlFor='weight' className='text-white'>
                    Peso (kg)
                  </Label>
                  <Input
                    id='weight'
                    {...register('weight', { required: 'Peso é obrigatório' })}
                    className='border-white/30 bg-white/20 text-white placeholder:text-gray-300 focus:bg-white/30'
                    placeholder='Ex: 75'
                  />
                  {errors.weight && (
                    <p className='text-sm text-red-400'>
                      {errors.weight.message}
                    </p>
                  )}
                </div>
              </div>

              <div className='space-y-4'>
                <Label className='text-white'>Superpoderes</Label>
                {selectedPowers.length > 0 && (
                  <div className='flex flex-wrap gap-2 rounded-lg border border-white/20 bg-white/10 p-3'>
                    {selectedPowers.map((powerId) => (
                      <Badge
                        key={powerId}
                        variant='secondary'
                        className='flex items-center gap-1 bg-gradient-to-r from-yellow-500 to-orange-500 text-white hover:from-yellow-600 hover:to-orange-600'
                      >
                        {superpowers.find((p) => p.id === powerId)?.name}
                        <button
                          type='button'
                          onClick={() => handleRemovePower(powerId)}
                          className='ml-1 rounded-full p-0.5 hover:bg-white/20'
                        >
                          <X className='h-3 w-3' />
                        </button>
                      </Badge>
                    ))}
                  </div>
                )}

                <div className='relative'>
                  <Select
                    onValueChange={(val) => handleAddPower(Number(val))}
                    value={'' as any}
                  >
                    <SelectTrigger className='border-white/30 bg-white/20 text-white'>
                      <SelectValue placeholder='Selecione um superpoder' />
                    </SelectTrigger>
                    <SelectContent className='border-gray-700 bg-gray-900'>
                      {superpowers
                        .filter((sp) => !selectedPowers.includes(sp.id))
                        .map((power) => (
                          <SelectItem
                            key={power.id}
                            value={String(power.id)}
                            className='text-white hover:bg-gray-800 focus:bg-gray-800'
                          >
                            {power.name}
                          </SelectItem>
                        ))}
                    </SelectContent>
                  </Select>
                </div>
                {selectedPowers.length === 0 && (
                  <p className='text-sm text-gray-300'>
                    Selecione pelo menos um superpoder
                  </p>
                )}
              </div>

              <div className='pt-6'>
                <Button
                  type='submit'
                  className='w-full border-0 bg-gradient-to-r from-green-600 to-blue-600 text-white hover:from-green-700 hover:to-blue-700'
                  size='lg'
                  disabled={selectedPowers.length === 0}
                >
                  <Save className='mr-2 h-5 w-5' />
                  {isEdit ? 'Salvar Alterações' : 'Criar Herói'}
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}

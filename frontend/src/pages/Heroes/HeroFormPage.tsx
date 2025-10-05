import { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import { useNavigate, useParams } from 'react-router-dom'
import { createHero, getHero, updateHero } from '../../services/heroes'
import { getSuperpowers } from '../../services/superpowers'
import { HeroRequest, SuperpowerResponse } from '../../services/types'

const schema = z.object({
  name: z.string().min(1, 'Nome é obrigatório'),
  heroName: z.string().min(1, 'Nome de herói é obrigatório'),
  birthDate: z.string().min(1, 'Data de nascimento é obrigatória'),
  height: z.coerce.number().positive('Altura deve ser maior que zero'),
  weight: z.coerce.number().positive('Peso deve ser maior que zero'),
  superpowerIds: z.array(z.number()).min(1, 'Selecione ao menos um superpoder')
})

type FormValues = z.infer<typeof schema>

export function HeroFormPage({ mode }: { mode: 'create' | 'edit' }) {
  const navigate = useNavigate()
  const { id } = useParams()
  const isEdit = mode === 'edit'

  const [superpowers, setSuperpowers] = useState<SuperpowerResponse[]>([])
  const [loading, setLoading] = useState(true)

  const { register, handleSubmit, setValue, formState: { errors, isSubmitting }, watch } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      name: '',
      heroName: '',
      birthDate: '',
      height: 1,
      weight: 1,
      superpowerIds: []
    }
  })

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
          setValue('birthDate', h.birthDate?.substring(0,10) || '')
          setValue('height', h.height)
          setValue('weight', h.weight)
          setValue('superpowerIds', h.superpowers?.map(s => s.id) || [])
        }
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [id, isEdit, setValue])

  async function onSubmit(values: FormValues) {
    const payload: HeroRequest = {
      name: values.name,
      heroName: values.heroName,
      birthDate: new Date(values.birthDate).toISOString(),
      height: values.height,
      weight: values.weight,
      superpowerIds: values.superpowerIds
    }
    if (isEdit && id) {
      await updateHero(Number(id), payload)
    } else {
      await createHero(payload)
    }
    navigate('/heroes')
  }

  const selected = watch('superpowerIds') as number[]

  if (loading) return <div className="text-gray-600">Carregando...</div>

  return (
    <div>
      <div className="mb-4">
        <h2 className="text-xl font-semibold">{isEdit ? 'Editar Herói' : 'Novo Herói'}</h2>
      </div>
      <div className="rounded-lg border bg-white p-4 shadow">
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
            <div>
              <label className="mb-1 block text-sm text-gray-700">Nome</label>
              <input className="w-full rounded-md border px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500" {...register('name')} />
              {errors.name && <span className="text-sm text-red-600">{errors.name.message}</span>}
            </div>
            <div>
              <label className="mb-1 block text-sm text-gray-700">Nome de Herói</label>
              <input className="w-full rounded-md border px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500" {...register('heroName')} />
              {errors.heroName && <span className="text-sm text-red-600">{errors.heroName.message}</span>}
            </div>
            <div>
              <label className="mb-1 block text-sm text-gray-700">Data de Nascimento</label>
              <input type="date" className="w-full rounded-md border px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500" {...register('birthDate')} />
              {errors.birthDate && <span className="text-sm text-red-600">{errors.birthDate.message}</span>}
            </div>
            <div>
              <label className="mb-1 block text-sm text-gray-700">Altura (m)</label>
              <input type="number" step="0.01" className="w-full rounded-md border px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500" {...register('height', { valueAsNumber: true })} />
              {errors.height && <span className="text-sm text-red-600">{errors.height.message}</span>}
            </div>
            <div>
              <label className="mb-1 block text-sm text-gray-700">Peso (kg)</label>
              <input type="number" step="0.1" className="w-full rounded-md border px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500" {...register('weight', { valueAsNumber: true })} />
              {errors.weight && <span className="text-sm text-red-600">{errors.weight.message}</span>}
            </div>
          </div>

          <div>
            <label className="mb-1 block text-sm text-gray-700">Superpoderes</label>
            <select
              multiple
              size={Math.min(8, Math.max(3, superpowers.length))}
              className="w-full rounded-md border px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
              value={(selected || []).map(String)}
              onChange={(e) => {
                const values = Array.from(e.target.selectedOptions).map(opt => Number(opt.value))
                setValue('superpowerIds', values, { shouldValidate: true })
              }}
            >
              {superpowers.map(sp => (
                <option key={sp.id} value={sp.id}>{sp.name}</option>
              ))}
            </select>
            {errors.superpowerIds && <span className="text-sm text-red-600">{errors.superpowerIds.message}</span>}
          </div>

          <div className="flex gap-2">
            <button className="inline-flex items-center rounded-md border px-3 py-2 text-sm hover:bg-gray-50" type="button" onClick={() => navigate(-1)}>Cancelar</button>
            <button className="inline-flex items-center rounded-md bg-blue-600 px-3 py-2 text-sm font-medium text-white hover:bg-blue-700" type="submit" disabled={isSubmitting}>
              {isEdit ? 'Salvar Alterações' : 'Criar Herói'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}

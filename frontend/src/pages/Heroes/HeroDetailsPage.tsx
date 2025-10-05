import { useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { deleteHero, getHero } from '../../services/heroes'
import { ApiResponse, HeroResponse } from '../../services/types'

export function HeroDetailsPage() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [loading, setLoading] = useState(true)
  const [data, setData] = useState<ApiResponse<HeroResponse> | null>(null)

  useEffect(() => {
    async function load() {
      setLoading(true)
      try {
        const res = await getHero(Number(id))
        setData(res)
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [id])

  async function onDelete() {
    if (!id) return
    if (!confirm('Confirmar exclusão do herói?')) return
    await deleteHero(Number(id))
    navigate('/heroes')
  }

  if (loading) return <div className="text-gray-600">Carregando...</div>
  if (!data || !data.result) return <div className="rounded-lg border bg-white p-4 shadow">Herói não encontrado.</div>

  const h = data.result

  return (
    <div>
      <div className="mb-4 flex items-center justify-between">
        <h2 className="text-xl font-semibold">{h.heroName}</h2>
        <div className="flex gap-2">
          <Link className="inline-flex items-center rounded-md border px-3 py-2 text-sm hover:bg-gray-50" to={`/heroes/${h.id}/edit`}>Editar</Link>
          <button className="inline-flex items-center rounded-md bg-red-600 px-3 py-2 text-sm text-white hover:bg-red-700" onClick={onDelete}>Excluir</button>
        </div>
      </div>

      <div className="rounded-lg border bg-white p-4 shadow">
        <p><span className="font-medium">Nome:</span> {h.name}</p>
        <p><span className="font-medium">Data de Nascimento:</span> {h.birthDate?.substring(0,10)}</p>
        <p><span className="font-medium">Altura:</span> {h.height} m</p>
        <p><span className="font-medium">Peso:</span> {h.weight} kg</p>
        <p className="mt-2"><span className="font-medium">Superpoderes:</span> {h.superpowers?.map(s => <span key={s.id} className="mr-1 inline-block rounded bg-indigo-100 px-2 py-0.5 text-xs font-medium text-indigo-700">{s.name}</span>)}</p>
      </div>
    </div>
  )
}

import { useEffect, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { deleteHero, getHeroes } from '../../services/heroes'
import { ApiResponse, HeroResponse } from '../../services/types'

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

  useEffect(() => { load() }, [])

  async function onDelete(id: number) {
    if (!confirm('Confirmar exclusão do herói?')) return
    await deleteHero(id)
    await load()
  }

  if (loading) return <div className="text-gray-600">Carregando...</div>
  if (!data || !data.result || data.result.length === 0) return (
    <div>
      <div className="mb-4 flex items-center justify-between">
        <h2 className="text-xl font-semibold">Heróis</h2>
        <div className="flex gap-2">
          <Link to="/heroes/new" className="inline-flex items-center rounded-md bg-blue-600 px-3 py-2 text-sm font-medium text-white hover:bg-blue-700">Novo Herói</Link>
        </div>
      </div>
      <div className="rounded-lg border bg-white p-4 shadow">Nenhum super-herói cadastrado.</div>
    </div>
  )

  return (
    <div>
      <div className="mb-4 flex items-center justify-between">
        <h2 className="text-xl font-semibold">Heróis</h2>
        <div className="flex gap-2">
          <Link to="/heroes/new" className="inline-flex items-center rounded-md bg-blue-600 px-3 py-2 text-sm font-medium text-white hover:bg-blue-700">Novo Herói</Link>
        </div>
      </div>

      <div className="rounded-lg border bg-white p-0 shadow">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-4 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">Id</th>
              <th className="px-4 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">Nome</th>
              <th className="px-4 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">Herói</th>
              <th className="px-4 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500">Superpoderes</th>
              <th className="px-4 py-3"></th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200 bg-white">
            {data.result.map(h => (
              <tr key={h.id}>
                <td className="px-4 py-3 text-sm text-gray-700">{h.id}</td>
                <td className="px-4 py-3 text-sm text-gray-700">{h.name}</td>
                <td className="px-4 py-3 text-sm text-blue-600">
                  <Link to={`/heroes/${h.id}`}>{h.heroName}</Link>
                </td>
                <td className="px-4 py-3">
                  {h.superpowers?.map(p => (
                    <span key={p.id} className="mr-1 mb-1 inline-block rounded bg-indigo-100 px-2 py-0.5 text-xs font-medium text-indigo-700">{p.name}</span>
                  ))}
                </td>
                <td className="px-4 py-3 text-right">
                  <button className="inline-flex items-center rounded-md border px-3 py-1.5 text-sm hover:bg-gray-50" onClick={() => navigate(`/heroes/${h.id}/edit`)}>Editar</button>
                  <button className="ml-2 inline-flex items-center rounded-md bg-red-600 px-3 py-1.5 text-sm text-white hover:bg-red-700" onClick={() => onDelete(h.id)}>Excluir</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

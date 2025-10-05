import { api } from './apiClient'
import { ApiResponse, HeroRequest, HeroResponse } from './types'

export async function getHeroes() {
  const { data } = await api.get<ApiResponse<HeroResponse[]>>('/api/heroes')
  return data
}

export async function getHero(id: number) {
  const { data } = await api.get<ApiResponse<HeroResponse>>(`/api/heroes/${id}`)
  return data
}

export async function createHero(payload: HeroRequest) {
  const { data } = await api.post<ApiResponse<HeroResponse>>(
    '/api/heroes',
    payload,
  )
  return data
}

export async function updateHero(id: number, payload: HeroRequest) {
  const { data } = await api.put<ApiResponse<HeroResponse>>(
    `/api/heroes/${id}`,
    payload,
  )
  return data
}

export async function deleteHero(id: number) {
  const { data } = await api.delete<ApiResponse<string>>(`/api/heroes/${id}`)
  return data
}

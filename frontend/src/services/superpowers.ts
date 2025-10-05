import { api } from './apiClient'
import { ApiResponse, SuperpowerResponse } from './types'

export async function getSuperpowers() {
  const { data } = await api.get<ApiResponse<SuperpowerResponse[]>>('/api/superpowers')
  return data
}


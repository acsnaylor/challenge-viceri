export type SuperpowerResponse = {
  id: number
  name: string
  description?: string | null
}

export type HeroResponse = {
  id: number
  name: string
  heroName: string
  birthDate: string | null
  height: number
  weight: number
  superpowers: SuperpowerResponse[]
}

export type HeroRequest = {
  name: string
  heroName: string
  birthDate: string
  height: number
  weight: number
  superpowerIds: number[]
}

export type ApiResponse<T> = {
  code: number
  message: string
  result?: T | null
  errors?: Record<string, string> | null
}

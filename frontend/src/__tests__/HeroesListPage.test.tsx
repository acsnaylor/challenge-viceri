import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { MemoryRouter } from 'react-router-dom'
import { HeroesListPage } from '@pages/Heroes/HeroesListPage'

const getHeroes = jest.fn()
const deleteHero = jest.fn().mockResolvedValue({})

jest.mock('@services/heroes', () => ({
  getHeroes: (...args: any[]) => (getHeroes as any)(...args),
  deleteHero: (...args: any[]) => (deleteHero as any)(...args),
}))

describe('HeroesListPage', () => {
  it('renderiza heróis vindos da API', async () => {
    getHeroes.mockResolvedValue({
      result: [
        {
          id: 1,
          name: 'Bruce Wayne',
          heroName: 'Batman',
          birthDate: '1972-02-19T00:00:00Z',
          height: 1.88,
          weight: 95,
          superpowers: [{ id: 1, name: 'Força' }],
        },
      ],
    })

    render(
      <MemoryRouter>
        <HeroesListPage />
      </MemoryRouter>,
    )

    await screen.findByText('Batman')
    expect(screen.getByText('Bruce Wayne')).toBeInTheDocument()
    expect(screen.getByText('Força')).toBeInTheDocument()
  })

  it('exclui herói quando confirmado', async () => {
    window.confirm = jest.fn(() => true) as any
    getHeroes
      .mockResolvedValueOnce({
        result: [
          {
            id: 1,
            name: 'Bruce Wayne',
            heroName: 'Batman',
            birthDate: '1972-02-19T00:00:00Z',
            height: 1.88,
            weight: 95,
            superpowers: [],
          },
        ],
      })
      .mockResolvedValueOnce({ result: [] })

    render(
      <MemoryRouter>
        <HeroesListPage />
      </MemoryRouter>,
    )

    await screen.findByText('Batman')

    const delBtn =
      screen
        .getAllByRole('button')
        .find((btn) => btn.textContent?.includes('Trash')) ||
      screen.getAllByRole('button')[1]
    await userEvent.click(delBtn as HTMLElement)

    await waitFor(() => expect(deleteHero).toHaveBeenCalledWith(1))

    await screen.findByText('Nenhum herói cadastrado')
  })
})

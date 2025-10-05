import { render, screen, waitFor } from '@testing-library/react'
import { fireEvent } from '@testing-library/dom'
import userEvent from '@testing-library/user-event'
import { MemoryRouter, Route, Routes } from 'react-router-dom'
import { HeroFormPage } from '@pages/Heroes/HeroFormPage'

jest.mock('@services/superpowers', () => ({
  getSuperpowers: jest
    .fn()
    .mockResolvedValue({ result: [{ id: 1, name: 'Força' }] }),
}))

const updateHero = jest.fn().mockResolvedValue({})
const createHero = jest.fn().mockResolvedValue({})
const getHero = jest.fn()

jest.mock('@services/heroes', () => ({
  getHero: (...args: any[]) => (getHero as any)(...args),
  updateHero: (...args: any[]) => (updateHero as any)(...args),
  createHero: (...args: any[]) => (createHero as any)(...args),
}))

describe('HeroFormPage - criação', () => {
  afterEach(() => {
    jest.clearAllMocks()
  })

  it('exibe modo de criação e botão salvar desabilitado sem superpoder', async () => {
    render(
      <MemoryRouter initialEntries={['/heroes/new']}>
        <Routes>
          <Route path='/heroes/new' element={<HeroFormPage onBack={() => {}} />} />
        </Routes>
      </MemoryRouter>,
    )

    await screen.findByText(/Selecione um superpoder/i)

    expect(screen.getByText(/Criar Novo Herói/i)).toBeInTheDocument()

    const saveBtn = screen.getByRole('button', { name: /Criar Herói/i })
    expect(saveBtn).toBeDisabled()
  })

  it('exibe mensagens de validação quando campos obrigatórios estão vazios', async () => {
    render(
      <MemoryRouter initialEntries={['/heroes/new']}>
        <Routes>
          <Route path='/heroes/new' element={<HeroFormPage onBack={() => {}} />} />
        </Routes>
      </MemoryRouter>,
    )

    await screen.findByText(/Selecione um superpoder/i)

    const form = (screen.getByRole('button', { name: /Criar Herói/i }).closest('form') as HTMLFormElement)
    fireEvent.submit(form)

    expect(await screen.findByText('Nome real é obrigatório')).toBeInTheDocument()
    expect(screen.getByText('Data de nascimento é obrigatória')).toBeInTheDocument()
    expect(screen.getByText('Nome do herói é obrigatório')).toBeInTheDocument()
    expect(screen.getByText('Altura é obrigatória')).toBeInTheDocument()
    expect(screen.getByText('Peso é obrigatório')).toBeInTheDocument()

    expect(createHero).not.toHaveBeenCalled()
  })

  it('cria herói convertendo vírgula e data para ISO', async () => {
    render(
      <MemoryRouter initialEntries={['/heroes/new']}>
        <Routes>
          <Route path='/heroes/new' element={<HeroFormPage onBack={() => {}} />} />
        </Routes>
      </MemoryRouter>,
    )

    await screen.findByText(/Selecione um superpoder/i)

    await userEvent.type(screen.getByLabelText(/Nome Real/i), 'Clark Kent')
    await userEvent.type(screen.getByLabelText(/Nome de Herói/i), 'Superman')
    const birth = screen.getByLabelText(/Data de Nascimento/i) as HTMLInputElement
    await userEvent.type(birth, '1999-02-01')
    await userEvent.type(screen.getByLabelText(/Altura/i), '1,90')
    await userEvent.type(screen.getByLabelText(/Peso/i), '95,4')

    const form = (screen.getByRole('button', { name: /Criar Herói/i }).closest('form') as HTMLFormElement)
    fireEvent.submit(form)

    await waitFor(() => expect(createHero).toHaveBeenCalled())
    const [payload] = createHero.mock.calls[0]
    expect(payload.height).toBeCloseTo(1.9)
    expect(payload.weight).toBeCloseTo(95.4)
    expect(payload.birthDate).toBe(new Date('1999-02-01').toISOString())
    expect(payload.superpowerIds).toEqual([])
  })
})

describe('HeroFormPage - edição', () => {
  afterEach(() => {
    jest.clearAllMocks()
  })

  it('remove superpoder selecionado e desabilita salvar', async () => {
    getHero.mockResolvedValue({
      result: {
        id: 1,
        name: 'Bruce Wayne',
        heroName: 'Batman',
        birthDate: '1972-02-19T00:00:00Z',
        height: 1.8,
        weight: 90,
        superpowers: [{ id: 1, name: 'Força' }],
      },
    })

    render(
      <MemoryRouter initialEntries={['/heroes/1/edit']}>
        <Routes>
          <Route path='/heroes/:id/edit' element={<HeroFormPage onBack={() => {}} />} />
        </Routes>
      </MemoryRouter>,
    )

    await screen.findByDisplayValue('Bruce Wayne')

    const saveBtn = screen.getByRole('button', { name: /Salvar Alterações/i })
    expect(saveBtn).not.toBeDisabled()

    const powerLabel = screen.getByText('Força')
    const removeBtn = powerLabel.parentElement?.querySelector('button') as HTMLElement
    await userEvent.click(removeBtn)

    expect(saveBtn).toBeDisabled()
  })

  it('botão de voltar chama onBack', async () => {
    const onBack = jest.fn()

    render(
      <MemoryRouter initialEntries={['/heroes/new']}>
        <Routes>
          <Route path='/heroes/new' element={<HeroFormPage onBack={onBack} />} />
        </Routes>
      </MemoryRouter>,
    )

    await screen.findByText(/Selecione um superpoder/i)

    const backBtn = screen.getAllByRole('button')[0]
    await userEvent.click(backBtn)
    expect(onBack).toHaveBeenCalled()
  })
})


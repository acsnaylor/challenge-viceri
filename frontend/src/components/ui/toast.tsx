import { createContext, useCallback, useContext, useMemo, useState } from 'react'
import type { ReactNode } from 'react'

type ToastVariant = 'info' | 'success' | 'error'

type ToastItem = {
  id: number
  message: string
  variant: ToastVariant
  duration: number
}

type ToastContextValue = {
  show: (message: string, options?: { variant?: ToastVariant; duration?: number }) => void
}

const ToastContext = createContext<ToastContextValue>({
  // no-op default to avoid crashes in tests without provider
  show: () => {},
})

export function useToast() {
  return useContext(ToastContext)
}

export function ToastProvider({ children }: { children: ReactNode }) {
  const [toasts, setToasts] = useState<ToastItem[]>([])

  const remove = useCallback((id: number) => {
    setToasts((prev) => prev.filter((t) => t.id !== id))
  }, [])

  const show = useCallback<ToastContextValue['show']>((message, options) => {
    const id = Date.now() + Math.floor(Math.random() * 1000)
    const variant: ToastVariant = options?.variant ?? 'info'
    const duration = options?.duration ?? 4000
    setToasts((prev) => [...prev, { id, message, variant, duration }])
    if (duration > 0) {
      setTimeout(() => remove(id), duration)
    }
  }, [remove])

  const value = useMemo(() => ({ show }), [show])

  return (
    <ToastContext.Provider value={value}>
      {children}
      <div className='pointer-events-none fixed right-4 top-4 z-50 flex w-[calc(100%-2rem)] max-w-sm flex-col gap-2'>
        {toasts.map((t) => (
          <div
            key={t.id}
            className={[
              'pointer-events-auto rounded-lg border p-3 shadow-md backdrop-blur-sm',
              t.variant === 'error' && 'border-red-500/30 bg-red-900/40 text-red-100',
              t.variant === 'success' && 'border-green-500/30 bg-green-900/40 text-green-100',
              t.variant === 'info' && 'border-white/20 bg-white/10 text-white',
            ].filter(Boolean).join(' ')}
            role='status'
            aria-live='polite'
          >
            {t.message}
          </div>
        ))}
      </div>
    </ToastContext.Provider>
  )
}

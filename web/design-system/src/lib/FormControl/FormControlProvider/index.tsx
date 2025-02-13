import { createContext, ReactNode, useContext, useMemo, useState } from "react"

type FormControlContextData = {
  label?: ReactNode
  setLabel: (label?: ReactNode) => void
}

const FormControlContext = createContext<FormControlContextData | null>(null)

export function useFormControl() {
  const context = useContext(FormControlContext)

  if (!context) {
    throw new Error("useFormControl must be used within a FormControlProvider.")
  }

  return context
}

type FormControlProviderProps = {
  children?: ReactNode
}

export function FormControlProvider({ children }: FormControlProviderProps) {
  const [label, setLabel] = useState<ReactNode>()

  const contextValue = useMemo<FormControlContextData>(
    () => ({
      label,
      setLabel,
    }),
    [label],
  )

  return <FormControlContext.Provider value={contextValue}>{children}</FormControlContext.Provider>
}

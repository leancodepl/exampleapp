import { createContext, ReactNode, useCallback, useContext, useEffect, useMemo, useState } from "react"

export function mkPortalComponent() {
  type PortalComponentContextData = {
    values: ReactNode[]
    addValue: (value: ReactNode) => () => void
  }

  const portalComponentContext = createContext<PortalComponentContextData>({ values: [], addValue: () => () => {} })

  type PortalComponentProviderProps = {
    children?: ReactNode
  }

  function PortalComponentProvider({ children }: PortalComponentProviderProps) {
    const [values, setValues] = useState<ReactNode[]>([])

    const addValue = useCallback((value: ReactNode) => {
      setValues(values => [...values, value])

      return () =>
        setValues(values => {
          const i = values.indexOf(value)

          if (i < 0) return values

          const newValues = values.slice()
          newValues.splice(i, 1)
          return newValues
        })
    }, [])

    const value = useMemo<PortalComponentContextData>(() => {
      return {
        values,
        addValue,
      }
    }, [addValue, values])

    return <portalComponentContext.Provider value={value}>{children}</portalComponentContext.Provider>
  }

  type PortalComponentValueProps = {
    children: ReactNode
  }

  function PortalComponentValue({ children }: PortalComponentValueProps) {
    const { addValue } = useContext(portalComponentContext)

    useEffect(() => addValue(children), [addValue, children])

    return null
  }

  function PortalComponentPlaceholder() {
    const { values } = useContext(portalComponentContext)

    // eslint-disable-next-line react/jsx-no-useless-fragment
    return <>{values}</>
  }

  return {
    Provider: PortalComponentProvider,
    Value: PortalComponentValue,
    Placeholder: PortalComponentPlaceholder,
  }
}

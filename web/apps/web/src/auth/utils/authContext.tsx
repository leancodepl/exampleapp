import { createContext, useContext, useMemo } from "react"
import { ReactNode } from "@tanstack/react-router"

type AuthContextData = {
  isLoading?: boolean
}

const authContext = createContext<AuthContextData>({})

type AuthContextProviderProps = {
  isLoading?: boolean
  children?: ReactNode
}

export function AuthContextProvider({ isLoading, children }: AuthContextProviderProps) {
  const value = useMemo<AuthContextData>(
    () => ({
      isLoading,
    }),
    [isLoading],
  )

  return <authContext.Provider value={value}>{children}</authContext.Provider>
}

export function useAuthContext() {
  return useContext(authContext)
}

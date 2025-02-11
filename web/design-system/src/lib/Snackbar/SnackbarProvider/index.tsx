import { useCallback, useMemo, useState } from "react"
import * as ToastPrimitive from "@radix-ui/react-toast"
import { ReactNode } from "@tanstack/react-router"
import { Snackbar, SnackbarProps } from "../Snackbar"
import { MountedSnackbar, SnackbarContext, SnackbarContextData, SnackbarData } from "../SnackbarContext"
import { SnackbarViewport } from "./styles"

type SnackbarProviderProps = {
  children?: ReactNode
}

export function SnackbarProvider({ children }: SnackbarProviderProps) {
  const [snackbars, setSnackbars] = useState<IdentifiableSnackbar[]>([])

  const snackbar = useCallback(
    (snackbarData: SnackbarData) =>
      new Promise<MountedSnackbar>(resolve => {
        setSnackbars(snackbars => {
          const newSnackbar: IdentifiableSnackbar = {
            ...snackbarData,
            id: Math.random(),
            onClose: () => {},
          }

          const close = () =>
            setSnackbars(snackbars => {
              const i = snackbars.indexOf(newSnackbar)

              if (i < 0) return snackbars

              const newSnackbars = snackbars.slice()

              newSnackbars.splice(i, 1)

              return newSnackbars
            })

          newSnackbar.onClose = close

          resolve({ close })

          return [...snackbars, newSnackbar]
        })
      }),
    [],
  )

  const hideAllSnackbars = useCallback(() => setSnackbars([]), [])

  const snackbarContextData = useMemo<SnackbarContextData>(
    () => ({ snackbar, hideAllSnackbars }),
    [hideAllSnackbars, snackbar],
  )

  const currentSnackbar = snackbars.at(0)

  return (
    <ToastPrimitive.Provider swipeDirection="right">
      <SnackbarContext.Provider value={snackbarContextData}>{children}</SnackbarContext.Provider>

      {currentSnackbar && <Snackbar key={currentSnackbar.id} {...currentSnackbar} />}
      <SnackbarViewport />
    </ToastPrimitive.Provider>
  )
}

type IdentifiableSnackbar = {
  id: number
} & SnackbarProps

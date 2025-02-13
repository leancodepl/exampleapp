import { createContext, ReactNode } from "react"

export type SnackbarData = {
  type: "error" | "info" | "success" | "warning"
  icon?: ReactNode
  title?: ReactNode
  action?: ReactNode
}

export type MountedSnackbar = {
  close: () => void
}

export type SnackbarContextData = {
  snackbar(snackbarData: SnackbarData): Promise<MountedSnackbar>
  hideAllSnackbars: () => void
}

export const SnackbarContext = createContext<SnackbarContextData>({
  snackbar: async () => ({ close: () => {} }),
  hideAllSnackbars: () => {},
})

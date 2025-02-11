import { useCallback, useContext } from "react"
import { IconXCircle } from "../icons"
import { SnackbarContext, SnackbarData } from "./SnackbarContext"

type SpecializedSnackbarData = Omit<SnackbarData, "type">

export function useSnackbar() {
  const { snackbar, hideAllSnackbars } = useContext(SnackbarContext)

  const info = useCallback((data: SpecializedSnackbarData) => snackbar({ type: "info", ...data }), [snackbar])
  const success = useCallback((data: SpecializedSnackbarData) => snackbar({ type: "success", ...data }), [snackbar])
  const error = useCallback(
    (data: SpecializedSnackbarData) => snackbar({ type: "error", icon: <IconXCircle />, ...data }),
    [snackbar],
  )

  return {
    success,
    info,
    error,
    hideAllSnackbars,
  }
}

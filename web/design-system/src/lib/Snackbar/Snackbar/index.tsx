import { useCallback } from "react"
import * as ToastPrimitive from "@radix-ui/react-toast"
import { ReactNode } from "@tanstack/react-router"
import { Text } from "../../Text"
import { dataType } from "../attributes"
import { SnackbarIcon, SnackbarRoot } from "./styles"

export type SnackbarType = "error" | "info" | "success" | "warning"

export type SnackbarProps = {
  type: SnackbarType
  title?: ReactNode
  action?: ReactNode
  icon?: ReactNode
  onClose: () => void
}

export function Snackbar({ onClose, title, action, icon, type }: SnackbarProps) {
  const onOpenChange = useCallback(
    (open: boolean) => {
      if (!open) setTimeout(onClose, 200)
    },
    [onClose],
  )

  return (
    <SnackbarRoot defaultOpen onOpenChange={onOpenChange} {...dataType(type)} forceMount>
      {icon && <SnackbarIcon>{icon}</SnackbarIcon>}

      {title && (
        <ToastPrimitive.Title className="ToastTitle">
          <Text color="default.primary">{title}</Text>
        </ToastPrimitive.Title>
      )}

      {action && (
        <ToastPrimitive.Action asChild altText="Goto schedule to undo" className="ToastAction">
          {action}
        </ToastPrimitive.Action>
      )}
    </SnackbarRoot>
  )
}

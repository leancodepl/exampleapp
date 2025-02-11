import * as TooltipPrimitive from "@radix-ui/react-tooltip"
import { ReactNode } from "@tanstack/react-router"

type TooltipTriggerProps = {
  children?: ReactNode
}

export function TooltipTrigger({ children }: TooltipTriggerProps) {
  return <TooltipPrimitive.Trigger asChild>{children}</TooltipPrimitive.Trigger>
}

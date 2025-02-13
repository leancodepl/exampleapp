import { ComponentPropsWithoutRef } from "react"
import * as TooltipPrimitive from "@radix-ui/react-tooltip"
import { MutuallyExclusiveUnion } from "../../utils"
import { TooltipArrow, TooltipContent } from "./styles"

export type TooltipType = "danger" | "info" | "inverse" | "success" | "warning"

type TooltipTypeProps = { type?: TooltipType } | MutuallyExclusiveUnion<TooltipType>

type TooltipProps = ComponentPropsWithoutRef<typeof TooltipPrimitive.Content> & TooltipTypeProps

export const Provider = TooltipPrimitive.Provider
export const Portal = TooltipPrimitive.Portal
export const Root = TooltipPrimitive.Root

export function Content({ children, ...props }: TooltipProps) {
  const type =
    ("type" in props && props.type) ||
    ("danger" in props && props["danger"] && "danger") ||
    ("info" in props && props["info"] && "info") ||
    ("inverse" in props && props["inverse"] && "inverse") ||
    ("success" in props && props["success"] && "success") ||
    ("warning" in props && props["warning"] && "warning") ||
    "info"

  return (
    <TooltipContent {...props} data-type={type}>
      {children}
      <TooltipArrow asChild>
        <div />
      </TooltipArrow>
    </TooltipContent>
  )
}

export { TooltipTrigger as Trigger } from "./TooltipTrigger"

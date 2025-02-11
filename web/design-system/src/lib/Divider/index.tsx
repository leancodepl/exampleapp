import { ComponentPropsWithoutRef, ElementRef, forwardRef } from "react"
import * as SeparatorPrimitive from "@radix-ui/react-separator"
import { DividerRoot } from "./styles"

export const Divider = forwardRef<
  ElementRef<typeof SeparatorPrimitive.Root>,
  ComponentPropsWithoutRef<typeof SeparatorPrimitive.Root>
>(({ orientation = "horizontal", decorative = true, ...props }, ref) => (
  <DividerRoot ref={ref} decorative={decorative} orientation={orientation} {...props} />
))

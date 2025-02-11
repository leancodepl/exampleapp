import * as SelectPrimitive from "@radix-ui/react-select"
import { ReactNode } from "@tanstack/react-router"
import { SelectPrimitiveContent } from "./styles"

type SelectContentProps = {
  children?: ReactNode
}

export function SelectContent({ children }: SelectContentProps) {
  return (
    <SelectPrimitive.Portal>
      <SelectPrimitiveContent avoidCollisions={false} collisionPadding={0} position="popper" side="bottom">
        <SelectPrimitive.Viewport>{children}</SelectPrimitive.Viewport>
      </SelectPrimitiveContent>
    </SelectPrimitive.Portal>
  )
}

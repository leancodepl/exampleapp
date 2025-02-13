import { ComponentPropsWithoutRef, ElementRef, forwardRef } from "react"
import * as SheetPrimitive from "@radix-ui/react-dialog"
import { SheetOverlay, StyledSheetContent } from "./styles"

type SheetContentProps = ComponentPropsWithoutRef<typeof SheetPrimitive.Content>

export const SheetContent = forwardRef<ElementRef<typeof SheetPrimitive.Content>, SheetContentProps>(
  ({ children, ...props }, ref) => (
    <SheetPrimitive.Portal>
      <SheetOverlay />

      <StyledSheetContent ref={ref} {...props}>
        {children}
      </StyledSheetContent>
    </SheetPrimitive.Portal>
  ),
)

import { ReactNode } from "react"
import * as DialogPrimitive from "@radix-ui/react-dialog"
import { DialogClose } from "../DialogClose"
import { DialogOverlay } from "../DialogOverlay"
import { StyledDialogContent } from "./styles"

type DialogContentProps = {
  children?: ReactNode
}

export function DialogContent({ children }: DialogContentProps) {
  return (
    <DialogPrimitive.Portal>
      <DialogOverlay />

      <StyledDialogContent>
        <DialogClose />

        {children}
      </StyledDialogContent>
    </DialogPrimitive.Portal>
  )
}

import { ReactNode } from "react"
import { StyledDialogDescription } from "./styles"

type DialogDescriptionProps = {
  children?: ReactNode
}

export function DialogDescription({ children }: DialogDescriptionProps) {
  return <StyledDialogDescription>{children}</StyledDialogDescription>
}

import { ReactNode } from "react"
import { Text } from "../../Text"
import { StyledDialogTitle } from "./styles"

type DialogTitleProps = {
  children?: ReactNode
}

export function DialogTitle({ children }: DialogTitleProps) {
  return (
    <StyledDialogTitle>
      <Text headline-small color="default.primary">
        {children}
      </Text>
    </StyledDialogTitle>
  )
}

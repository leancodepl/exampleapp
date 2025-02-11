import { ReactNode } from "react"
import { StyledDescriptionBlock } from "./styles"

type DescriptionBlockProps = {
  children?: ReactNode
  span?: number
}

export function DescriptionBlock({ children, span }: DescriptionBlockProps) {
  return (
    <StyledDescriptionBlock style={{ gridColumn: span ? `span ${span}` : undefined }}>
      {children}
    </StyledDescriptionBlock>
  )
}

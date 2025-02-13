import { forwardRef, ReactNode } from "react"
import { Text } from "../Text"
import { SeparatorRoot } from "./styles"

type SeparatorProps = {
  children?: ReactNode
}

export const Separator = forwardRef<HTMLDivElement, SeparatorProps>(({ children }, ref) => {
  return <SeparatorRoot ref={ref}>{children && <Text body>{children}</Text>}</SeparatorRoot>
})

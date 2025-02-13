import { ReactNode } from "react"
import { Text } from "../../Text"

type DescriptionLabelProps = {
  children?: ReactNode
}

export function DescriptionLabel({ children }: DescriptionLabelProps) {
  return (
    <Text caption color="default.secondary">
      {children}
    </Text>
  )
}

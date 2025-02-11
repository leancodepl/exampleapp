import { ReactNode } from "react"
import { Text } from "../../Text"

type DescriptionValueProps = {
  children?: ReactNode
}

export function DescriptionValue({ children }: DescriptionValueProps) {
  return (
    <Text body overflowWrap color="default.primary">
      {children}
    </Text>
  )
}

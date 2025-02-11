import { ComponentPropsWithoutRef, ReactNode } from "react"
import { Text } from "../../Text"
import { RadioIndicator, RadioItemWrapper } from "./styles"

type RadioItemProps = {
  children?: ReactNode
  asChild?: boolean
} & ComponentPropsWithoutRef<typeof RadioItemWrapper>

export function RadioItem({ children, asChild, ...props }: RadioItemProps) {
  return (
    <RadioItemWrapper {...props}>
      <RadioIndicator forceMount />

      <Text body asChild={asChild} color="default.primary">
        {children}
      </Text>
    </RadioItemWrapper>
  )
}

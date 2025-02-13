import { ReactNode } from "react"
import { Text } from "../Text"
import { dataType } from "./attributes"
import { BadgeRoot } from "./styles"

export type BadgeType = "danger" | "info" | "neutral" | "success" | "warning"

export type BadgeProps = {
  type: BadgeType
  children?: ReactNode
}

export function Badge({ children, type }: BadgeProps) {
  return (
    <Text asChild caption>
      <BadgeRoot {...dataType(type)}>{children}</BadgeRoot>
    </Text>
  )
}

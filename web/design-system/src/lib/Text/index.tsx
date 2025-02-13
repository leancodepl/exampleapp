import React, { forwardRef, ReactNode } from "react"
import { Slot } from "@radix-ui/react-slot"
import classNames from "classnames"
import { MutuallyExclusiveUnion } from "../../utils"
import { colorMap, TextRoot } from "./styles"

export type TextSize =
  | "body"
  | "caption"
  | "display"
  | "headline-large"
  | "headline-medium"
  | "headline-small"
  | "overline"
  | "subtitle"

export type TextSizeProps = { size?: TextSize } | MutuallyExclusiveUnion<TextSize>

type TextProps = {
  className?: string
  bold?: boolean
  color?: keyof typeof colorMap
  italic?: boolean
  children?: ReactNode
  ellipsis?: boolean
  capitalize?: boolean
  asChild?: boolean
  contents?: boolean
  overflowWrap?: boolean
} & TextSizeProps

export const Text = forwardRef<HTMLSpanElement, TextProps>((props, ref) => {
  const { children, bold, italic, className, color, asChild, ellipsis, capitalize, contents, overflowWrap } = props

  const size =
    ("size" in props && props.size) ||
    ("display" in props && props.display && "display") ||
    ("headline-large" in props && props["headline-large"] && "headline-large") ||
    ("headline-medium" in props && props["headline-medium"] && "headline-medium") ||
    ("headline-small" in props && props["headline-small"] && "headline-small") ||
    ("subtitle" in props && props["subtitle"] && "subtitle") ||
    ("body" in props && props["body"] && "body") ||
    ("caption" in props && props["caption"] && "caption") ||
    ("overline" in props && props["overline"] && "overline") ||
    undefined

  const cssColor = color ? colorMap[color] : undefined

  const Component = asChild ? Slot : "span"

  return (
    <TextRoot
      ref={ref}
      as={Component}
      bold={bold}
      capitalize={capitalize}
      className={classNames(className, cssColor)}
      contents={contents}
      ellipsis={ellipsis}
      italic={italic}
      overflowWrap={overflowWrap}
      size={size}>
      {children}
    </TextRoot>
  )
})

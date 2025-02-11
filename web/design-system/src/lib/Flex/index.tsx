import { ReactNode } from "@tanstack/react-router"
import classNames from "classnames"
import {
  alignMap,
  directionMap,
  FlexRoot,
  gapMap,
  justifyMap,
  paddingBottomMap,
  paddingLeftMap,
  paddingMap,
  paddingRightMap,
  paddingTopMap,
} from "./styles"

type FlexProps = {
  gap?: keyof typeof gapMap
  direction?: keyof typeof directionMap
  align?: keyof typeof alignMap
  justify?: keyof typeof justifyMap
  padding?:
    | {
        top?: keyof typeof paddingTopMap
        left?: keyof typeof paddingLeftMap
        right?: keyof typeof paddingRightMap
        bottom?: keyof typeof paddingBottomMap
      }
    | keyof typeof paddingMap

  className?: string

  children?: ReactNode
}

export function Flex({ gap, direction, align, justify, padding, className, children }: FlexProps) {
  const cssGap = gap ? gapMap[gap] : undefined
  const cssDirection = direction ? directionMap[direction] : undefined
  const cssAlign = align ? alignMap[align] : undefined
  const cssJustify = justify ? justifyMap[justify] : undefined
  const cssPadding = padding
    ? typeof padding === "string"
      ? paddingMap[padding]
      : classNames(
          padding.top && paddingTopMap[padding.top],
          padding.left && paddingLeftMap[padding.left],
          padding.right && paddingRightMap[padding.right],
          padding.bottom && paddingBottomMap[padding.bottom],
        )
    : undefined

  return (
    <FlexRoot className={classNames(className, cssGap, cssDirection, cssAlign, cssJustify, cssPadding)}>
      {children}
    </FlexRoot>
  )
}

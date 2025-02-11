import { CSSProperties } from "react"
import { ReactNode } from "@tanstack/react-router"
import { cssLoading } from "../../utils"
import { SkeletonLoaderRoot } from "./styles"

type SkeletonLoaderProps = {
  height?: number
  children?: ReactNode
}

export function SkeletonLoader({ height, children }: SkeletonLoaderProps) {
  return (
    <SkeletonLoaderRoot
      className={cssLoading}
      style={height ? ({ "--skeleton-height": `${height}px` } as CSSProperties) : undefined}>
      {children}
    </SkeletonLoaderRoot>
  )
}

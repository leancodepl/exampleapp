import { CSSProperties, ReactNode, useMemo } from "react"
import { Breakpoint } from "@pigment-css/react"
import classNames from "classnames"
import { descriptionBreakpointMap, StyledDescriptionRoot } from "./styles"

export type ResponsiveCols = { cols: number } & Partial<Record<Breakpoint, number>>

type DescriptionRootProps = {
  children?: ReactNode
  cols: number | ResponsiveCols
}

export function DescriptionRoot({ children, cols }: DescriptionRootProps) {
  const { style, className } = useMemo(() => {
    if (typeof cols === "number") return { style: { "--description-root-columns": cols } as CSSProperties }

    return {
      style: {
        "--description-root-columns": cols.cols,
        "--description-root-columns-xs": cols.xs,
        "--description-root-columns-sm": cols.sm,
        "--description-root-columns-md": cols.md,
        "--description-root-columns-lg": cols.lg,
        "--description-root-columns-xl": cols.xl,
      } as CSSProperties,
      className: classNames([
        cols.xs && descriptionBreakpointMap.xs,
        cols.sm && descriptionBreakpointMap.sm,
        cols.md && descriptionBreakpointMap.md,
        cols.lg && descriptionBreakpointMap.lg,
        cols.xl && descriptionBreakpointMap.xl,
      ]),
    }
  }, [cols])

  return (
    <StyledDescriptionRoot className={className} style={style}>
      {children}
    </StyledDescriptionRoot>
  )
}

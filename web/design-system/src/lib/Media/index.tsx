import { ReactNode } from "react"
import { Breakpoint } from "@pigment-css/react"
import classNames from "classnames"
import { MediaContainer, mediaFromMap, mediaToMap, themeMap } from "./styles"

type MediaProps = {
  from?: Breakpoint
  to?: Breakpoint
  theme?: "dark" | "light"
  children?: ReactNode
}

export function Media({ children, ...props }: MediaProps) {
  const className = cssMedia(props)

  return <MediaContainer className={className}>{children}</MediaContainer>
}

export function cssMedia({ from, to, theme }: MediaProps) {
  const cssFrom = from ? mediaFromMap[from] : undefined
  const cssTo = to ? mediaToMap[to] : undefined
  const cssTheme = theme ? themeMap[theme] : undefined

  if (!cssFrom && !cssTo && !cssTheme) return undefined

  return classNames(cssFrom, cssTo, cssTheme)
}

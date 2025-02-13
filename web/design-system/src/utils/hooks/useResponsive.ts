import { useMedia } from "react-use"
import { Breakpoint } from "@pigment-css/react"
import { defaultTheme } from "../../theme"

export function useIsUp(breakpoint: Breakpoint) {
  return useMedia(`(min-width: ${defaultTheme.breakpoints[breakpoint]}px)`)
}

export function useIsDown(breakpoint: Breakpoint) {
  return useMedia(`(max-width: ${defaultTheme.breakpoints[breakpoint] - 0.1}px)`)
}

export function useIsMobile() {
  return useIsDown("sm")
}

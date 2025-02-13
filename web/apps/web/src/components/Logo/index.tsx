import { ThemeComponent } from "../ThemeComponent"
import LogoDark from "./logo_dark.svg?react"
import LogoLight from "./logo_light.svg?react"

export function Logo() {
  return <ThemeComponent dark={<LogoDark />} light={<LogoLight />} />
}

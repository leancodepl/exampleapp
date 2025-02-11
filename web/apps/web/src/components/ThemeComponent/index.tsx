import { ReactNode } from "@tanstack/react-router"
import { Media } from "@web/design-system"

type ThemeComponentProps = {
  light?: ReactNode
  dark?: ReactNode
}

export function ThemeComponent({ light, dark }: ThemeComponentProps) {
  return (
    <>
      {light && <Media theme="light">{light}</Media>}
      {dark && <Media theme="dark">{dark}</Media>}
    </>
  )
}

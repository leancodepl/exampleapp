import { ReactNode } from "react"
import { AppTopbar } from "../AppTopbar"
import { AuthorizedLayoutRoot, LayoutContainer } from "./styles"

type AuthorizedLayoutProps = {
  children?: ReactNode
}

export function AuthorizedLayout({ children }: AuthorizedLayoutProps) {
  return (
    <AuthorizedLayoutRoot>
      <AppTopbar />

      <LayoutContainer>{children}</LayoutContainer>
    </AuthorizedLayoutRoot>
  )
}

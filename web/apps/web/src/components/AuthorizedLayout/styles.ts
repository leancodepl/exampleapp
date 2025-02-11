import { styled } from "@pigment-css/react"
import { Container } from "../Container"

export const AuthorizedLayoutRoot = styled.main`
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-width: 0;
  max-width: 100vw;
  min-height: 100lvh;
`

export const LayoutContainer = styled(Container)`
  flex: 1 1 auto;
`

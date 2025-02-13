import { styled } from "@pigment-css/react"
import { breakpoints, spacing } from "@web/design-system"

export const OidcProviders = styled.div`
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: ${spacing._4};

  ${breakpoints.down.xs} {
    grid-template-columns: 1fr;
  }
`

import { styled } from "@pigment-css/react"
import { breakpoints, colors, radii, spacing } from "@web/design-system"

export const ProfileContainerContainer = styled.div`
  ${breakpoints.up.sm} {
    padding: ${spacing._4};

    border: 1px solid ${colors.foreground.default.tertiary};
    border-radius: ${radii.md};
  }
`

export const ProfileContainerContent = styled.div`
  max-width: 432px;
`

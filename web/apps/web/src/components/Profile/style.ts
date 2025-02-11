import { styled } from "@pigment-css/react"
import { colors, spacing } from "@web/design-system"

export const ProfileTabsSeparator = styled.div`
  height: 1px;
  margin: 2px -${spacing._6} 0;

  background: ${colors.foreground.default.tertiary};
`

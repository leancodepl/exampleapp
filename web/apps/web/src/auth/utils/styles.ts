import { styled } from "@pigment-css/react"
import { colors } from "@web/design-system"

export const ProfileOidcTick = styled.div`
  width: 18px;
  height: 18px;
  margin: 3px;

  background: ${colors.background.default.secondary};
  border-radius: 100%;
`

export const ProfileOidcTickChecked = styled(ProfileOidcTick)`
  color: ${colors.foreground.inverse.primary};

  background: ${colors.background.accent.primary};
`

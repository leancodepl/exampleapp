import { styled } from "@pigment-css/react"
import { colors } from "@web/design-system"
import { Logo } from "../Logo"

export const SidebarLogo = styled(Logo)`
  flex: 0 0 auto;
`

export const SidebarIcon = styled.div`
  display: contents;

  color: ${colors.foreground.default.secondary};
`
